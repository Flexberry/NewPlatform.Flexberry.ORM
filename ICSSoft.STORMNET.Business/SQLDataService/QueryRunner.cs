namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    using static ICSSoft.STORMNET.Business.SQLDataService;

    /// <summary>
    /// Класс для запуска запросов на изменение (INSERT/UPDATE/DELETE) в нужном порядке.
    /// </summary>
    internal class QueryRunner
    {
        /// <summary>
        /// Список запросов на выполнение для каждой таблицы.
        /// </summary>
        /// <remarks>Ключ - название таблицы. Значение - все запросы этой таблицы.</remarks>
        private readonly Dictionary<string, List<Query>> _queries;

        /// <summary>
        /// Список запланированных операций для каждой таблицы.
        /// </summary>
        /// <remarks>Ключ - название таблицы. Значение - все операции этой таблицы.</remarks>
        private readonly Dictionary<string, OperationType> _tableOperations;

        /// <summary>
        /// SQLDataService, используемый для запуска запросов (необходим только для запуска <see cref="SQLDataService.CustomizeCommand(IDbCommand)"/>).
        /// </summary>
        private readonly SQLDataService _sqlDataService;

        /// <summary>
        /// Класс для запуска запросов.
        /// </summary>
        /// <param name="deleteQueries">Запросы на удаление.</param>
        /// <param name="updateQueries">Запросы на обновление.</param>
        /// <param name="updateFirstQueries">Запросы на обновление (в первую очередь).</param>
        /// <param name="updateLastQueries">Запросы на обновление (в последнюю очередь).</param>
        /// <param name="insertQueries">Запросы на вставку.</param>
        /// <param name="tableOperations">Запланированные типы запросов, которые необходимо выполнить в каждой таблице.</param>
        /// <param name="sqlDataService">SQLDataService, используемый для запуска запросов (необходим только для запуска <see cref="SQLDataService.CustomizeCommand(IDbCommand)"/>).</param>
        internal QueryRunner(
            Dictionary<string, List<string>> deleteQueries,
            Dictionary<string, List<string>> updateQueries,
            Dictionary<string, List<string>> updateFirstQueries,
            Dictionary<string, List<string>> updateLastQueries,
            Dictionary<string, List<string>> insertQueries,
            SortedList tableOperations,
            SQLDataService sqlDataService)
        {
            _queries = new Dictionary<string, List<Query>>();

            IEnumerable<string> tableNames = new[]
            {
                deleteQueries.Keys,
                updateQueries.Keys,
                updateFirstQueries.Keys,
                updateLastQueries.Keys,
                insertQueries.Keys,
            }.SelectMany(x => x).Distinct();

            foreach (string tableName in tableNames)
            {
                _queries[tableName] = new List<Query>();
            }

            foreach (var q in deleteQueries)
            {
                _queries[q.Key].AddRange(
                    q.Value.Select(x => new Query(x, OperationType.Delete)));
            }

            foreach (var q in updateQueries)
            {
                _queries[q.Key].AddRange(
                    q.Value.Select(x => new Query(x, OperationType.Update)));
            }

            foreach (var q in updateFirstQueries)
            {
                _queries[q.Key].AddRange(
                    q.Value.Select(x => new Query(x, OperationType.Update, QueryPriority.First)));
            }

            foreach (var q in updateLastQueries)
            {
                _queries[q.Key].AddRange(
                    q.Value.Select(x => new Query(x, OperationType.Update, QueryPriority.Last)));
            }

            foreach (var q in insertQueries)
            {
                _queries[q.Key].AddRange(
                    q.Value.Select(x => new Query(x, OperationType.Insert)));
            }

            _tableOperations = new Dictionary<string, OperationType>();

            for (int i = 0; i < tableOperations.Count; i++)
            {
                _tableOperations.Add(
                    (string)tableOperations.GetKey(i),
                    (OperationType)tableOperations.GetByIndex(i));
            }

            _sqlDataService = sqlDataService;
        }

        /// <summary>
        /// Запустить запросы в указанном порядке.
        /// </summary>
        /// <param name="queryOrder">Порядок таблиц, в котором будут выполняться запросы.</param>
        /// <param name="dbTransactionWrapper">Экземпляр <see cref="DbTransactionWrapper"/>.</param>
        /// <param name="alwaysThrowException">Выбрасывать исключение (вместо того, чтобы возвращать его).</param>
        /// <param name="taskID">ID бизнес-задачи, в рамках которой выполняется запрос.</param>
        /// <returns>Исключение.</returns>
        internal Exception RunQueries(StringCollection queryOrder, DbTransactionWrapper dbTransactionWrapper, bool alwaysThrowException = false, object taskID = null)
        {
            // Заполним tableOperations значениями OperationType.None для всех таблиц, у которых нет операций.
            foreach (var tableName in queryOrder)
            {
                if (!_tableOperations.ContainsKey(tableName))
                {
                    _tableOperations.Add(tableName, OperationType.None);
                }

                if (!_queries.ContainsKey(tableName))
                {
                    _queries.Add(tableName, new List<Query>());
                }
            }

            Exception ex = null;
            IDbCommand command = dbTransactionWrapper.CreateCommand();
            do
            {
                string tableName = queryOrder[0];

                bool hasPendingDeletes = _tableOperations[tableName].HasFlag(OperationType.Delete);
                bool hasUpdateLast = _queries[tableName].Any(x => x.OperationType == OperationType.Update && x.Priority == QueryPriority.Last);
                if (hasPendingDeletes || hasUpdateLast)
                {
                    break;
                }

                ex = RunQuery(tableName, command, OperationType.Insert, QueryPriority.Normal, alwaysThrowException, taskID);

                if (ex != null)
                {
                    throw ex;
                }

                ex = RunQuery(tableName, command, OperationType.Update, QueryPriority.Normal, alwaysThrowException, taskID);

                if (ex != null)
                {
                    throw ex;
                }

                queryOrder.RemoveAt(0);
            }
            while (queryOrder.Count > 0);

            // сзади чистые Update
            int queryOrderIndex = queryOrder.Count - 1;
            while (queryOrderIndex >= 0)
            {
                string tableName = queryOrder[queryOrderIndex];
                bool hasUpdateLast = _queries[tableName].Any(x => x.OperationType == OperationType.Update && x.Priority == QueryPriority.Last);
                bool isUpdateOnly = _tableOperations[tableName] == OperationType.Update || _tableOperations[tableName] == OperationType.None;
                if (hasUpdateLast || !isUpdateOnly)
                {
                    break;
                }

                ex = RunQuery(tableName, command, OperationType.Update, QueryPriority.Normal, alwaysThrowException, taskID, exactType: true);

                if (ex != null)
                {
                    throw ex;
                }

                queryOrderIndex--;
            }

            foreach (string tableName in queryOrder)
            {
                ex = RunQuery(tableName, command, OperationType.Update, QueryPriority.First, alwaysThrowException, taskID, checkTableOperations: false);

                if (ex != null)
                {
                    throw ex;
                }
            }

            // delete запросы выполняются в обратном порядке
            foreach (string tableName in queryOrder.Cast<string>().Reverse())
            {
                ex = RunQuery(tableName, command, OperationType.Delete, QueryPriority.Normal, alwaysThrowException, taskID);

                if (ex != null)
                {
                    throw ex;
                }
            }

            // А теперь опять с начала
            foreach (string tableName in queryOrder)
            {
                ex = RunQuery(tableName, command, OperationType.Insert, QueryPriority.Normal, alwaysThrowException, taskID);

                if (ex != null)
                {
                    throw ex;
                }

                ex = RunQuery(tableName, command, OperationType.Update, QueryPriority.Normal, alwaysThrowException, taskID);

                if (ex != null)
                {
                    throw ex;
                }
            }

            foreach (string tableName in queryOrder)
            {
                ex = RunQuery(tableName, command, OperationType.Update, QueryPriority.Last, alwaysThrowException, taskID, checkTableOperations: false);

                if (ex != null)
                {
                    throw ex;
                }
            }

            return ex;
        }

        /// <summary>
        /// Запустить запросы в указанном порядке (асинхронно).
        /// </summary>
        /// <param name="queryOrder">Порядок таблиц, в котором будут выполняться запросы.</param>
        /// <param name="dbTransactionWrapperAsync">Экземпляр <see cref="DbTransactionWrapperAsync"/>.</param>
        /// <param name="alwaysThrowException">Выбрасывать исключение (вместо того, чтобы возвращать его).</param>
        /// <param name="taskID">ID бизнес-задачи, в рамках которой выполняется запрос.</param>
        /// <returns>Исключение.</returns>
        internal async Task<Exception> RunQueriesAsync(StringCollection queryOrder, DbTransactionWrapperAsync dbTransactionWrapperAsync, bool alwaysThrowException = false, object taskID = null)
        {
            // Заполним tableOperations значениями OperationType.None для всех таблиц, у которых нет операций.
            foreach (var tableName in queryOrder)
            {
                if (!_tableOperations.ContainsKey(tableName))
                {
                    _tableOperations.Add(tableName, OperationType.None);
                }

                if (!_queries.ContainsKey(tableName))
                {
                    _queries.Add(tableName, new List<Query>());
                }
            }

            Exception ex = null;
            DbCommand command = await dbTransactionWrapperAsync.CreateCommandAsync();
            do
            {
                string tableName = queryOrder[0];

                bool hasPendingDeletes = _tableOperations[tableName].HasFlag(OperationType.Delete);
                bool hasUpdateLast = _queries[tableName].Any(x => x.OperationType == OperationType.Update && x.Priority == QueryPriority.Last);
                if (hasPendingDeletes || hasUpdateLast)
                {
                    break;
                }

                ex = await RunQueryAsync(tableName, command, OperationType.Insert, QueryPriority.Normal, alwaysThrowException, taskID);

                if (ex != null)
                {
                    throw ex;
                }

                ex = await RunQueryAsync(tableName, command, OperationType.Update, QueryPriority.Normal, alwaysThrowException, taskID);

                if (ex != null)
                {
                    throw ex;
                }

                queryOrder.RemoveAt(0);
            }
            while (queryOrder.Count > 0);

            // сзади чистые Update
            int queryOrderIndex = queryOrder.Count - 1;
            while (queryOrderIndex >= 0)
            {
                string tableName = queryOrder[queryOrderIndex];
                bool hasUpdateLast = _queries[tableName].Any(x => x.OperationType == OperationType.Update && x.Priority == QueryPriority.Last);
                bool isUpdateOnly = _tableOperations[tableName] == OperationType.Update || _tableOperations[tableName] == OperationType.None;
                if (hasUpdateLast || !isUpdateOnly)
                {
                    break;
                }

                ex = await RunQueryAsync(tableName, command, OperationType.Update, QueryPriority.Normal, alwaysThrowException, taskID, exactType: true);

                if (ex != null)
                {
                    throw ex;
                }

                queryOrderIndex--;
            }

            foreach (string tableName in queryOrder)
            {
                ex = await RunQueryAsync(tableName, command, OperationType.Update, QueryPriority.First, alwaysThrowException, taskID, checkTableOperations: false);

                if (ex != null)
                {
                    throw ex;
                }
            }

            // delete запросы выполняются в обратном порядке
            foreach (string tableName in queryOrder.Cast<string>().Reverse())
            {
                ex = await RunQueryAsync(tableName, command, OperationType.Delete, QueryPriority.Normal, alwaysThrowException, taskID);

                if (ex != null)
                {
                    throw ex;
                }
            }

            // А теперь опять с начала
            foreach (string tableName in queryOrder)
            {
                ex = await RunQueryAsync(tableName, command, OperationType.Insert, QueryPriority.Normal, alwaysThrowException, taskID);

                if (ex != null)
                {
                    throw ex;
                }

                ex = await RunQueryAsync(tableName, command, OperationType.Update, QueryPriority.Normal, alwaysThrowException, taskID);

                if (ex != null)
                {
                    throw ex;
                }
            }

            foreach (string tableName in queryOrder)
            {
                ex = await RunQueryAsync(tableName, command, OperationType.Update, QueryPriority.Last, alwaysThrowException, taskID, checkTableOperations: false);

                if (ex != null)
                {
                    throw ex;
                }
            }

            return ex;
        }

        /// <summary>
        /// Запустить запросы указанного типа в указанной таблице.
        /// </summary>
        /// <param name="tableName">Название таблицы (в которой нужно запустить запросы).</param>
        /// <param name="command">Команда к БД.</param>
        /// <param name="operationType">Категория запросов, которые нужно выполнить (выполняются все запросы указанной категории).</param>
        /// <param name="priority">Приоритет (порядок выполнения).</param>
        /// <param name="alwaysThrowException">Выбрасывать исключение (вместо того, чтобы возвращать его).</param>
        /// <param name="taskID">ID бизнес-задачи, в рамках которой выполняется запрос.</param>
        /// <param name="exactType">true - точное совпадение типа операции; false - разрешается частичное совпадение (в смешанных операциях напр. insert + update).</param>
        /// <param name="checkTableOperations">Проверять наличие операции в таблице операций. Если запросы указанного типа для текущей таблицы не были запланированы - они выполнены не будут.</param>
        /// <returns>Исключение.</returns>
        internal Exception RunQuery(
            string tableName,
            IDbCommand command,
            OperationType operationType,
            QueryPriority priority = QueryPriority.Normal,
            bool alwaysThrowException = false,
            object taskID = null,
            bool exactType = false,
            bool checkTableOperations = true)
        {
            Exception ex = null;

            OperationType operations = _tableOperations[tableName];
            bool isOperationPlanned = operations.HasFlag(operationType) || !checkTableOperations;
            if (isOperationPlanned)
            {
                var queriesToRun = _queries[tableName]
                    .Where(x =>
                        (exactType ?
                            x.OperationType == operationType :
                            x.OperationType.HasFlag(operationType))
                        && x.Priority == priority)
                    .Select(x => x.Text)
                    .ToList();

                ex = _sqlDataService.RunCommands(queriesToRun, command, taskID, alwaysThrowException);

                bool queriesWereExecuted = queriesToRun.Count > 0 && ex == null;
                if (queriesWereExecuted)
                {
                    _tableOperations[tableName] = Minus(_tableOperations[tableName], operationType);
                }
            }

            return ex;
        }

        /// <summary>
        /// Запустить запросы указанного типа в указанной таблице (асинхронно).
        /// </summary>
        /// <param name="tableName">Название таблицы (в которой нужно запустить запросы).</param>
        /// <param name="command">Команда к БД.</param>
        /// <param name="operationType">Категория запросов, которые нужно выполнить (выполняются все запросы указанной категории).</param>
        /// <param name="priority">Приоритет (порядок выполнения).</param>
        /// <param name="alwaysThrowException">Выбрасывать исключение (вместо того, чтобы возвращать его).</param>
        /// <param name="taskID">ID бизнес-задачи, в рамках которой выполняется запрос.</param>
        /// <param name="exactType">true - точное совпадение типа операции; false - разрешается частичное совпадение (в смешанных операциях напр. insert + update).</param>
        /// <param name="checkTableOperations">Проверять наличие операции в таблице операций. Если запросы указанного типа для текущей таблицы не были запланированы - они выполнены не будут.</param>
        /// <returns>Исключение.</returns>
        internal async Task<Exception> RunQueryAsync(
            string tableName,
            DbCommand command,
            OperationType operationType,
            QueryPriority priority = QueryPriority.Normal,
            bool alwaysThrowException = false,
            object taskID = null,
            bool exactType = false,
            bool checkTableOperations = true)
        {
            Exception ex = null;

            OperationType operations = _tableOperations[tableName];
            bool isOperationPlanned = operations.HasFlag(operationType) || !checkTableOperations;
            if (isOperationPlanned)
            {
                var queriesToRun = _queries[tableName]
                    .Where(x =>
                        (exactType ?
                            x.OperationType == operationType :
                            x.OperationType.HasFlag(operationType))
                        && x.Priority == priority)
                    .Select(x => x.Text)
                    .ToList();

                ex = await _sqlDataService.RunCommandsAsync(queriesToRun, command, taskID, alwaysThrowException);

                bool queriesWereExecuted = queriesToRun.Count > 0 && ex == null;
                if (queriesWereExecuted)
                {
                    _tableOperations[tableName] = Minus(_tableOperations[tableName], operationType);
                }
            }

            return ex;
        }

        private static OperationType Minus(OperationType ops, OperationType value)
        {
            return ops & (~value);
        }
    }

    /// <summary>
    /// Структура для хранения запроса на изменение .
    /// </summary>
    internal class Query
    {
        /// <summary>
        /// Конструктор запроса.
        /// </summary>
        /// <param name="text">Текст команды запроса.</param>
        /// <param name="operationType">Тип запроса (None/Update/Delete/Insert).</param>
        /// <param name="priority">Приоритет выполнения запроса.</param>
        internal Query(string text, OperationType operationType, QueryPriority priority = QueryPriority.Normal)
        {
            this.Text = text;
            this.OperationType = operationType;
            this.Priority = priority;
        }

        /// <summary>
        /// Текст команды запроса.
        /// </summary>
        internal string Text { get; set; }

        /// <summary>
        /// Тип запроса (None/Update/Delete/Insert).
        /// </summary>
        internal OperationType OperationType { get; set; }

        /// <summary>
        /// Приоритет выполнения запроса.
        /// </summary>
        internal QueryPriority Priority { get; set; }
    }

    /// <summary>
    /// Приоритет выполнения запроса.
    /// </summary>
    internal enum QueryPriority
    {
        /// <summary>
        /// Выполняется перед остальными запросами своего вида.
        /// </summary>
        First,

        /// <summary>
        /// Выполняется как обычно.
        /// </summary>
        Normal,

        /// <summary>
        /// Выполняется после остальных запросов своего вида.
        /// </summary>
        Last,
    }
}

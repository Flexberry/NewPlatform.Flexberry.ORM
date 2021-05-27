namespace ICSSoft.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Писалка запросов на обновление, вставку и удаление данных в StringBuilder.
    /// </summary>
    public class ChangesToSqlBTMonitor : ICSSoft.STORMNET.Business.IBusinessTaskMonitor
    {
        private static string _lockConst = "const";

        /// <summary>
        /// Записанные запросы, которые были завершены.
        /// </summary>
        public static string Record
        {
            get
            {
                StringBuilder recorder = new StringBuilder();
                foreach (KeyValuePair<object, QueryWithApprovement> kvp in _queries)
                {
                    if (kvp.Value.Approvement)
                    {
                        recorder.AppendLine(kvp.Value.Query + ";");
                    }
                }

                return recorder.ToString();
            }
        }

        /// <summary>
        /// Все запросы будем писать в массив для того, чтобы подтверждать выполнение (если что-то упало, то такой запрос нам не нужен).
        /// </summary>
        private static Dictionary<object, QueryWithApprovement> _queries = new Dictionary<object, QueryWithApprovement>();

        /// <summary>
        /// Очистить внутренний массив запросов.
        /// </summary>
        public static void Clear()
        {
            lock (_lockConst)
            {
                _queries.Clear();
            }
        }

        private List<string> _filters = null;

        /// <summary>
        /// Фильтровать запросы к таблицам. Настраивается через конфигурационный файл по ключу "ChangesToSqlExcept", перечисленные через запятую.
        /// </summary>
        public List<string> Filters
        {
            get
            {
                if (_filters == null)
                {
                    _filters = new List<string>(new[] { "STORMNETLOCKDATA" });
                    string appSetting = ConfigurationManager.AppSettings["ChangesToSqlExcept"];
                    if (!string.IsNullOrEmpty(appSetting))
                    {
                        string[] exceptStr = appSetting.Split(',');
                        foreach (string s in exceptStr)
                        {
                            string trimmedStr = s.Trim();
                            if (string.IsNullOrEmpty(trimmedStr))
                            {
                                continue;
                            }

                            if (!_filters.Contains(trimmedStr))
                            {
                                _filters.Add(trimmedStr);
                            }
                        }
                    }
                }

                return _filters;
            }
        }

        /// <summary>
        /// Фильтровать лишние события, а не лишние записывать.
        /// </summary>
        /// <param name="input">входная строка.</param>
        /// <param name="id">id задачи.</param>
        private void Print(string input, object id)
        {
            foreach (string filter in Filters)
            {
                if (input.StartsWith("select", true, CultureInfo.InvariantCulture)
                    || input.StartsWith(string.Format("INSERT INTO \"{0}\"", filter), true, CultureInfo.InvariantCulture)
                    || input.StartsWith(string.Format("DELETE FROM \"{0}\"", filter), true, CultureInfo.InvariantCulture)
                    || input.StartsWith(string.Format("UPDATE \"{0}\"", filter), true, CultureInfo.InvariantCulture))
                {
                    return;
                }
            }

            lock (_lockConst)
            {
                _queries.Add(id, new QueryWithApprovement(input, false));
            }
        }

        #region IBusinessTaskMonitor Members

        public object BeginSubTask(string subTask, object taskID)
        {
            Guid id = Guid.NewGuid();
            Print(subTask, id);
            return id;
        }

        public void BeginTask(string taskName, object ID)
        {
            // Print(taskName);
        }

        public object BeginTask(string taskName)
        {
            // Print(taskName);
            return null;
        }

        public void EndSubTask(object subTaskID)
        {
            if (subTaskID == null)
            {
                return;
            }

            // Подтвердим выполнение запроса
            if (_queries.ContainsKey(subTaskID))
            {
                lock (_lockConst)
                {
                    if (_queries.ContainsKey(subTaskID))
                    {
                        _queries[subTaskID].Approvement = true;
                    }
                }
            }

            // это мы не записываем
        }

        public void EndTask(object ID)
        {
            // это мы не записываем
        }

        #endregion

        /// <summary>
        /// Запрос и подтверждение.
        /// </summary>
        private class QueryWithApprovement
        {
            public string Query = string.Empty;
            public bool Approvement = false;

            public QueryWithApprovement(string query, bool approvement)
            {
                Query = query;
                Approvement = approvement;
            }
        }
    }
}

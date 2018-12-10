namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.Convertors;
    using ICSSoft.STORMNET.KeyGen;

    /// <summary>
    ///     Общий вспомогательный класс.
    /// </summary>
    public class PKHelper
    {
        /// <summary>
        ///     Проверить равенство объектов по их KeyGuid.
        /// </summary>
        /// <param name="obj1">Объект 1.</param>
        /// <param name="obj2">Объект 2.</param>
        /// <returns>True - KeyGuid объектов равны, false - в любом ином случае.</returns>
        public static bool EQPK(object obj1, object obj2)
        {
            bool result;
            if (obj1 == null || obj2 == null)
            {
                result = false;
            }
            else
            {
                var key1 = GetKeyByObject(obj1);
                var key2 = GetKeyByObject(obj2);

                if (key1 == null || key2 == null)
                {
                    result = false;
                }
                else
                {
                    result = key1.Equals(key2);
                }
            }

            return result;
        }

        /// <summary>
        ///     Проверить по KeyGuid, что указанный объект содержится в массиве объектов.
        /// </summary>
        /// <param name="obj">Объект, который ищем.</param>
        /// <param name="objs">Массив объектов, в котором ищем.</param>
        /// <returns>True - содержится, false - отсутствует.</returns>
        public static bool PKIn(object obj, params object[] objs)
        {
            return GetKeys(objs).Any(x => EQPK(obj, x));
        }

        /// <summary>
        ///     Сравнить родителя и переданный объект по KeyGuid.
        /// </summary>
        /// <param name="dataObject">Объект, чей родитель проверяется.</param>
        /// <param name="parent">Объект, с которым будет сравниваться родитель.</param>
        /// <param name="propertyName">Имя родительского свойства в объекте.</param>
        /// <returns>True - равны, false - неравны или dataObject null.</returns>
        public static bool EQParentPK(DataObject dataObject, object parent, string propertyName = "Иерархия")
        {
            bool result;
            if (dataObject == null)
            {
                result = false;
            }
            else
            {
                var parentValue = Information.GetPropValueByName(dataObject, propertyName);
                result = EQPK(parentValue, parent);
            }

            return result;
        }

        /// <summary>
        ///     Сравнить объекты по KeyGuid.
        /// </summary>
        /// <param name="dataObject1">Объект 1.</param>
        /// <param name="dataObject2">Объект 2.</param>
        /// <param name="checkType">Проверять тип объектов.</param>
        /// <returns>True - равны или оба null, else - неравны или только один null.</returns>
        public static bool EQDataObject(DataObject dataObject1, DataObject dataObject2, bool checkType)
        {
            bool result;
            if (dataObject1 == null && dataObject2 == null)
            {
                result = true;
            }
            else if (dataObject1 == null || dataObject2 == null)
            {
                result = false;
            }
            else
            {
                if (checkType)
                {
                    result = dataObject1.GetType() == dataObject2.GetType() &&
                             EQPK(dataObject1, dataObject2);
                }
                else
                {
                    result = EQPK(dataObject1, dataObject2);
                }
            }

            return result;
        }

        /// <summary>
        ///     Сравнить объекты по KeyGuid.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="dataObject1">Объект 1.</param>
        /// <param name="dataObject2">Объект 2.</param>
        /// <returns>True - равны или оба null, false - неравны или только один null.</returns>
        public static bool EQDataObject<T>(T dataObject1, T dataObject2)
            where T : DataObject
        {
            bool result;
            if (dataObject1 == null && dataObject2 == null)
            {
                result = true;
            }
            else if (dataObject1 == null || dataObject2 == null)
            {
                result = false;
            }
            else
            {
                result = EQPK(dataObject1, dataObject2);
            }

            return result;
        }

        /// <summary>
        ///     Из Guid, KeyGuid, DataObject или string извлекает Guid.
        /// </summary>
        /// <param name="obj">Объект для преобразования.</param>
        /// <returns>Guid объекта.</returns>
        public static Guid? GetGuidByObject(object obj)
        {
            return GetKeyByObject(obj)?.Guid;
        }

        /// <summary>
        ///     Из Guid, KeyGuid, DataObject или string извлекает KeyGuid.
        /// </summary>
        /// <param name="obj">Объект для преобразования.</param>
        /// <returns>KeyGuid объекта, null - невозможно преобразовать к KeyGuid.</returns>
        public static KeyGuid GetKeyByObject(object obj)
        {
            KeyGuid result = null;

            var guid = obj as KeyGuid;
            if (guid != null)
            {
                result = guid;
            }
            else
            {
                if (obj is Guid)
                {
                    result = new KeyGuid((Guid)obj);
                }
                else
                {
                    var o = obj as DataObject;
                    if (o != null)
                    {
                        var kg = o.__PrimaryKey as KeyGuid;
                        if (kg != null)
                        {
                            result = kg;
                        }
                        else if (InOperatorsConverter.CanConvert(o.__PrimaryKey.GetType(), typeof(KeyGuid)))
                        {
                            result = InOperatorsConverter.Convert(o.__PrimaryKey, typeof(KeyGuid)) as KeyGuid;
                        }
                    }
                    else
                    {
                        string s = obj as string;
                        if (s != null && KeyGuid.IsGuid(s))
                        {
                            result = new KeyGuid(s);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     Получить юникальные ключи.
        /// </summary>
        /// <param name="values">
        ///     Перечисление объектов, содержащих ключи.
        ///     В т.ч. допускаются списки, списки списков и тд и тп.
        /// </param>
        /// <returns>Массив ключей.</returns>
        public static KeyGuid[] GetKeys(params object[] values)
        {
            var res = new List<KeyGuid>();
            foreach (var val in values)
            {
                var kg = GetKeyByObject(val);
                if (kg != null)
                {
                    res.Add(kg);
                }

                var guids = val as IEnumerable<Guid>;
                if (guids != null)
                {
                    res.AddRange(guids.Select(x => GetKeyByObject(x)));
                }

                var objs = val as IEnumerable<object>;
                if (objs != null)
                {
                    res.AddRange(objs.SelectMany(x => GetKeys(x)));
                }
            }

            return res.Distinct().ToArray();
        }

        /// <summary>
        ///     Преобразовать перечисление объектов в строку ключей.
        ///     Основное использование - передача в SQL-запросы.
        /// </summary>
        /// <param name="objs">Перечисление объектов.</param>
        /// <returns>Строка ключей в формате D через запятую, обернутых в ''.</returns>
        public static string GetKeysString(params object[] objs)
        {
            return string.Join(
                ",",
                GetKeys(objs).Select(o => $"'{o.Guid}'").ToArray());
        }

        /// <summary>
        ///     Получение DataObject с проинициализированным первичным ключом.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="pk">Первичный ключ.</param>
        /// <exception cref="ArgumentException">Аргумент не является первичным ключом.</exception>
        /// <returns>DataObject с проинициализированным первичным ключом.</returns>
        public static T CreateDataObject<T>(object pk)
            where T : DataObject, new()
        {
            var obj = FastAllocator<T>.New();
            var key = GetKeyByObject(pk);
            if (key == null)
            {
                throw new ArgumentException();
            }

            obj.SetExistObjectPrimaryKey(key);
            return obj;
        }

        /// <summary>
        ///     Получение DataObject[] с проинициализированными первичными ключами.
        /// </summary>
        /// <param name="values">
        ///     Перечисление объектов, содержащих ключи.
        ///     В т.ч. допускаются списки, списки списков и тд и тп.
        /// </param>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <returns>DataObject[] с проинициализированным первичным ключом.</returns>
        public static T[] CreateObjectsByKey<T>(params object[] values)
            where T : DataObject, new()
        {
            return GetKeys(values).Select(CreateDataObject<T>).ToArray();
        }
    }
}
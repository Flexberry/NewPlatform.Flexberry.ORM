using System;
using System.Collections.Generic;

namespace ICSSoft.STORMNET.KeyGen
{
    /// <summary>
    /// Статический класс генерации ключей, через который генерируются все первичные ключи
    /// Он использует для генерации ключа генераторы, наследуемые от BaseKeyGenerator.
    /// Этот конкретный генератор прописывается непосредственно объекту данных специальным атрибутом <see cref="KeyGeneratorAttribute"/>.
    /// </summary>
    public class KeyGenerator
    {
        //TODO: Проверить скорость работы, если заменить на Dictionary<Type, BaseKeyGenerator>
        //private static ICSSoft.STORMNET.Collections.TypeBaseCollection cacheGenerators = new ICSSoft.STORMNET.Collections.TypeBaseCollection();
        private static Dictionary<Type, BaseKeyGenerator> cacheGenerators = new Dictionary<Type, BaseKeyGenerator>();

        private static Dictionary<Type, Type> _cacheKeys = new Dictionary<Type, Type>();

        /// <summary>
        /// Prevents a default instance of the <see cref="KeyGenerator"/> class from being created.
        /// </summary>
        private KeyGenerator()
        {
        }

        /*
        private static bool? fIsWebApp = null;

        /// <summary>
        /// Приложение является веб-приложением
        /// </summary>
        public static bool IsWebApp
        {
            get
            {
                if (fIsWebApp == null)
                {
                    fIsWebApp = (System.Web.HttpContext.Current != null);
                }
                return fIsWebApp.Value;
            }
        }
        */

        /// <summary>
        /// Возвращает непосредственно генератор, производный от <see cref="BaseKeyGenerator"/>.
        /// Удобно, если требуется использовать у этого генератора методы,
        /// отличные от имеющихся в BaseKeyGenerator.
        /// </summary>
        public static BaseKeyGenerator Generator(DataObject dataobject)
        {
            return Generator(dataobject.GetType());
        }

        private static string m_ObjNull = "CONST";

        /// <summary>
        /// Возвращает непосредственно генератор, производный от <see cref="BaseKeyGenerator"/>.
        /// Удобно, если требуется использовать у этого генератора методы,
        /// отличные от имеющихся в BaseKeyGenerator.
        /// </summary>
        public static BaseKeyGenerator Generator(Type dataobjecttype)
        {
            BaseKeyGenerator genrtor;
            if (cacheGenerators.ContainsKey(dataobjecttype))
                genrtor = cacheGenerators[dataobjecttype];
            else
            {
                lock (m_ObjNull)
                {
                    if (cacheGenerators.ContainsKey(dataobjecttype))
                        genrtor = cacheGenerators[dataobjecttype];
                    else
                    {
                        Type keygentype = Information.GetKeyGeneratorType(dataobjecttype);
                        genrtor = (BaseKeyGenerator)keygentype.GetConstructor(new Type[] { }).Invoke(new object[] { });
                        cacheGenerators.Add(dataobjecttype, genrtor);
                    }
                }
            }

            return genrtor;
        }


        /// <summary>
        /// Сгенерировать ключ и установить его в объект данных
        /// </summary>
        public static object GenerateUnique(DataObject dataobject, object sds)
        {
            if (!dataobject.PrimaryKeyIsUnique)
            {
                dataobject.__PrimaryKey = GenerateUnique(dataobject.GetType(), sds);
                dataobject.PrimaryKeyIsUnique = true;
            }
            return dataobject.__PrimaryKey;
        }

        /// <summary>
        /// Сгенерировать ключ и установить его в объект данных
        /// </summary>
        public static object Generate(DataObject dataobject, object sds)
        {
            Type type = dataobject.GetType();
            dataobject.__PrimaryKey = Generate(type, sds);
            dataobject.PrimaryKeyIsUnique = Generator(type).Unique;
            return dataobject.__PrimaryKey;
        }

        /// <summary>
        /// Сгенерировать ключ
        /// </summary>
        public static object Generate(Type dataobjecttype, object sds)
        {
            BaseKeyGenerator keyGen = Generator(dataobjecttype);
            return keyGen.Generate(dataobjecttype, sds);
        }

        /// <summary>
        /// Сгенерировать ключ
        /// </summary>
        public static object GenerateUnique(Type dataobjecttype, object sds)
        {
            BaseKeyGenerator keyGen = Generator(dataobjecttype);
            return keyGen.GenerateUniqe(dataobjecttype, sds);
        }

        /// <summary>
        /// Возвращает тип ключа (например, для GUIDGenerator это typeof(Guid))
        /// </summary>
        public static Type KeyType(DataObject dataobject)
        {
            return KeyType(dataobject.GetType());
        }

        /// <summary>
        /// Возвращает тип ключа (например, для GUIDGenerator это typeof(Guid))
        /// </summary>
        public static Type KeyType(Type dataobjecttype)
        {
            // сработаем через кэш - выйграем время на вызовах лишних методов
            if (_cacheKeys.ContainsKey(dataobjecttype))
            {
                return _cacheKeys[dataobjecttype];
            }

            lock (m_ObjNull)
            {
                if (_cacheKeys.ContainsKey(dataobjecttype))
                {
                    return _cacheKeys[dataobjecttype];
                }

                BaseKeyGenerator keyGen = Generator(dataobjecttype);
                Type keyType = keyGen.KeyType;
                _cacheKeys.Add(dataobjecttype, keyType);
                return keyType;
            }
        }
    }
}

using System;

namespace ICSSoft.STORMNET.FunctionalLanguage
{
    /// <summary>
    /// Определение переменной в ограничении (обычно указывает на атрибут в объекте).
    /// </summary>
    [NotStored]
    public class VariableDef : TypedObject
    {
        /// <summary>
        /// пустой конструктор.
        /// </summary>
        public VariableDef()
        {
        }

        /// <summary>
        /// Определение переменной в ограничении (обычно указывает на атрибут в объекте).
        /// </summary>
        /// <param name="objType">ObjectType-Тип переменной. (Например, langdef.StringType).</param>
        /// <param name="objStringedView">Собственно имя свойства объекта, по которому собираемся строить ограничение.</param>
        /// <param name="objCaption"></param>
        public VariableDef(ObjectType objType, string objStringedView, string objCaption)
            : base(objType, objStringedView, objCaption)
        {
        }

        /// <summary>
        /// Самый распространённый конструктор, который используется при построении ограничений.
        /// </summary>
        /// <param name="objType">ObjectType-Тип переменной. (Например, langdef.StringType).</param>
        /// <param name="objStringedView">Собственно имя свойства объекта, по которому собираемся строить ограничение.</param>
        public VariableDef(ObjectType objType, string objStringedView)
            : base(objType, objStringedView, objStringedView)
        {
        }

        /// <summary>
        /// Определение переменной в ограничении (обычно указывает на атрибут в объекте).
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="property"></param>
        /// <param name="objCaption"></param>
        /// <param name="ldef"></param>
        public VariableDef(Type baseType, string property, string objCaption, FunctionalLanguageDef ldef)
            : base(ldef.GetObjectTypeForNetType(Information.GetPropertyType(baseType, property)), property, objCaption)
        {
            ldef.Variables.AddObject(this);
        }

        /// <summary>
        /// Определение переменной в ограничении (обычно указывает на атрибут в объекте).
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="property"></param>
        /// <param name="ldef"></param>
        public VariableDef(Type baseType, string property, FunctionalLanguageDef ldef)
            : base(ldef.GetObjectTypeForNetType(Information.GetPropertyType(baseType, property)), property, property)
        {
            ldef.Variables.AddObject(this);
        }

        private FunctionalLanguageDef fieldLanguage;

        /// <summary>
        /// Язык описания ограничений.
        /// </summary>
        [ICSSoft.STORMNET.Agregator]
        public FunctionalLanguageDef Language
        {
            get { return fieldLanguage; }
            set { fieldLanguage = value; }
        }

        /// <summary>
        /// вместо сериализации.
        /// </summary>
        /// <returns></returns>
        public virtual object ToSimpleValue()
        {
            if (Type != null)
            {
                return new[] { Type.StringedView, StringedView, Caption };
            }

            return new[] { string.Empty, StringedView, Caption };
        }

        /// <summary>
        /// Делегат для получения типа по его имени в методе SimpleValueToDataObject.
        /// </summary>
        public static TypeResolveDelegate ExtraTypeResolver = null;

        /// <summary>
        /// вместо десериализации (вполне может выдать Exception, если тип переменной не будет найден в ldef или Type.GetType((string[])value[0]), так что try-catch снаружи крайне рекомендуется).
        /// </summary>
        /// <param name="value">Массив в который закручен наш VariableDef.</param>
        /// <param name="ldef">Определение языка.</param>
        public virtual void FromSimpleValue(object value, FunctionalLanguageDef ldef)
        {
            var vals = (string[])value;
            switch (vals[0])
            {
                case "Int32":
                    vals[0] = "System.Int32";
                    break;
            }

            Type = ldef.GetObjectType(vals[0]);

            // Шлыков
            if (Type == null)
            {
                if (string.IsNullOrEmpty(vals[0]))
                {
                    vals[0] = "System.String";
                }

                Type tp = null;

                try
                {
                    tp = System.Type.GetType(vals[0], false);
                }
                catch (Exception)
                {
                    if (ExtraTypeResolver != null)
                    {
                        tp = ExtraTypeResolver.Invoke(vals[0]);
                    }
                }

                // Братчиков 09.12.2008 Баг при десереализации
                if (tp == null)
                {
                    string s = vals[0].ToLower();
                    switch (s)
                    {
                        case "nullabledatetime":
                            tp = System.Type.GetType("ICSSoft.STORMNET.UserDataTypes.NullableDateTime,ICSSoft.STORMNET.UserDataTypes");
                            break;
                        case "nullabledecimal":
                            tp = System.Type.GetType("ICSSoft.STORMNET.UserDataTypes.NullableDecimal,ICSSoft.STORMNET.UserDataTypes");
                            break;
                        case "nullableint":
                            tp = System.Type.GetType("ICSSoft.STORMNET.UserDataTypes.NullableInt,ICSSoft.STORMNET.UserDataTypes");
                            break;
                        case "keyguid":
                            tp = System.Type.GetType("ICSSoft.STORMNET.KeyGen.KeyGuid,ICSSoft.STORMNET.DataObject");
                            break;
                        default: throw new Exception(string.Format("Невозможно найти тип: {0}", vals[0]));
                    }
                }

                // Братчиков 09.12.2008 Баг при десереализации
                Type = new ObjectType(tp.AssemblyQualifiedName, tp.Name, tp);
                ldef.Types.AddObject(Type);
            }

            StringedView = vals[1];
            if (vals.Length > 2)
            {
                Caption = vals[2];
            }

            if (Caption == null)
            {
                Caption = StringedView;
            }
        }
    }

    /// <summary>
    /// Делегат для получения типа по его имени (используется в особых случаях, когда стандартные методы почему-то не помогают).
    /// </summary>
    /// <param name="typeName">Имя типа.</param>
    /// <returns> Сформированный по имени тип. </returns>
    public delegate Type TypeResolveDelegate(string typeName);
}

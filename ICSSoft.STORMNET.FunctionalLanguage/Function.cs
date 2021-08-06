namespace ICSSoft.STORMNET.FunctionalLanguage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;

    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

    /// <summary>
    /// Ограничивающая функция. Применяется для задания ограничений.
    /// </summary>
    [Serializable]
    public class Function : ISerializable, IEquatable<Function>
    {
        private FunctionDef _fieldFunctionDef;

        private ArrayList fieldParameters = new ArrayList();

        /// <summary>
        /// Константа для сериализации.
        /// </summary>
        private const string FuncName = "Function";

        /// <summary>
        /// Пустой конструктор по-умолчанию.
        /// </summary>
        public Function()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="functionDefinition">Определение функции.</param>
        /// <param name="parameters">Параметры.</param>
        public Function(FunctionDef functionDefinition, params object[] parameters)
        {
            _fieldFunctionDef = functionDefinition;
            fieldParameters.AddRange(parameters);
        }

        /// <summary>
        /// Конструктор для десереализации.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="text"></param>
        public Function(SerializationInfo info, StreamingContext text)
            : this()
        {
            // первый вариант десериализации, без учета функций, входящих в ExternalLangDef
            // var f = SQLWhereLanguageDef.LanguageDef.FunctionFromSimpleStruct(
            //    Tools.ToolBinarySerializer.ObjectFromString(info.GetString(FuncName)));

            Function f;

            // Попытка использовать ExternalLangDef, так как там
            // содержится больше функций, чем в SQLWhereLangDef
            var externalLangDef = Type.GetType(
                "ICSSoft.STORMNET.Windows.Forms.ExternalLangDef, ICSSoft.STORMNET.Business.ExternalLangDef, Version=1.0.0.0, " +
                "Culture=neutral, PublicKeyToken=50dc27591ed591e6",
                false);
            if (externalLangDef == null)
            {
                externalLangDef = Type.GetType(
                    "ICSSoft.STORMNET.Windows.Forms.ExternalLangDef, ICSSoft.STORMNET.UI, Version=1.0.0.1, " +
                    "Culture=neutral, PublicKeyToken=21ce651d390c1fa0",
                    false);
            }

            if (externalLangDef != null)
            {
                MethodInfo functionFromSimpleStruct = externalLangDef.GetMethod("FunctionFromSimpleStruct");
                PropertyInfo languageDef = externalLangDef.GetProperty("LanguageDef");
                f = (Function)functionFromSimpleStruct.Invoke(languageDef.GetValue(null, null),
                                                    new object[]
                                                        {
                                                            Tools.ToolBinarySerializer.ObjectFromString(
                                                                info.GetString(FuncName)),
                                                        });
            }
            else
            {
                // Если загрузка ExternalLangDef не удалась
                f = SQLWhereLanguageDef.LanguageDef.FunctionFromSimpleStruct(
                        Tools.ToolBinarySerializer.ObjectFromString(info.GetString(FuncName)));
            }

            fieldParameters = f.fieldParameters;
            _fieldFunctionDef = f._fieldFunctionDef;
        }

        /// <summary>
        /// Сериализация.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            string val =
                Tools.ToolBinarySerializer.ObjectToString(SQLWhereLanguageDef.LanguageDef.FunctionToSimpleStruct(this));
            info.AddValue(FuncName, val);
        }

        /// <summary>
        /// Получить свойства, участвующие в ограничении.
        /// </summary>
        /// <returns>Список свойств из ограничения.</returns>
        public string[] GetLimitProperties()
        {
            var res = new List<string>();
            InternalGetLimitProperties(this, res);

            return res.ToArray();
        }

        private static void InternalGetLimitProperties(Function lf, IList arl)
        {
            foreach (object obj in lf.Parameters)
            {
                if (obj is VariableDef)
                {
                    // это имя свойства
                    arl.Add((obj as VariableDef).StringedView);
                }
                else if (obj is Function)
                {
                    // спускаемся вниз
                    InternalGetLimitProperties(obj as Function, arl);
                }
            }
        }

        /// <summary>
        /// Определение функции.
        /// </summary>
        public FunctionDef FunctionDef
        {
            get { return _fieldFunctionDef; }
            set { _fieldFunctionDef = value; }
        }

        /// <summary>
        /// Массив параметров.
        /// </summary>
        public ArrayList Parameters
        {
            get { return fieldParameters; }
        }

        /// <summary>
        /// Переопределяем сравнение функций (сравнение идёт по функциям, получаемым методом ToString).
        /// </summary>
        /// <param name="otherFunction"> Функция, с которой идёт сравнение на равенство текущей функции. </param>
        /// <returns> True, если значение ToString совпало. </returns>
        public bool Equals(Function otherFunction)
        {
            if (otherFunction == null)
            {
                return false;
            }

            return ToString() == otherFunction.ToString();
        }

        /// <summary>
        /// Переопределяем сравнение функций (сравнение идёт по функциям, получаемым методом ToString).
        /// </summary>
        /// <param name="obj"> Объект, с которым идёт сравнение (если это не Function, то вернётся null). </param>
        /// <returns> True, если значение ToString совпало. </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Function))
            {
                return false;
            }

            var functionObj = obj as Function;
            return Equals(functionObj);
        }

        /// <summary>
        /// Получаем хэш-код (для реализации переопределения сравнения хэш-код считается от значения, получаемого через ToString).
        /// </summary>
        /// <returns> Получаемый хэш-код. </returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Переопределеяем оператор "==", чтобы сравнение шло как ToString.
        /// </summary>
        /// <param name="function1"> Первая сравниваемая функция. </param>
        /// <param name="function2"> Вторая сравниваемая функция. </param>
        /// <returns> Являются ли ToString функций равными. </returns>
        public static bool operator ==(Function function1, Function function2)
        {
            if ((object)function1 == null || ((object)function2) == null)
            {
                return object.Equals(function1, function2);
            }

            return function1.Equals(function2);
        }

        /// <summary>
        /// Переопределеяем оператор "!=", чтобы сравнение шло как ToString.
        /// </summary>
        /// <param name="function1"> Первая сравниваемая функция. </param>
        /// <param name="function2"> Вторая сравниваемая функция. </param>
        /// <returns> Являются ли ToString функций неравными. </returns>
        public static bool operator !=(Function function1, Function function2)
        {
            if ((object)function1 == null || ((object)function2) == null)
            {
                return !object.Equals(function1, function2);
            }

            return !function1.Equals(function2);
        }

        /// <summary>
        /// в строку.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(_fieldFunctionDef.StringedView);
            stringBuilder.Append(" (");
            foreach (object par in fieldParameters)
            {
                if (par is ViewedObject)
                {
                    stringBuilder.Append(" ");
                    stringBuilder.Append(((ViewedObject)par).StringedView);
                }
                else if (par != null)
                {
                    stringBuilder.Append(" ");
                    stringBuilder.Append(par);
                    stringBuilder.Append(" ");
                }
                else
                {
                    stringBuilder.Append(" NULL ");
                }
            }

            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Преобразование функции в строковое дружественное пользователю представление.
        /// </summary>
        /// <returns></returns>
        public string ToUserFriendlyString()
        {
            var parameters = new object[Parameters.Count];
            for (var i = 0; i < parameters.Length; i++)
            {
                var par = Parameters[i];
                if (par is VariableDef)
                {
                    parameters[i] = (par as VariableDef).Caption;
                }
                else if (par is Function)
                {
                    parameters[i] = (par as Function).ToUserFriendlyString();
                }
                else
                {
                    parameters[i] = par;
                }
            }

            if (FunctionDef.StringedView == SQLWhereLanguageDef.LanguageDef.funcEQ
                && parameters.Length == 1)
            {
                return string.Format("{0}", parameters);
            }

            var frm = Convertors.Formatter.transfertformat(FunctionDef.UserViewFormat, parameters.Length);
            return string.Format(frm, parameters);
        }

        /// <summary>
        /// Проверка с вложениями.
        /// </summary>
        public void CheckWithSubFolders()
        {
            Check(true);
        }

        /// <summary>
        /// Проверка без вложений.
        /// </summary>
        public void CheckWithOutSubFolders()
        {
            Check(false);
        }

        /// <summary>
        /// Проверка совместимости функции и параметров без выбрасывания эксепшенов.
        /// </summary>
        /// <returns></returns>
        public bool CheckWithoutSubFoldersSafetly()
        {
            return CheckSafetly(false);
        }

        /// <summary>
        /// Клонирование функции.
        /// </summary>
        /// <returns></returns>
        public Function Clone()
        {
            FunctionDef fdcopy = FunctionDef;

            object[] newpars = new object[Parameters.Count];
            for (int i = 0; i < newpars.Length; i++)
            {
                if (Parameters[i] is Function)
                {
                    newpars[i] = (Parameters[i] as Function).Clone();
                }
                else
                {
                    newpars[i] = Parameters[i];
                }
            }

            return new Function(FunctionDef, newpars);
        }

        /// <summary>
        /// Проверить соответствие функции и параметров без выбрасывания эксепшенов.
        /// </summary>
        /// <param name="checkSubFunctions"></param>
        /// <returns></returns>
        public bool CheckSafetly(bool checkSubFunctions)
        {
            if (_fieldFunctionDef == null)
            {
                return false;

                // throw new NullFunctionDefException();
            }

            if ((fieldParameters.Count != _fieldFunctionDef.Parameters.Count)
                && (fieldParameters.Count > _fieldFunctionDef.Parameters.Count)
                && (!_fieldFunctionDef.Parameters[_fieldFunctionDef.Parameters.Count - 1].MultiValueSupport))
            {
                return false;

                // throw new ParameterCountException();
            }

            for (int i = 0; i < fieldParameters.Count; i++)
            {
                ObjectType parameterDefType = (i >= _fieldFunctionDef.Parameters.Count) ? _fieldFunctionDef.Parameters[_fieldFunctionDef.Parameters.Count - 1].Type : _fieldFunctionDef.Parameters[i].Type;
                if (fieldParameters[i] is Function)
                {
                    ObjectType parameterType = (fieldParameters[i] as Function).FunctionDef.ReturnType;
                    if (parameterType != parameterDefType)
                    {
                        if (CompatibilityTypeTest.Check(parameterType.NetCompatibilityType, parameterDefType.NetCompatibilityType) == TypesCompatibilities.No)
                        {
                            return false;
                        }

                        if (checkSubFunctions)
                        {
                            (fieldParameters[i] as Function).Check(true);
                        }
                    }
                }
                else if (fieldParameters[i] is VariableDef)
                {
                    if (CompatibilityTypeTest.Check((fieldParameters[i] as VariableDef).Type.NetCompatibilityType, parameterDefType.NetCompatibilityType) == TypesCompatibilities.No)
                    {
                        return false;
                    }
                }
                else
                {
                    if (CompatibilityTypeTest.Check(fieldParameters[i].GetType(), parameterDefType.NetCompatibilityType) == TypesCompatibilities.No)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Проверка функции с выбросом эксепшенов.
        /// </summary>
        /// <param name="checkSubFunctions"></param>
        /// <exception cref="NullFunctionDefException"></exception>
        /// <exception cref="ParameterCountException"></exception>
        /// <exception cref="UncompatibleParameterTypeException"></exception>
        public void Check(bool checkSubFunctions)
        {
            if (_fieldFunctionDef == null)
            {
                throw new NullFunctionDefException();
            }

            if ((fieldParameters.Count != _fieldFunctionDef.Parameters.Count)
                && (fieldParameters.Count > _fieldFunctionDef.Parameters.Count)
                && (!_fieldFunctionDef.Parameters[_fieldFunctionDef.Parameters.Count - 1].MultiValueSupport))
            {
                throw new ParameterCountException();
            }

            for (int i = 0; i < fieldParameters.Count; i++)
            {
                ObjectType parameterDefType = (i >= _fieldFunctionDef.Parameters.Count)
                                                  ? _fieldFunctionDef.Parameters[
                                                      _fieldFunctionDef.Parameters.Count - 1].Type
                                                  : _fieldFunctionDef.Parameters[i].Type;
                if (fieldParameters[i] is Function)
                {
                    ObjectType parameterType = (fieldParameters[i] as Function).FunctionDef.ReturnType;
                    if (parameterType != parameterDefType)
                    {
                        if (
                            CompatibilityTypeTest.Check(parameterType.NetCompatibilityType,
                                                        parameterDefType.NetCompatibilityType) ==
                            TypesCompatibilities.No)
                        {
                            throw new UncompatibleParameterTypeException(i);
                        }

                        if (checkSubFunctions)
                        {
                            (fieldParameters[i] as Function).Check(true);
                        }
                    }
                }
                else if (fieldParameters[i] is VariableDef)
                {
                    if (
                        CompatibilityTypeTest.Check((fieldParameters[i] as VariableDef).Type.NetCompatibilityType,
                                                    parameterDefType.NetCompatibilityType) ==
                        TypesCompatibilities.No)
                    {
                        throw new UncompatibleParameterTypeException(i);
                    }
                }
                else
                {
                    if (
                        CompatibilityTypeTest.Check(fieldParameters[i].GetType(),
                                                    parameterDefType.NetCompatibilityType) ==
                        TypesCompatibilities.No)
                    {
                        throw new UncompatibleParameterTypeException(i);
                    }
                }
            }
        }
    }
}

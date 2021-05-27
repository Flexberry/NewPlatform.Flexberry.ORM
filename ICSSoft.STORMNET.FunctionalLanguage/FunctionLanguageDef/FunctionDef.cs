using System;

namespace ICSSoft.STORMNET.FunctionalLanguage
{
    using ICSSoft.STORMNET;

    /// <summary>
    /// Определение функции.
    /// </summary>
    [NotStored]
    public class FunctionDef : TypedObject
    {
        private DetailArrayOfFunctionalParameterDef fieldParameters;
        private string fieldUserViewFormat;
        private bool fieldFreeQuery;
        private int fID = 0;

        /// <summary>
        /// Целочисленный ключ определения функции.
        /// </summary>
        public int ID
        {
            get { return fID; }
            set { fID = value; }
        }

        /// <summary>
        /// конструктор.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="returnType"></param>
        /// <param name="objStringedView"></param>
        /// <param name="objCaption"></param>
        /// <param name="objImagedView"></param>
        /// <param name="userViewFormat"></param>
        public FunctionDef(int ID, ObjectType returnType, string objStringedView, string objCaption, string userViewFormat)
            : base(returnType, objStringedView, objCaption)
        {
            fieldParameters = new DetailArrayOfFunctionalParameterDef(this);
            fieldUserViewFormat = userViewFormat;
            this.ID = ID;
        }

        /// <summary>
        /// конструктор.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="returnType"></param>
        /// <param name="objStringedView"></param>
        /// <param name="objCaption"></param>
        /// <param name="objImagedView"></param>
        /// <param name="userViewFormat"></param>
        /// <param name="parameters"></param>
        public FunctionDef(int ID, ObjectType returnType, string objStringedView, string objCaption, string userViewFormat,
            params FunctionParameterDef[] parameters)
            : base(returnType, objStringedView, objCaption)
        {
            fieldParameters = new DetailArrayOfFunctionalParameterDef(this);
            for (int i = 0; i < parameters.Length; i++)
            {
                fieldParameters.Add(parameters[i]);
            }

            fieldUserViewFormat = userViewFormat;
            this.ID = ID;
        }

        /// <summary>
        /// конструктор.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="returnType"></param>
        /// <param name="objStringedView"></param>
        /// <param name="objCaption"></param>
        /// <param name="objImagedView"></param>
        /// <param name="userViewFormat"></param>
        /// <param name="FreeQuery"></param>
        /// <param name="parameters"></param>
        public FunctionDef(int ID, ObjectType returnType, string objStringedView, string objCaption, string userViewFormat, bool FreeQuery,
            params FunctionParameterDef[] parameters)
            : base(returnType, objStringedView, objCaption)
        {
            fieldParameters = new DetailArrayOfFunctionalParameterDef(this);
            for (int i = 0; i < parameters.Length; i++)
            {
                fieldParameters.Add(parameters[i]);
            }

            fieldUserViewFormat = userViewFormat;
            fieldFreeQuery = FreeQuery;
            this.ID = ID;
        }

        /// <summary>
        /// Влияет на генерацию SQL-запроса. Если true, то добавляются все поля.
        /// </summary>
        public bool FreeQuery
        {
            get { return fieldFreeQuery; }
        }

        /// <summary>
        /// Тип возвращаемого значения.
        /// </summary>
        public ObjectType ReturnType
        {
            get { return Type; }
        }

        /// <summary>
        /// Параметры функции.
        /// </summary>
        public DetailArrayOfFunctionalParameterDef Parameters
        {
            get { return fieldParameters; }
        }

        /// <summary>
        /// формат отображения пользователю (используется на форме задания ограничений).
        /// </summary>
        public string UserViewFormat
        {
            get { return fieldUserViewFormat; }
        }

        private FunctionalLanguageDef fieldLanguage;

        /// <summary>
        /// Язык ограничений, в рамках которого существует данное определение функции (язык включает все определения как детейлы).
        /// </summary>
        [ICSSoft.STORMNET.Agregator]
        public FunctionalLanguageDef Language
        {
            get { return fieldLanguage; }
            set { fieldLanguage = value; }
        }
    }

    /// <summary>
    /// массив параметров.
    /// </summary>
    public class DetailArrayOfFunctionalParameterDef : DetailArray
    {
        /// <summary>
        /// конструктор.
        /// </summary>
        /// <param name="masterObj"></param>
        public DetailArrayOfFunctionalParameterDef(FunctionDef masterObj)
            : base(typeof(FunctionParameterDef), masterObj)
        {
        }

        /// <summary>
        /// получить функцию по индексу.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FunctionParameterDef this[int index]
        {
            get { return (FunctionParameterDef)ItemByIndex(index); }
        }

        /// <summary>
        /// добавление.
        /// </summary>
        /// <param name="dataobject"></param>
        public void Add(FunctionParameterDef dataobject)
        {
            AddObject(dataobject);
        }
    }
}

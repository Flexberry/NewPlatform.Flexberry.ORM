namespace ICSSoft.STORMNET.FunctionalLanguage
{
    /// <summary>
    /// Унаследованный от TypedObject класс для определения параметров функции.
    /// </summary>
    [NotStored]
    public class FunctionParameterDef : TypedObject
    {
        private bool fieldMultiValueSupport;
        private FunctionDef fieldFunctionDef;

        /// <summary>
        /// Определение функции (агрегатор).
        /// </summary>
        [Agregator]
        public FunctionDef FunctionDef
        {
            get { return fieldFunctionDef; }
            set { fieldFunctionDef = value; }
        }

        /// <summary>
        /// конструктор.
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="objStringedView"></param>
        /// <param name="objCaption"></param>
        public FunctionParameterDef(ObjectType objType, string objStringedView, string objCaption)
            : base(objType, objStringedView, objCaption)
        {
            fieldMultiValueSupport = false;
        }

        /// <summary>
        /// конструктор.
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="objStringedView"></param>
        /// <param name="objCaption"></param>
        /// <param name="multiValueSupport"></param>
        public FunctionParameterDef(ObjectType objType, string objStringedView, string objCaption, bool multiValueSupport)
            : base(objType, objStringedView, objCaption)
        {
            fieldMultiValueSupport = multiValueSupport;
        }

        /// <summary>
        /// конструктор.
        /// </summary>
        /// <param name="objType"></param>
        public FunctionParameterDef(ObjectType objType)
            : base(objType, null, null)
        {
            fieldMultiValueSupport = false;
        }

        /// <summary>
        /// конструктор.
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="multiValueSupport"></param>
        public FunctionParameterDef(ObjectType objType, bool multiValueSupport)
            : base(objType, null, null)
        {
            fieldMultiValueSupport = multiValueSupport;
        }

        /// <summary>
        /// Поддерживается ли много значений одного параметра.
        /// </summary>
        public bool MultiValueSupport
        {
            get { return fieldMultiValueSupport; }
            set { fieldMultiValueSupport = value; }
        }
    }
}

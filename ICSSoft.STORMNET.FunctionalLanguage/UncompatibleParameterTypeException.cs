namespace ICSSoft.STORMNET.FunctionalLanguage
{
    using System;

    /// <summary>
    /// Несовместимые типы параметров.
    /// </summary>
    public class UncompatibleParameterTypeException : Exception
    {
        /// <summary>
        /// Номер параметра
        /// </summary>
        public int ParameterNum;
        internal UncompatibleParameterTypeException(int parNum)
        {
            ParameterNum = parNum;
        }
    }
}
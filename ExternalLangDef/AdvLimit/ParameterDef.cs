using System;
using ICSSoft.STORMNET.FunctionalLanguage;

namespace ICSSoft.STORMNET.Windows.Forms
{
    /// <summary>
    /// Определение параметра
    /// </summary>
    [Serializable]
    public class ParameterDef : ICSSoft.STORMNET.FunctionalLanguage.VariableDef
    {
        private string fParamName;
        private string fAdv;
        private bool fMultiply;

        public string ParamName { get { return fParamName; } set { fParamName = value; StringedView = "@" + value; } }

        public string Adv { get { return fAdv; } set { fAdv = value; } }

        public bool Multiply { get { return fMultiply; } set { fMultiply = value; } }

        public ParameterDef(string ParamName, ICSSoft.STORMNET.FunctionalLanguage.ObjectType type, bool Multiply, string Advansed)
            : base(type, "@" + ParamName, "@" + ParamName)
        {
            fParamName = ParamName;
            fAdv = Advansed;
            fMultiply = Multiply;
        }

        public ParameterDef() { }

        public override object ToSimpleValue()
        {
            return new object[] { fParamName, fAdv, fMultiply, base.ToSimpleValue() };
        }

        public override void FromSimpleValue(object value, FunctionalLanguageDef ldef)
        {
            object[] obj = (object[])value;
            base.FromSimpleValue(obj[3], ldef);
            fParamName = (string)obj[0];
            fAdv = (string)obj[1];
            fMultiply = (bool)obj[2];
        }

        public override string ToString()
        {
            return "@" + ParamName;
        }
    }
}
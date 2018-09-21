using System;

namespace ICSSoft.STORMNET.UserDataTypes
{
    [ICSSoft.STORMNET.Windows.Forms.Binders.ControlProvider("ICSSoft.STORMNET.UserDataTypes.PartliedDateControlProvider, ICSSoft.STORMNET.Windows.Forms")]
    [Serializable]
    [ICSSoft.STORMNET.StoreInstancesInType(typeof(ICSSoft.STORMNET.Business.SQLDataService), typeof(string))]
    public class PartliedDate
    {
        internal string fValue;

        private PartliedDate(string val){fValue = val;}

        public PartliedDate()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public override string ToString()
        {
            return fValue;
        }

        static public PartliedDate Parse(string val)
        {
            PartliedDate p =new PartliedDate(val);
            return p;
        }
    }

    public class PartliedDateTransformer
    {
        //строка вида 03.15.1222
        internal string pSSS;

        static public explicit operator PartliedDate(PartliedDateTransformer value)
        {
            PartliedDate p = new PartliedDate();
            p.fValue = value.pSSS.Substring(6)+"."+value.pSSS.Substring(3,2)+"."+value.pSSS.Substring(0,2);
            return p;
        }

        static public explicit operator PartliedDateTransformer(PartliedDate value)
        {
            PartliedDateTransformer p = new PartliedDateTransformer();
            p.pSSS = value.fValue.Substring(8)+"."+value.fValue.Substring(5,2)+"."+value.fValue.Substring(0,4);
            return p;
        }

        public override string ToString()
        {
            return pSSS;
        }

        static public PartliedDateTransformer Parse(string val)
        {
            PartliedDateTransformer p =new PartliedDateTransformer();
            p.pSSS = val;
            return p;
        }
    }
}

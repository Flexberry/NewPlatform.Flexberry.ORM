using System;
using System.Collections.Generic;
using System.Text;
using ICSSoft.STORMNET.FunctionalLanguage;
using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

namespace ICSSoft.STORMNET.Business
{
    public class SQLLoadingCustomizationStruct
    {
        static public LoadingCustomizationStruct GetSimpleStruct(Type DataObjectType, string View, string propertyName, object limitValue)
        {
            return GetSimpleStruct(Information.GetView(View, DataObjectType), propertyName, limitValue);
        }

        static public LoadingCustomizationStruct GetSimpleStruct(View view, string propertyName, object limitValue)
        {
            var lcs = new LoadingCustomizationStruct(null);
            FunctionalLanguage.SQLWhere.SQLWhereLanguageDef ldef =
                FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
            Type proptype = (propertyName == SQLWhereLanguageDef.StormMainObjectKey)?typeof(Guid):Information.GetPropertyType(view.DefineClassType, propertyName);
            var var = new VariableDef(ldef.GetObjectTypeForNetType(proptype), propertyName);
            lcs.LoadingTypes = new[] { view.DefineClassType };
            lcs.View = view;
            lcs.LimitFunction = ldef.GetFunction(ldef.funcEQ, var, limitValue);
            return lcs;
        }


        static public LoadingCustomizationStruct GetSimpleStruct(Type DataObjectType, string View, object ObjectKey)
        {
            return GetSimpleStruct(DataObjectType, View, SQLWhereLanguageDef.StormMainObjectKey, ObjectKey);
        }

        static public LoadingCustomizationStruct GetSimpleStruct(View view, ICSSoft.STORMNET.DataObject[] objects)
        {
            var lcs = new LoadingCustomizationStruct(null);
            List<Type> types = new List<Type>();
            List<object> keys = new List<object>();
            foreach (ICSSoft.STORMNET.DataObject obj in objects)
            {
                object Key = obj.__PrimaryKey;
                if (!keys.Contains(Key))
                {
                    keys.Add(Key);
                    Type t = obj.GetType();
                    if (!types.Contains(t))
                        types.Add(t);
                }
            }
            lcs.LoadingTypes = types.ToArray();
            lcs.View = view;
            lcs.LimitFunction = SQLWhereLanguageDef.Keys2Function(keys.ToArray());
            return lcs;
        }

        static public LoadingCustomizationStruct GetSimpleStruct(View View, object ObjectKey)
        {
            return GetSimpleStruct(View, SQLWhereLanguageDef.StormMainObjectKey, ObjectKey);
        }

    }
}

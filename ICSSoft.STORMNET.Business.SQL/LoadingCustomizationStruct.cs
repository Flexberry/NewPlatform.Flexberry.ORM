using System;
using System.Collections.Generic;
using System.Text;
using ICSSoft.STORMNET.FunctionalLanguage;

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
            Type proptype = (propertyName == "STORMMainObjectKey")?typeof(Guid):Information.GetPropertyType(view.DefineClassType, propertyName);
            var var = new VariableDef(ldef.GetObjectTypeForNetType(proptype), propertyName);
            lcs.LoadingTypes = new[] { view.DefineClassType };
            lcs.View = view;
            lcs.LimitFunction = ldef.GetFunction(ldef.funcEQ, var, limitValue);
            return lcs;
        }


        static public LoadingCustomizationStruct GetSimpleStruct(Type DataObjectType, string View, object ObjectKey)
        {
            return GetSimpleStruct(DataObjectType, View, "STORMMainObjectKey", ObjectKey);
        }

        static public LoadingCustomizationStruct GetSimpleStruct(View View, object ObjectKey)
        {
            return GetSimpleStruct(View, "STORMMainObjectKey", ObjectKey);
        }

    }
}

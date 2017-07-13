using System;
using System.Collections;
using System.ComponentModel;

namespace ICSSoft.STORMNET.FunctionalLanguage
{
    /// <summary>
    /// Функция для контролов (используется при: сериализации-десериализации LoadingCustomizationStruct, ExtendedTextBox, ObjectListVeiw и пр.)
    /// </summary>
    [TypeConverter(typeof(FunctionForControlsConverter))]
    [Serializable]
    public class FunctionForControls
    {

        /// <summary>
        /// Имя
        /// </summary>
        public string Name;
        /// <summary>
        /// Функция
        /// </summary>
        public ICSSoft.STORMNET.FunctionalLanguage.Function Function
        {
            get { return function; }
        }
        private ICSSoft.STORMNET.FunctionalLanguage.Function function;
        /// <summary>
        /// Представление
        /// </summary>
        public View View = null;
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="view"></param>
        /// <param name="func"></param>
        public FunctionForControls(ICSSoft.STORMNET.View view, ICSSoft.STORMNET.FunctionalLanguage.Function func)
        {
            this.View = view;
            this.function = func;
        }
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="View"></param>
        /// <param name="type"></param>
        /// <param name="func"></param>
        public FunctionForControls(string View, Type type, ICSSoft.STORMNET.FunctionalLanguage.Function func)
        {
            this.View = Information.GetView(View, type);
            function = func;
        }
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="def"></param>
        /// <param name="View"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        public FunctionForControls(ICSSoft.STORMNET.FunctionalLanguage.FunctionDef def, string View, Type type, params object[] parameters)
        {
            this.View = Information.GetView(View, type);
            function = new ICSSoft.STORMNET.FunctionalLanguage.Function(def, parameters);
        }
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="view"></param>
        /// <param name="def"></param>
        /// <param name="parameters"></param>
        public FunctionForControls(ICSSoft.STORMNET.View view, ICSSoft.STORMNET.FunctionalLanguage.FunctionDef def, params object[] parameters)
        {
            function = new ICSSoft.STORMNET.FunctionalLanguage.Function(def, parameters);
            this.View = view;
        }

        private System.Xml.XmlElement ToXMLElement(System.Xml.XmlDocument doc, ICSSoft.STORMNET.FunctionalLanguage.Function funct)
        {
            System.Xml.XmlElement el = doc.CreateElement("Function");
            if (funct != null)
            {
                el.SetAttribute("Name", funct.FunctionDef.StringedView);
                for (int i = 0; i < funct.Parameters.Count; i++)
                {
                    if (funct.Parameters[i] is ICSSoft.STORMNET.FunctionalLanguage.Function)
                        el.AppendChild(ToXMLElement(doc, funct.Parameters[i] as ICSSoft.STORMNET.FunctionalLanguage.Function));
                    else if (funct.Parameters[i] is ICSSoft.STORMNET.FunctionalLanguage.VariableDef)
                    {
                        System.Xml.XmlElement varel = doc.CreateElement("Variable");
                        varel.SetAttribute("Value", (funct.Parameters[i] as ICSSoft.STORMNET.FunctionalLanguage.VariableDef).StringedView);
                        el.AppendChild(varel);
                    }
                    else
                    {
                        System.Xml.XmlElement valel = doc.CreateElement("Value");
                        int parindex = (i >= funct.FunctionDef.Parameters.Count) ? funct.FunctionDef.Parameters.Count - 1 : i;
                        valel.SetAttribute("ConstType", funct.FunctionDef.Parameters[parindex].Type.StringedView);
                        valel.SetAttribute("Value", funct.Parameters[i].ToString());
                        el.AppendChild(valel);
                    }
                }
            }
            return el;
        }

        /// <summary>
        /// В строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlElement elem = ToXMLElement(doc, this.function);
            elem.SetAttribute("___Name", Name);
            doc.AppendChild(elem);
            return doc.OuterXml;
        }

        private static ICSSoft.STORMNET.FunctionalLanguage.Function FromXMLElements(System.Xml.XmlElement elem, ICSSoft.STORMNET.FunctionalLanguage.FunctionalLanguageDef lang, ICSSoft.STORMNET.FunctionalLanguage.VariableDef[] vars)
        {
            string funcname = elem.GetAttribute("Name");
            object[] parameters = new object[elem.ChildNodes.Count];
            for (int i = 0; i < elem.ChildNodes.Count; i++)
            {
                System.Xml.XmlElement subel = (System.Xml.XmlElement)elem.ChildNodes[i];
                switch (subel.Name)
                {
                    case "Function":
                        parameters[i] = FromXMLElements(subel, lang, vars);
                        break;
                    case "Variable":
                        string varName = subel.GetAttribute("Value");
                        foreach (ICSSoft.STORMNET.FunctionalLanguage.VariableDef vd in vars)
                            if (vd.StringedView == varName)
                            {
                                parameters[i] = vd;
                                break;
                            }
                        break;
                    case "Value":
                    {
                        string typename = subel.GetAttribute("ConstType");
                        parameters[i] = null;
                        foreach (ICSSoft.STORMNET.FunctionalLanguage.ObjectType ot in lang.Types)
                        {
                            if (ot.StringedView == typename)
                            {
                                try
                                {
                                    parameters[i] = Convert.ChangeType(subel.GetAttribute("Value"), ot.NetCompatibilityType);
                                    break;
                                }
                                catch { }
                            }
                        }
                        if (parameters[i] == null)
                            parameters[i] = subel.GetAttribute("Value");
                        break;
                    }
                };
            }
            if (funcname == null || funcname == "")
                return null;
            else
                return lang.GetFunction(funcname, parameters);

        }
        /// <summary>
        /// Разбор
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="viewname"></param>
        /// <returns></returns>
        static public FunctionForControls Parse(string value, Type type, string viewname)
        {
            ICSSoft.STORMNET.View v = ICSSoft.STORMNET.Information.GetView(viewname, type);
            return Parse(value, v);
        }
        /// <summary>
        /// Разбор
        /// </summary>
        /// <param name="value"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        static public FunctionForControls Parse(string value, ICSSoft.STORMNET.View v)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(value);
            ICSSoft.STORMNET.FunctionalLanguage.FunctionalLanguageDef lang = ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef.LanguageDef;
            ICSSoft.STORMNET.FunctionalLanguage.VariableDef[] vars; ArrayList arvars = new ArrayList();
            arvars.Add(new ICSSoft.STORMNET.FunctionalLanguage.VariableDef((lang as ICSSoft.STORMNET.FunctionalLanguage.SQLWhere.SQLWhereLanguageDef).GuidType, "STORMMainObjectKey"));
            foreach (ICSSoft.STORMNET.PropertyInView piv in v.Properties)
            {
                System.Type propType = ICSSoft.STORMNET.Information.GetPropertyType(v.DefineClassType, piv.Name);
                try
                {
                    ICSSoft.STORMNET.FunctionalLanguage.ObjectType t = lang.GetObjectTypeForNetType(propType);
                    if (t != null)
                        arvars.Add(new ICSSoft.STORMNET.FunctionalLanguage.VariableDef(t, piv.Name, piv.Caption));
                }
                catch
                { }
            }
            vars = (ICSSoft.STORMNET.FunctionalLanguage.VariableDef[])arvars.ToArray(typeof(ICSSoft.STORMNET.FunctionalLanguage.VariableDef));
            ICSSoft.STORMNET.FunctionalLanguage.Function fnc = FromXMLElements((System.Xml.XmlElement)doc.FirstChild, lang, vars);
            FunctionForControls res = null;
            if (fnc == null)
                res = new FunctionForControls(v, fnc);
            else
                res = new FunctionForControls(v, fnc.FunctionDef, fnc.Parameters.ToArray());
            try
            {
                res.Name = ((System.Xml.XmlElement)doc.FirstChild).GetAttribute("___Name");
            }
            catch { }
            return res;
        }
    }
}
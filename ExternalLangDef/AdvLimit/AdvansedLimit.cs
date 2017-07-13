namespace ICSSoft.STORMNET.Windows.Forms
{
    using System;
    using System.Collections;
    using FunctionalLanguage;

    /// <summary>
    /// Объектная модель ограничения, редактируемого на форме задания ограничений. Включает в себя как ограничивающую функцию, так и определения параметров. Тут же живёт сериализованное описание кастом-формы для ввода параметров.
    /// </summary>
    [Serializable]
    public partial class AdvansedLimit
    {
        /// <summary>
        /// Имя ограничения.
        /// </summary>
        public string Name;

        /// <summary>
        /// Горячая клавиша.
        /// </summary>
        public int HotKeyData;

        /// <summary>
        /// Определения параметров.
        /// </summary>
        public ParameterDef[] Parameters;

        /// <summary>
        /// Ограничивающая функция.
        /// </summary>
        public Function Function;

        /// <summary>
        /// Сериализованная настройка кастом-формы ввода параметров.
        /// </summary>
        public string FormCustomizeString;

        /// <summary>
        /// Значения параметров.
        /// </summary>
        public SortedList paramValues;

        /// <summary>
        /// Лукапы (используются при подъёме универсальной кастом-формы ввода параметров).
        /// </summary>
        public Hashtable LookUps;

        /// <summary>
        /// конструктор с параметрами
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="pars"></param>
        /// <param name="Func"></param>
        /// <param name="custString"></param>
        /// <param name="pV"></param>
        /// <param name="LookUps"></param>
        public AdvansedLimit(string Name, ParameterDef[] pars, FunctionalLanguage.Function Func, string custString, SortedList pV, Hashtable LookUps)
        {
            this.Name = Name;
            Parameters = pars;
            Function = Func;
            FormCustomizeString = custString;
            paramValues = pV;
            this.LookUps = LookUps;
        }

        /// <summary>
        /// конструктор
        /// </summary>
        public AdvansedLimit() { }

        /// <summary>
        /// псевдосериализация FunctionalLanguageDef в object[]
        /// </summary>
        /// <param name="fld"></param>
        /// <returns></returns>
        public object ToSimpleValue(FunctionalLanguageDef fld)
        {
            object[] Pars = new object[Parameters.Length];
            for (int i = 0; i < Pars.Length; i++)
                Pars[i] = Parameters[i].ToSimpleValue();
            return new object[] { Name, Pars, fld.FunctionToSimpleStruct(Function), FormCustomizeString, paramValues };
        }

        /// <summary>
        /// псевдодесериализация object[] в FunctionalLanguageDef
        /// В конце выполняется синхронизация параметров функции с параметрами текущего объекта (параметрам функции присвоятся параметры текущего объекта при совпадении имён)
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="fld"></param>
        public void FromSimpleValue(object Value, FunctionalLanguageDef fld)
        {
            object[] obj = (object[])Value;
            Name = (string)obj[0];
            object[] Pars = (object[])obj[1];
            Parameters = new ParameterDef[Pars.Length];
            for (int i = 0; i < Pars.Length; i++)
            {
                Parameters[i] = new ParameterDef();
                Parameters[i].FromSimpleValue(Pars[i], fld);
            }
            Function = fld.FunctionFromSimpleStruct(obj[2]);
            FormCustomizeString = (string)obj[3];
            paramValues = (SortedList)obj[4];
            SyncParams(Function);
        }

        /// <summary>
        /// Синхронизация параметров функции с параметрами текущего объекта (параметрам функции присвоятся параметры текущего объекта при совпадении имён)
        /// </summary>
        /// <param name="func"></param>
        private void SyncParams(FunctionalLanguage.Function func)
        {
            for (int i = 0; i < func.Parameters.Count; i++)
            {
                object o = func.Parameters[i];
                if (o is FunctionalLanguage.Function)
                    SyncParams(o as FunctionalLanguage.Function);
                else if (o is ParameterDef)
                {
                    ParameterDef op = o as ParameterDef;
                    foreach (ParameterDef pd in Parameters)
                    {
                        if (pd.ParamName == op.ParamName)
                        {
                            func.Parameters[i] = pd;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Последний отредактированный объект данных (TODO: первый претендент на рефакторинг, т.к. по сути является глобальной переменной, которую непонятно кто и где использует)
        /// </summary>
        public DataObject lastEdobj = null;

        /// <summary>
        /// !!! Сделано public для обратной совместимости, не трогать.
        /// Параметр, который передаётся в универсальную форму редактирования параметров при её подъёме, который попадает в заголовок зависимой формы.
        /// </summary>
        public string InnerParameter = string.Empty;

        /// <summary>
        /// !!! Сделано public для обратной совместимости, использовать аккуратно.
        /// Берутся значения из глобальной переменной edobj и впариваются в функцию вместо параметров
        /// TODO: переписать эту функцию так, чтобы глобальная переменная не применялась и сделать её публичной
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Function ConvertFunction(Function func)
        {
            if (func == null) return null;

            FunctionalLanguage.Function res = new ICSSoft.STORMNET.FunctionalLanguage.Function(func.FunctionDef);
            ArrayList pars = new ArrayList(); pars.AddRange(func.Parameters);
            for (int i = 0; i < pars.Count; i++)
            {
                if (pars[i] is FunctionalLanguage.Function)
                {
                    FunctionalLanguage.Function cur_func = pars[i] as FunctionalLanguage.Function;
                    pars[i] = ConvertFunction(cur_func);
                }
                else if (pars[i] is ParameterDef)
                {
                    object val = ICSSoft.STORMNET.Information.GetPropValueByName(edobj, (pars[i] as ParameterDef).ParamName);
                    if (val is DetailArray)
                    {
                        DetailArray dar = val as DetailArray;
                        DataObject[] objs = dar.GetAllObjects();
                        foreach (DataObject obj in objs)
                        {
                            if (obj.GetStatus(false) == ObjectStatus.Deleted)
                                dar.Remove(obj);
                            else
                            {
                                object ops = ICSSoft.STORMNET.Information.GetPropValueByName(obj, (pars[i] as ParameterDef).ParamName);
                                if (ops == null)
                                    dar.Remove(obj);
                            }
                        }

                        object[] pp = new object[dar.Count];
                        for (int j = 0; j < pp.Length; j++)
                            pp[j] = ICSSoft.STORMNET.Information.GetPropValueByName(dar.ItemByIndex(j), (pars[i] as ParameterDef).ParamName);
                        if (func.FunctionDef.Parameters[func.FunctionDef.Parameters.Count - 1].MultiValueSupport)
                        {
                            pars.RemoveAt(i);
                            pars.InsertRange(i, pp);
                        }
                        else
                            if (pp.Length > 0)
                                pars[i] = pp[0];
                            else
                                pars[i] = null;
                        i--;
                    }
                    else
                        pars[i] = ICSSoft.STORMNET.Information.GetPropValueByName(edobj, (pars[i] as ParameterDef).ParamName);
                }
                else if (func.FunctionDef.StringedView == "SQL" && ICSSoft.STORMNET.Business.DataServiceProvider.DataService is ICSSoft.STORMNET.Business.SQLDataService)
                {
                    ICSSoft.STORMNET.Business.SQLDataService ds = (ICSSoft.STORMNET.Business.SQLDataService)ICSSoft.STORMNET.Business.DataServiceProvider.DataService;

                    string sSQl = pars[i].ToString();

                    int j;

                    while ((j = sSQl.IndexOf("@")) != -1)
                    {
                        int k = j + 1;
                        while (k < sSQl.Length && (char.IsLetterOrDigit(sSQl[k]) || sSQl[k] == '_'))
                        {
                            k++;
                        }
                        string sParamName = sSQl.Substring(j, k - j);
                        sSQl = sSQl.Remove(j, k - j);

                        string sParamValue = "";

                        foreach (ParameterDef p in Parameters)
                        {
                            if ("@" + p.ParamName == sParamName)
                            {
                                sParamValue = ds.ConvertSimpleValueToQueryValueString(ICSSoft.STORMNET.Information.GetPropValueByName(edobj, p.ParamName));
                                break;
                            }
                        }
                        sSQl = sSQl.Insert(j, sParamValue);
                    }

                    pars[i] = sSQl;
                }
            }
            res.Parameters.AddRange(pars);
            return res;
        }

        /// <summary>
        /// Добавляет переданное ограничение к текущему через OR
        /// </summary>
        /// <param name="addedFunction">Добавляемая функция</param>
        /// <returns>Новая функция</returns>
        public Function AddFunctionByOR(Function addedFunction)
        {
            var langdef = ExternalLangDef.LanguageDef;
            this.Function = langdef.GetFunction(langdef.funcOR, this.Function, addedFunction);
            return this.Function;
        }


        /// <summary>
        /// !!! Сделано public для обратной совместимости, не трогать.
        /// Глобальная переменная, через которую передаются значения параметров, указанные пользователем в ограничивающую функцию. 
        /// </summary>
        public DataObject edobj;

        /// <summary>
        /// !!! Сделано public для обратной совместимости, не трогать.
        /// Глобальный флаг, который будет выставлен, когда редактирование значений параметров закончено
        /// </summary>
        public bool EndEdit = false;

        /// <summary>
        /// !!! Сделано public для обратной совместимости, не трогать.
        /// Обработчик события сохранения кастом-формы задания параметров ограничения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ue_SaveEvent(object sender, ICSSoft.STORMNET.UI.SaveEventArgs e)
        {
            edobj = e.dataobject;
        }

        /// <summary>
        /// !!! Сделано public для обратной совместимости, не трогать.
        /// Обработчик события остановки кастом-формы задания параметров ограничения.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ue_EditorStoppedEvent(object sender, ICSSoft.STORMNET.UI.EditorStoppedEventArgs e)
        {
            EndEdit = true;
        }

        /// <summary>
        /// !!! Сделано public для обратной совместимости, не трогать.
        /// Событие отмены редактирования параметров ограничений на кастом-форме.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ue_CancelEvent(object sender, ICSSoft.STORMNET.UI.CancelEventArgs e)
        {
            edobj = null;
            EndEdit = true;
        }
    }
}
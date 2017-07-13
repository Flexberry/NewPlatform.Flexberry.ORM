using System;
using System.Collections.Specialized;

namespace ICSSoft.STORMNET.Business
{
	/// <summary>
	/// Summary description for ODBCMySQLDataService.
	/// </summary>
	public class ODBCMySQLDataService:ODBCDataService
	{
		private System.Collections.Specialized.StringDictionary identDict = new System.Collections.Specialized.StringDictionary();

        /// <summary>
        /// Создание копии экземпляра сервиса данных.
        /// </summary>
        /// <returns>Копии экземпляра сервиса данных.</returns>
        public override object Clone()
        {
            var instance = (ODBCMySQLDataService)base.Clone();
            instance.identDict = identDict;
            return instance;
        }

		public override string PutIdentifierIntoBrackets(string identifier)
		{
			if (identifier.IndexOf(".")>=0 || identifier.Length>32)
			{
				if (identDict.ContainsKey(identifier))
					return identDict[identifier];
				else
				{
					string ni = "gi"+Guid.NewGuid().ToString("N").Substring(4)+"";
					identDict.Add(identifier,ni);
					return ni;
				}
			}
			else return TranslitRu(identifier);
		}
		

		public override string GetConvertToTypeExpression(Type valType, string value)
		{
			return value;
		}


		public string TranslitRu(string stringRu)
		{
			StringDictionary sd = new StringDictionary();
			sd.Add("А", "A");
			sd.Add("Б", "B");
			sd.Add("В", "V");
			sd.Add("Г", "G");
			sd.Add("Д", "D");
			sd.Add("Е", "E");
			sd.Add("Ё", "YO");
			sd.Add("Ж", "ZH");
			sd.Add("З", "Z");
			sd.Add("И", "I");
			sd.Add("Й", "J");
			sd.Add("К", "K");
			sd.Add("Л", "L");
			sd.Add("М", "M");
			sd.Add("Н", "N");
			sd.Add("О", "O");
			sd.Add("П", "P");
			sd.Add("Р", "R");
			sd.Add("С", "S");
			sd.Add("Т", "T");
			sd.Add("У", "U");
			sd.Add("Ф", "F");
			sd.Add("Х", "H");
			sd.Add("Ц", "C");
			sd.Add("Ч", "CH");
			sd.Add("Ш", "SH");
			sd.Add("Щ", "SCH");
			sd.Add("Ь", "YS");
			sd.Add("Ы", "W");
			sd.Add("Ъ", "YT");
			sd.Add("Э", "EE");
			sd.Add("Ю", "YU");
			sd.Add("Я", "YA");

			string result = "";
			for (int i=0; i<stringRu.Length; i++)
			{
				string ruSimbol = stringRu[i].ToString();
				string tlSimbol = "";
				if (sd.ContainsKey(ruSimbol))
				{
					tlSimbol=sd[ruSimbol];
				}
				else
				{
					tlSimbol=ruSimbol;
				}
				result=string.Format("{0}{1}", result, tlSimbol);
			}

			return result;
		}
		public override string ConvertSimpleValueToQueryValueString(object value)
		{
			if (value is DateTime)
			{
				return String.Format("'{0}'", ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"));
			}
			else if (value is string)
			{
				string res = base.ConvertSimpleValueToQueryValueString (value);
				return res.Replace(@"\",@"\\");
			}
			else
				return base.ConvertSimpleValueToQueryValueString (value);
		}



	}
}

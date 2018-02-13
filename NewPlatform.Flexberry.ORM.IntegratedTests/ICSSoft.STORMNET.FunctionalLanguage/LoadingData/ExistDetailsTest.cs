//namespace ICSSoft.STORMNET.Tests.TestClasses.FunctionalLanguage.LoadingData
//{
//    using System;
//    using System.Linq;

//    using ICSSoft.STORMNET.Business;
//    using ICSSoft.STORMNET.FunctionalLanguage;
//    using ICSSoft.STORMNET.KeyGen;
//    using ICSSoft.STORMNET.Windows.Forms;

//    using IIS.AMS02.Объекты;

//    using Xunit;

//    
//    public class ExistDetailsTest
//    {
//        [Fact]
//        
//        public void TestMethod1()
//        {
//            // тестирование запроса с детейлами с условием сравнения собственных свойств детейлов
//            IDataService dataService = DataServiceLoader.InitializeDataSetvice();
//            View view = Information.GetView("ЗадержанныйL", typeof(Задержанный));
//            View view2 = Information.GetView("ИзъятоУЗадержанногоD", typeof(ИзъятоУЗадержанного));
//            View view3 = Information.GetView("ПричинаЗадержанияL", typeof(ПричинаЗадержания));
//            view.AddDetailInView("Изъято", view2, true);
//            view.AddDetailInView("Причины", view3, true);
//            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Задержанный), view);
//            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
//            var detail1 = new DetailVariableDef(langDef.GetObjectType("Details"), "Изъято", view2, "Задержанный");
//            var detail2 = new DetailVariableDef(langDef.GetObjectType("Details"), "Причины", view3, "Задержанный");
//            var func = langDef.GetFunction(langDef.funcExistDetails,
//                                           detail1, detail2,
//                                           langDef.GetFunction(langDef.funcEQ,
//                                                               new VariableDef(langDef.StringType, "Количество"),
//                                                               new VariableDef(langDef.StringType, "Номер")));

//            lcs.LimitFunction = func;
//            var dos = dataService.LoadObjects(lcs);
//            lcs.LimitFunction = null;
//            var dos2 = dataService.LoadObjects(lcs);

//            Assert.True(dos2.Length == DataServiceLoader.CountЗадержанный &&
//                          dos.Length == 2 &&
//                          dos[0].__PrimaryKey.Equals(new KeyGuid("f36e9cf2-aed2-49e1-9c23-a0a3fbe90509")) &&
//                          dos[1].__PrimaryKey.Equals(new KeyGuid("acf1fe8d-b8e5-4012-a3a5-a99f56a264bd")));
//        }

//        [Fact]
//        
//        public void TestMethod2()
//        {
//            // тестирование запроса с детейлами с условием сравнения свойств мастеров детейлов
//            IDataService dataService = DataServiceLoader.InitializeDataSetvice();
//            View view = Information.GetView("ЗадержанныйL", typeof(Задержанный));
//            View view2 = Information.GetView("ИзъятоУЗадержанногоD", typeof(ИзъятоУЗадержанного));
//            View view3 = Information.GetView("ПричинаЗадержанияL", typeof(ПричинаЗадержания));
//            view.AddDetailInView("Изъято", view2, true);
//            view.AddDetailInView("Причины", view3, true);
//            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Задержанный), view);
//            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
//            var detail1 = new DetailVariableDef(langDef.GetObjectType("Details"), "Изъято", view2, "Задержанный");
//            var detail2 = new DetailVariableDef(langDef.GetObjectType("Details"), "Причины", view3, "Задержанный");
//            var func = langDef.GetFunction(langDef.funcExistDetails,
//                                           detail1, detail2,
//                                           langDef.GetFunction(langDef.funcGEQ,
//                                                               new VariableDef(langDef.StringType, "Предмет.Наименование"),
//                                                               new VariableDef(langDef.StringType, "Вид.Наименование")));

//            lcs.LimitFunction = func;
//            var dos = dataService.LoadObjects(lcs);
//            lcs.LimitFunction = null;
//            var dos2 = dataService.LoadObjects(lcs);

//            Assert.True(dos2.Length == DataServiceLoader.CountЗадержанный &&
//                        dos.Length == 2 &&
//                        dos[0].__PrimaryKey.Equals(new KeyGuid("acd98e27-f498-40c3-ba20-4b3957678a6a")) &&
//                        dos[1].__PrimaryKey.Equals(new KeyGuid("f36e9cf2-aed2-49e1-9c23-a0a3fbe90509")));
//        }

//        [Fact]
//        
//        public void TestMethod3()
//        {
//            // тестирование запроса с детейлами с дополнительным условием и внутри выражения
//            IDataService dataService = DataServiceLoader.InitializeDataSetvice();
//            View view = Information.GetView("ЗадержанныйL", typeof(Задержанный));
//            View view2 = Information.GetView("ИзъятоУЗадержанногоD", typeof(ИзъятоУЗадержанного));
//            View view3 = Information.GetView("ПричинаЗадержанияL", typeof(ПричинаЗадержания));
//            view.AddDetailInView("Изъято", view2, true);
//            view.AddDetailInView("Причины", view3, true);
//            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Задержанный), view);
//            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
//            var detail1 = new DetailVariableDef(langDef.GetObjectType("Details"), "Изъято", view2, "Задержанный");
//            var detail2 = new DetailVariableDef(langDef.GetObjectType("Details"), "Причины", view3, "Задержанный");
//            var func = langDef.GetFunction(langDef.funcExistDetails,
//                                           detail1, detail2,
//                                           langDef.GetFunction(langDef.funcEQ,
//                                                               new VariableDef(langDef.StringType, "Количество"),
//                                                               new VariableDef(langDef.StringType, "Номер")));
//            var func2 = langDef.GetFunction(langDef.funcEQ, func, true);
//            var func3 = langDef.GetFunction(langDef.funcGEQ, new VariableDef(langDef.NumericType, "Номер"), 2);
//            lcs.LimitFunction = langDef.GetFunction(langDef.funcAND, func2, func3);
//            var dos = dataService.LoadObjects(lcs);
//            lcs.LimitFunction = null;
//            var dos2 = dataService.LoadObjects(lcs);

//            Assert.True(dos2.Length == DataServiceLoader.CountЗадержанный &&
//                          dos.Length == 2 &&
//                          dos[0].__PrimaryKey.Equals(new KeyGuid("f36e9cf2-aed2-49e1-9c23-a0a3fbe90509")) &&
//                          dos[1].__PrimaryKey.Equals(new KeyGuid("acf1fe8d-b8e5-4012-a3a5-a99f56a264bd")));
//        }

//        [Fact]
//        
//        public void TestMethod4()
//        {
//            try
//            {
//                // тестирование запроса с детейлами с несколькими TypeLoading
//                IDataService dataService = DataServiceLoader.InitializeDataSetvice();
//                View view = Information.GetView("УДЛицоУСL", typeof(ЛицоУС));
//                View view2 = Information.GetView("ЛицоВЭпизодеЭпизодE", typeof(ЛицоВЭпизоде));
//                View view3 = Information.GetView("ЦельПриездаЛицаE", typeof(ЦельПриездаЛица));
//                view.AddDetailInView("ЛицоВЭпизоде", view2, true);
//                view.AddDetailInView("ЦелиПриездаЛица", view3, true);
//                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(ЛицоУС), view);
//                lcs.LoadingTypes = new[] { typeof(Преступник), typeof(Потерпевший) };
//                ExternalLangDef langDef = ExternalLangDef.LanguageDef;
//                langDef.DataService = dataService;
//                var detail1 = new DetailVariableDef(langDef.GetObjectType("Details"), "ЛицоВЭпизоде", view2, "ЛицоВУгДеле");
//                var detail2 = new DetailVariableDef(langDef.GetObjectType("Details"), "ЦелиПриездаЛица", view3, "ЛицоВУгДеле");            
//                var func = langDef.GetFunction(langDef.funcExistDetails,
//                                               detail1, detail2,
//                                               langDef.GetFunction(langDef.funcGEQ,
//                                                                   new VariableDef(langDef.StringType, "СтатьиПреступникаСтрокой"),
//                                                                   new VariableDef(langDef.StringType, "ЦельВъезда.СтатКод")));
//                lcs.LimitFunction = func;
//                SQLDataService.ChangeCustomizationString += ChangeCustomizationString;
//                var dos = dataService.LoadObjects(lcs);
//                lcs.LimitFunction = null;
//                var dos2 = dataService.LoadObjects(lcs);
//                Assert.True(dos2.Length == 5774 && dos.Length == 23);
//            }
//            finally 
//            {
//                SQLDataService.ChangeCustomizationString -= ChangeCustomizationString;
//                DataServiceProvider.DataService = new MSSQLDataService { CustomizationString = DataServiceLoader.BaseCustomizationString };
//            }
//        }

//        /// <summary>
//        /// Вспомогательный метод для подмены строки соединения.
//        /// </summary>
//        /// <param name="types"> Определение типов, для которого нужно переопределить строку соединения. </param>
//        /// <returns> Используемая в данных тестах строка соединения. </returns>
//        private string ChangeCustomizationString(Type[] types)
//        {
//            return types.Any(x => x.AssemblyQualifiedName.Contains("AMS02"))
//                ? DataServiceLoader.NewCustomizationString
//                : DataServiceLoader.BaseCustomizationString;
//        }

//        [Fact]
//        public void TestMethod5()
//        {
//            // тестирование запроса с детейлами с присутсвием вложенных детейлов
//            Assert.Inconclusive("Verify the correctness of this test method.");

//            //TODO: тест не будет работать, пока не будет реализована поддержка сравнения детейлов выше первого уровня

//            IDataService dataService = DataServiceLoader.InitializeDataSetvice();
//            View view = Information.GetView("Ф2СвязанныеКарточки", typeof(Преступник));
//            View viewDetail = Information.GetView("РезультатСудебнРазбиратDСДетейлами", typeof(РезультатСудебнРазбират));
//            View view1 = Information.GetView("НаказаниеПоРешениюСудаE", typeof(НаказаниеПоРешениюСуда));
//            View view2 = Information.GetView("КвалификацияЭпизодаE", typeof(КвалификацияЭпизода));
//            viewDetail.AddDetailInView("НаказанияПоРешениюСуда", view1, true);
//            viewDetail.AddDetailInView("КвалификацииЭпизодов", view2, true);
//            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Преступник), view);
//            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
//            view.AddDetailInView("РезультатыСудебнРазбират", viewDetail, true);
            
//            var detail1 = new DetailVariableDef(langDef.GetObjectType("Details"), "НаказанияПоРешениюСуда", view1, "РезультатСудебнРазбират");
//            var detail2 = new DetailVariableDef(langDef.GetObjectType("Details"), "КвалификацииЭпизодов", view2, "РезультатСудебнРазбират");
//            var func = langDef.GetFunction(langDef.funcExistDetails, detail1, detail2,
//                                           langDef.GetFunction(langDef.funcEQ,
//                                           new VariableDef(langDef.DateTimeType, "РезультатыСудебнРазбират.НаказанияПоРешениюСуда.ДатаСоздания"),
//                                           new VariableDef(langDef.DateTimeType, "РезультатыСудебнРазбират.КвалификацииЭпизодов.ДатаСоздания")));
//            lcs.LimitFunction = func;
//            var dos = dataService.LoadObjects(lcs);
//            lcs.LimitFunction = null;
//            var dos2 = dataService.LoadObjects(lcs);

//        }
//    }
//}

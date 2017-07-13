//namespace ICSSoft.STORMNET.Tests.TestClasses.FunctionalLanguage.LoadingData
//{
//    using ICSSoft.STORMNET.Business;
//    using ICSSoft.STORMNET.FunctionalLanguage;
//    using ICSSoft.STORMNET.KeyGen;
//    using ICSSoft.STORMNET.Windows.Forms;

//    using IIS.AMS02.Объекты;

//    using Xunit;

//    
//    public class ExistTest
//    {
//        [Fact]
//        
//        public void TestMethod1()
//        {
//            // проверка наличия детейлов по условию сравнения двух собсвенных свойств детейла
//            IDataService dataService = DataServiceLoader.InitializeDataSetvice();
//            View view = Information.GetView("ЗадержанныйL", typeof(Задержанный));
//            View view2 = Information.GetView("ПричинаЗадержанияL", typeof(ПричинаЗадержания));
//            view.AddDetailInView("Причины", view2, true);
//            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Задержанный), view);
//            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
//            var detail = new DetailVariableDef(langDef.GetObjectType("Details"), "Причины", view2, "Задержанный");
//            var func = langDef.GetFunction(langDef.funcExist,
//                                           detail,
//                                           langDef.GetFunction(langDef.funcLEQ,
//                                                               new VariableDef(langDef.DateTimeType, "ГодПротокола"),
//                                                               new VariableDef(langDef.DateTimeType, "ГодРегистрКУСП")));

//            lcs.LimitFunction = func;
//            var dos = dataService.LoadObjects(lcs);
//            lcs.LimitFunction = null;
//            var dos2 = dataService.LoadObjects(lcs);
            
//            Assert.True(dos2.Length == DataServiceLoader.CountЗадержанный &&
//                          dos.Length == 1 &&
//                          dos[0].__PrimaryKey.Equals(new KeyGuid("f36e9cf2-aed2-49e1-9c23-a0a3fbe90509")));
//        }

//        [Fact]
//        
//        public void TestMethod2()
//        {
//            // проверка наличия детейлов по условию сравнения двую мастеровых свойств детейла
//            IDataService dataService = DataServiceLoader.InitializeDataSetvice();
//            View view = Information.GetView("ЗадержанныйL", typeof(Задержанный));
//            View view2 = Information.GetView("ПричинаЗадержанияL", typeof(ПричинаЗадержания));
//            view.AddDetailInView("Причины", view2, true);
//            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Задержанный), view);
//            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
//            var detail = new DetailVariableDef(langDef.GetObjectType("Details"), "Причины", view2, "Задержанный");
//            var func = langDef.GetFunction(langDef.funcExist,
//                                           detail,
//                                           langDef.GetFunction(langDef.funcGEQ,
//                                                               new VariableDef(langDef.StringType, "Вид.Наименование"),
//                                                               new VariableDef(langDef.StringType, "СтатьяКОАП.Наименование")));

//            lcs.LimitFunction = func;
//            var dos = dataService.LoadObjects(lcs);
//            lcs.LimitFunction = null;
//            var dos2 = dataService.LoadObjects(lcs);

//            Assert.True(dos2.Length == DataServiceLoader.CountЗадержанный &&
//                          dos.Length == 1 &&
//                          dos[0].__PrimaryKey.Equals(new KeyGuid("f36e9cf2-aed2-49e1-9c23-a0a3fbe90509")));
//        }

//        [Fact]
//        
//        public void TestMethod3()
//        {
//            // проверка наличия детейлов по условию сравнения собственного свойства детейла и собственного свойства агрегатора
//            IDataService dataService = DataServiceLoader.InitializeDataSetvice();
//            View view = Information.GetView("ЗадержанныйL", typeof(Задержанный));
//            View view2 = Information.GetView("ПричинаЗадержанияL", typeof(ПричинаЗадержания));
//            view.AddDetailInView("Причины", view2, true);
//            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Задержанный), view);
//            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
//            var detail = new DetailVariableDef(langDef.GetObjectType("Details"), "Причины", view2, "Задержанный");
//            var func = langDef.GetFunction(langDef.funcExist,
//                                           detail,
//                                           langDef.GetFunction(langDef.funcLEQ,
//                                                               new VariableDef(langDef.DateTimeType, "Задержанный.Номер"),
//                                                               new VariableDef(langDef.DateTimeType, "Номер")));

//            lcs.LimitFunction = func;
//            var dos = dataService.LoadObjects(lcs);
//            lcs.LimitFunction = null;
//            var dos2 = dataService.LoadObjects(lcs);

//            Assert.True(dos2.Length == DataServiceLoader.CountЗадержанный &&
//                          dos.Length == 1 &&
//                          dos[0].__PrimaryKey.Equals(new KeyGuid("f36e9cf2-aed2-49e1-9c23-a0a3fbe90509")));
//        }

//        [Fact]
//        
//        public void TestMethod4()
//        {
//            // проверка наличия детейлов по условию сравнения собственного свойства детейла и мастерового свойства агрегатора
//            IDataService dataService = DataServiceLoader.InitializeDataSetvice();
//            View view = Information.GetView("ЗадержанныйL", typeof(Задержанный));
//            View view2 = Information.GetView("ПричинаЗадержанияL", typeof(ПричинаЗадержания));
//            view.AddDetailInView("Причины", view2, true);
//            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Задержанный), view);
//            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
//            var detail = new DetailVariableDef(langDef.GetObjectType("Details"), "Причины", view2, "Задержанный");
//            var func = langDef.GetFunction(langDef.funcExist,
//                                           detail,
//                                           langDef.GetFunction(langDef.funcNEQ,
//                                                               new VariableDef(langDef.StringType, "Задержанный.АдресПроживания.НаимУлицы"),
//                                                               new VariableDef(langDef.StringType, "Комментарий")));

//            lcs.LimitFunction = func;
//            var dos = dataService.LoadObjects(lcs);
//            lcs.LimitFunction = null;
//            var dos2 = dataService.LoadObjects(lcs);

//            Assert.True(dos2.Length == DataServiceLoader.CountЗадержанный &&
//                          dos.Length == 1 &&
//                          dos[0].__PrimaryKey.Equals(new KeyGuid("acd98e27-f498-40c3-ba20-4b3957678a6a")));
//        }

//        [Fact]
//        
//        public void TestMethod5()
//        {
//            // проверка наличия детейлов по условию с функцией и в нутри выражения
//            IDataService dataService = DataServiceLoader.InitializeDataSetvice();
//            View view = Information.GetView("ЗадержанныйL", typeof(Задержанный));
//            View view2 = Information.GetView("ПричинаЗадержанияL", typeof(ПричинаЗадержания));
//            view.AddDetailInView("Причины", view2, true);
//            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Задержанный), view);
//            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
//            var detail = new DetailVariableDef(langDef.GetObjectType("Details"), "Причины", view2, "Задержанный");
//            var func = langDef.GetFunction(langDef.funcExist,
//                                           detail,
//                                           langDef.GetFunction(langDef.funcNotIsNull, new VariableDef(langDef.GuidType, "Задержанный.АдресПроживания")));
            
//            lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, func, true);
//            var dos = dataService.LoadObjects(lcs);
//            lcs.LimitFunction = null;
//            var dos2 = dataService.LoadObjects(lcs);

//            Assert.True(dos2.Length == DataServiceLoader.CountЗадержанный &&
//                          dos.Length == 1 &&
//                          dos[0].__PrimaryKey.Equals(new KeyGuid("acd98e27-f498-40c3-ba20-4b3957678a6a")));
//        }


//        [Fact]
//        
//        public void TestMethod6()
//        {
//            // проверка наличия детейлов по собственным полям с указанием алиасов (на мaнер работы ограничений, созданных в редакторе ограничений)
//            IDataService dataService = DataServiceLoader.InitializeDataSetvice();
//            View view = Information.GetView("ЗадержанныйL", typeof(Задержанный));
//            View view2 = Information.GetView("ПричинаЗадержанияL", typeof(ПричинаЗадержания));
//            view.AddDetailInView("Причины", view2, true);
//            var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Задержанный), view);
//            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
//            var detail = new DetailVariableDef(langDef.GetObjectType("Details"), "Причины", view2, "Задержанный");
//            var func = langDef.GetFunction(langDef.funcExist,
//                                           detail,
//                                           langDef.GetFunction(langDef.funcNotIsNull, new VariableDef(langDef.GuidType, "Причины.ВидПодозрения")));
//            func = langDef.GetFunction(langDef.funcAND, func,
//                                       langDef.GetFunction(langDef.funcIsNull, new VariableDef(langDef.GuidType, "АдресПроживания")));

//            lcs.LimitFunction = langDef.GetFunction(langDef.funcEQ, func, true);
//            var dos = dataService.LoadObjects(lcs);
//            lcs.LimitFunction = null;
//            var dos2 = dataService.LoadObjects(lcs);

//            Assert.True(dos2.Length == DataServiceLoader.CountЗадержанный &&
//                          dos.Length == 1 &&
//                          dos[0].__PrimaryKey.Equals(new KeyGuid("f36e9cf2-aed2-49e1-9c23-a0a3fbe90509")));
//        }
//    }
//}

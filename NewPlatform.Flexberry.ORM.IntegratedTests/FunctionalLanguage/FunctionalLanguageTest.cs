using NewPlatform.Flexberry.ORM.Tests;

namespace ICSSoft.STORMNET.Tests.TestClasses.FunctionalLanguage
{
    using System;
    using System.Collections.Generic;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;

    
    public class FunctionalLanguageTest
    {
        [Fact]
        
        public void SqlWhereLangDefInitializationTest()
        {
            SQLWhereLanguageDef langdef = SQLWhereLanguageDef.LanguageDef;    

            Assert.True(langdef != null, "LanguageDef of SQLWhereLanguageDef could not be initialized");
            //проверим id функций, чтобы они не повторялись
            var funcIds = new List<int>(langdef.Functions.Count);
            foreach (FunctionDef func in langdef.Functions)
            {
                if (funcIds.Contains(func.ID))
                    Assert.True(false, String.Format("Functions in SQLWhereLanguageDef with the same id({0}) defined", func.ID));
                funcIds.Add(func.ID);
            }
        }

        [Fact]
        
        public void ExternalLangDefInitializationTest()
        {
            ExternalLangDef langdef = ExternalLangDef.LanguageDef;

            Assert.True(langdef != null, "LanguageDef of ExternalLangDef could not be initialized");

            //проверим id функций, чтобы они не повторялись
            var funcIds = new List<int>(langdef.Functions.Count);
            foreach (FunctionDef func in langdef.Functions)
            {
                if (funcIds.Contains(func.ID))
                    Assert.True(false, String.Format("Functions in ExternalLangDef with the same id({0}) defined", func.ID));
                funcIds.Add(func.ID);
            }
        }

        // TODO: перенести эту часть тестов в winforms.
        /*
        /// <summary>
        /// Проверка показа\скрытия стандартного редактора ограничений
        /// </summary>
        [Fact]
        
        public void StandartEditorTest()
        {
            //Test driven development of StandardViewControl

            var svc = new StandardViewControl();
            var langDef = ExternalLangDef.LanguageDef;
            svc.SetLangDef(langDef);

            var varDefDate = new VariableDef(langDef.DateTimeType, "ДатаПодачиЗаявки");
            var varDefBool = new VariableDef(langDef.BoolType, "Актуально");
            var paramBool = new ParameterDef("paramBool", langDef.BoolType, false, "");
            var paramDate = new ParameterDef("paramDate", langDef.DateTimeType, false, "");
            var paramNumeric = new ParameterDef("paramNumeric", langDef.NumericType, false, "");

            var viewDate = new View
                           {
                               DefineClassType = langDef.DatePartType.NetCompatibilityType
                           };
            const string caption = "Детейл";
            var detailVarDefDate = new DetailVariableDef(langDef.GetObjectType("Details"), caption, viewDate,
                                                     "ConnectMasterProp", "OwnerMasterProp");

            var viewBool = new View
                           {
                               DefineClassType = langDef.BoolType.NetCompatibilityType
                           };
            var detailVarDefBool = new DetailVariableDef(langDef.GetObjectType("Details"), caption, viewBool,
                                                     "ConnectMasterProp", "OwnerMasterProp");


            var filterSetting = new FilterSetting();

            var strigedTypeDate = langDef.DatePartType.NetCompatibilityType.FullName;
            var strigedTypeBool = langDef.BoolType.NetCompatibilityType.FullName;
            var doTypeDate = new DataObjectTypeWithViewStorage {Text = "view", type = strigedTypeDate, ViewName = "viewName"};
            var doTypeBool = new DataObjectTypeWithViewStorage {Text = "view", type = strigedTypeBool, ViewName = "viewName"};
            var filterDetailDate = new FilterDetail
                                   {
                                       FilterSetting = filterSetting,
                                       DataObjectView = doTypeBool,
                                       Caption = caption
                                   };

            var filterDetailBool = new FilterDetail
                                {
                                    FilterSetting = filterSetting,
                                    DataObjectView = doTypeBool,
                                    Caption = caption
                                };

            filterSetting.Details.Add(filterDetailDate);
            filterSetting.Details.Add(filterDetailBool);

            svc.FilterTreeControl = null;
            svc.FilterSetting = filterSetting;

            #region Функции для дат

            #region Отображаемые функции для дат

            //Год("ДатаПодачиЗаявки") = 1
            var func1 = langDef.GetFunction(langDef.funcEQ,
                                            langDef.GetFunction(langDef.funcYearPart, varDefDate),
                                            1);
            Assert.True(svc.CheckFunction(func1));

            //Только дата ("ДатаПодачиЗаявки") = Только дата (( СЕГОДНЯ()))
            var func2 = langDef.GetFunction(langDef.funcEQ,
                                            langDef.GetFunction(langDef.funcOnlyDate, varDefDate),
                                            langDef.GetFunction(langDef.funcOnlyDate,
                                                                langDef.GetFunction(langDef.paramTODAY)));
            Assert.True(svc.CheckFunction(func2));

            //Только дата ("ДатаПодачиЗаявки") = Только дата ("17.05.2011 0:00:00")
            var func5 = langDef.GetFunction(langDef.funcEQ,
                                            langDef.GetFunction(langDef.funcOnlyDate, varDefDate),
                                            langDef.GetFunction(langDef.funcOnlyDate,
                                                                new DateTime(2011, 05, 17, 0, 0, 0)));
            Assert.True(svc.CheckFunction(func5));


            //(Только дата ("ДатаПодачиЗаявки") >= Только дата (@param))
            var func21 = langDef.GetFunction(langDef.funcGEQ,
                                             langDef.GetFunction(langDef.funcOnlyDate, varDefDate),
                                             langDef.GetFunction(langDef.funcOnlyDate, paramDate));
            Assert.True(svc.CheckFunction(func21));

            //(ГОД ("ДатаПодачиЗаявки") <= @param)
            var func22 = langDef.GetFunction(langDef.funcEQ,
                                             langDef.GetFunction(langDef.funcYearPart, varDefDate),
                                             paramNumeric);
            Assert.True(svc.CheckFunction(func22));
            #endregion




            #region Не отображаемые функции для дат

            //Только время ("ДатаПодачиЗаявки") = Только дата (( СЕГОДНЯ()))
            var func3 = langDef.GetFunction(langDef.funcEQ,
                                            langDef.GetFunction(langDef.funcOnlyTime, varDefDate),
                                            langDef.GetFunction(langDef.funcOnlyDate,
                                                                langDef.GetFunction(langDef.paramTODAY)));
            Assert.False(svc.CheckFunction(func3));

            //Только дата ("ДатаПодачиЗаявки") = Только дата ("ДатаПодачиЗаявки")
            var func4 = langDef.GetFunction(langDef.funcEQ,
                                            langDef.GetFunction(langDef.funcOnlyDate, varDefDate),
                                            langDef.GetFunction(langDef.funcOnlyDate, varDefDate));
            Assert.False(svc.CheckFunction(func4));

            //"ДатаПодачиЗаявки" = Только время (Только время (( СЕГОДНЯ())))
            var func14 = langDef.GetFunction(langDef.funcEQ,
                                             varDefDate,
                                             langDef.GetFunction(langDef.funcOnlyTime,
                                                                 langDef.GetFunction(langDef.funcOnlyTime,
                                                                                     langDef.GetFunction(
                                                                                         langDef.paramTODAY)
                                                                     )));
            Assert.False(svc.CheckFunction(func14));

            //"ДатаПодачиЗаявки" = Только время (Только время ("ДатаПодачиЗаявки"))
            var func15 = langDef.GetFunction(langDef.funcEQ,
                                             varDefDate,
                                             langDef.GetFunction(langDef.funcOnlyTime,
                                                                 langDef.GetFunction(langDef.funcOnlyTime,
                                                                                     varDefDate)));
            Assert.False(svc.CheckFunction(func15));

            //"ДатаПодачиЗаявки" = Только время (Только время ("ДатаПодачиЗаявки"))
            var func17 = langDef.GetFunction(langDef.funcEQ,
                                             langDef.GetFunction(langDef.paramTODAY),
                                             varDefDate);
            Assert.False(svc.CheckFunction(func17));

            #endregion

            #endregion

            #region функции Boolean

            #region Отображаемые  функции Boolean

            // И (Не пусто ("Актуально"))
            var func7 = langDef.GetFunction(langDef.funcAND,
                                            langDef.GetFunction(langDef.funcNotIsNull, varDefBool));
            Assert.True(svc.CheckFunction(func7));


            //"Актуально" = параметр paramBool
            var func9 = langDef.GetFunction(langDef.funcEQ, varDefBool, paramBool);
            Assert.True(svc.CheckFunction(func9));

            //И ( ИЛИ (НЕ ( И ( НЕ ( "Актуально" = paramBool) ) ) ) )
            var func10 = langDef.GetFunction(langDef.funcAND,
                                             langDef.GetFunction(langDef.funcOR,
                                                                 langDef.GetFunction(langDef.funcNOT,
                                                                                     langDef.GetFunction(
                                                                                         langDef.funcAND,
                                                                                         langDef.GetFunction(
                                                                                             langDef.funcNOT,
                                                                                             langDef.GetFunction(
                                                                                                 langDef.funcEQ,
                                                                                                 varDefBool, paramBool)
                                                                                             )
                                                                                         )
                                                                     )
                                                 ));
            Assert.True(svc.CheckFunction(func10));

            //"Актуально" = Истина()
            var func19 = langDef.GetFunction(langDef.funcEQ,
                                             varDefBool,
                                             langDef.GetFunction(langDef.paramTrue));
            Assert.True(svc.CheckFunction(func19));

            //И ("Актуально" = Истина())
            var func20 = langDef.GetFunction(langDef.funcAND,
                                             langDef.GetFunction(langDef.funcEQ,
                                                                 varDefBool,
                                                                 langDef.GetFunction(langDef.paramTrue)));
            Assert.True(svc.CheckFunction(func20));

            #endregion

            #region Не отображаемые  функции Boolean

            //Не пусто ( НЕ ("Актуально"))
            var func6 = langDef.GetFunction(langDef.funcNotIsNull,
                                            langDef.GetFunction(langDef.funcNOT, varDefBool));
            Assert.False(svc.CheckFunction(func6));


            //"Актуально" = "Актуально"
            var func8 = langDef.GetFunction(langDef.funcEQ, varDefBool, varDefBool);
            Assert.False(svc.CheckFunction(func8));

            //И(НЕ(Истина()))
            var func23 = langDef.GetFunction(langDef.funcAND,
                                             langDef.GetFunction(langDef.funcNOT,
                                                                 langDef.GetFunction(langDef.paramTrue)));
            Assert.False(svc.CheckFunction(func23));
            #endregion

            #endregion


            #region Функции с детейлами

            #region Отображаемые функции с детейлами

            var func12 = langDef.GetFunction(langDef.funcExist,
                                             detailVarDefDate,
                                             langDef.GetFunction(langDef.funcEQ,
                                                                 langDef.GetFunction(langDef.funcYearPart,
                                                                                     varDefDate),
                                                                 1));
            Assert.True(svc.CheckFunction(func12));

            //(Существуют такие (Ф4) , что (ДатаПодачиЗаявки СРЕДИ {СЕГОДНЯ(), ДатаПодачиЗаявки , 08.07.2011 0:00:00}))
            var func16 = langDef.GetFunction(langDef.funcExist,
                                             detailVarDefDate,
                                             langDef.GetFunction(langDef.funcIN, 
                                                                 langDef.GetFunction(langDef.paramTODAY),
                                                                 varDefDate,
                                                                 DateTime.Parse("08.07.2011")));
            Assert.True(svc.CheckFunction(func16));

            //(Существуют такие (05. Заявления УД) , что НЕ 05. Заявления УД.Основное)
            var func18 = langDef.GetFunction(langDef.funcExist,
                                             detailVarDefBool,
                                             langDef.GetFunction(langDef.funcNOT, varDefBool));
            Assert.True(svc.CheckFunction(func18));
            #endregion


            #region Не отображаемые функции с детейлами

            var func13 = langDef.GetFunction(langDef.funcExist,
                                             detailVarDefDate,
                                             langDef.GetFunction(langDef.funcEQ,
                                                                 langDef.GetFunction(langDef.funcYearPart, varDefDate),
                                                                 langDef.GetFunction(langDef.funcDayPart, varDefDate)));
            Assert.False(svc.CheckFunction(func13));

            #endregion

            #endregion

        }
        */
        /// <summary>
        /// Проверка обработки null, переданного в качестве параметра при построении функции
        /// </summary>
        [Fact]
        
        public void NullParametersTest()
        {
            var ldef = ExternalLangDef.LanguageDef;

            // Сущность == null заменяется на Сущность is null
            Function lf = ldef.GetFunction(
                ldef.funcEQ,
                new VariableDef(ldef.DateTimeType, Information.ExtractPropertyPath<Кредит>(x => x.ДатаВыдачи)),
                null);
            Function lfetalone = ldef.GetFunction(
                ldef.funcIsNull,
                new VariableDef(ldef.DateTimeType, Information.ExtractPropertyPath<Кредит>(x => x.ДатаВыдачи)));
            Assert.Equal(lfetalone.ToString(), lf.ToString());

            // null == Сущность заменяется на Сущность is null
            lf = ldef.GetFunction(
                ldef.funcEQ,
                null,
                new VariableDef(ldef.DateTimeType, Information.ExtractPropertyPath<Кредит>(x => x.ДатаВыдачи)));
            Assert.Equal(lfetalone.ToString(), lf.ToString());

            // null == null заменяется на True
            lf = ldef.GetFunction(ldef.funcEQ, null, null);
            lfetalone = ldef.GetFunction(ldef.paramTrue);
            Assert.Equal(lfetalone.ToString(), lf.ToString());

            // null != null заменяется на False
            lf = ldef.GetFunction(ldef.funcNEQ, null, null);
            lfetalone = ldef.GetFunction(ldef.funcNOT, ldef.GetFunction(ldef.paramTrue));
            Assert.Equal(lfetalone.ToString(), lf.ToString());
        }
    }
}

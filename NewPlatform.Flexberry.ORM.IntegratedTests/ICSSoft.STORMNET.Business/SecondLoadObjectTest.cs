namespace ICSSoft.STORMNET.Tests.TestClasses.Business
{
    using Xunit;

    public class SecondLoadObjectTest
    {/*
        /// <summary>
        /// Конструктор.
        /// </summary>
        public LoadObjectTest()
            : base("SLOTest")
        {
        }

        private Random _random = new Random();

        /// <summary>
        /// Получить единичный объект из БД
        /// </summary>
        /// <param name="медведь"></param>
        /// <param name="props">Свойства, которые надо добавить в представление</param>
        /// <param name="initDataCopy"></param>
        /// <returns></returns>
        private SQLDataService GetObject(out Медведь медведь, string[] props, bool initDataCopy = true)
        {
            SQLDataService ds = (SQLDataService)DataServiceProvider.DataService;

            Assert.NotNull(ds);
            View view = new View();
            view.DefineClassType = typeof(Медведь);
            foreach (string prop in props)
            {
                view.AddProperty(prop);
            }
            LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);
            lcs.ColumnsSort = new[] { new ColumnsSortDef("__PrimaryKey", _random.Next(10) > 5 ? SortOrder.Asc : SortOrder.Desc) };
            lcs.InitDataCopy = initDataCopy;
            var dataObjects = ds.LoadObjects(lcs);

            Assert.True(dataObjects.Length > 0);

            медведь = (Медведь)dataObjects[0];
            return ds;
        }

        /// <summary>
        /// Получить единичный объект из БД с детейлами
        /// </summary>
        /// <param name="медведь"></param>
        /// <param name="props">Свойства, которые надо добавить в представление</param>
        /// <param name="detailProps">Свойства, которые надо добавить в детейловое представление</param>
        /// <param name="initDataCopy">Инициализировать ли копию данных</param>
        /// <returns></returns>
        private SQLDataService GetObjectWithDetails(out Медведь медведь, string[] props, string[] detailProps, bool initDataCopy = true)
        {
            SQLDataService ds = (SQLDataService)DataServiceProvider.DataService;

            Assert.NotNull(ds);
            View view = new View();
            view.DefineClassType = typeof(Медведь);
            foreach (string prop in props)
            {
                view.AddProperty(prop);
            }

            View detailVeiw = new View();
            detailVeiw.DefineClassType = typeof(Берлога);
            foreach (string prop in detailProps)
            {
                detailVeiw.AddProperty(prop);
            }

            view.AddDetailInView("Берлога", detailVeiw, true);

            LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);
            lcs.ColumnsSort = new[] { new ColumnsSortDef("__PrimaryKey", _random.Next(10) > 5 ? SortOrder.Asc : SortOrder.Desc) };
            lcs.InitDataCopy = initDataCopy;
            var dataObjects = ds.LoadObjects(lcs);

            Assert.True(dataObjects.Length > 0);

            медведь = (Медведь)dataObjects[0];
            return ds;
        }

        #region Проверка на адекватное количество загруженных свойств после догрузки

        /// <summary>
        /// Проверим сначала собственные свойства
        /// </summary>
        [Fact]
        public void SecondLoadTestLoadedProperties1()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "Вес" });

            var alteredPropertyNames = медведь.GetAlteredPropertyNames();
            Assert.NotNull(alteredPropertyNames);
            Assert.Equal(0, alteredPropertyNames.Length);

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(1, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("ПорядковыйНомер");
            secondView.AddProperty("Вес");
            secondView.AddProperty("Gender");
            secondView.AddProperty("BirthDate");
            secondView.AddProperty("ЦветГлаз");

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(5, loadedProperties.Length);
        }

        /// <summary>
        /// Дочитка мастеров и загруженные ранее свойства
        /// </summary>
        [Fact]
        public void SecondLoadTestLoadedProperties2()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "Вес" });

            var alteredPropertyNames = медведь.GetAlteredPropertyNames();
            Assert.NotNull(alteredPropertyNames);
            Assert.Equal(0, alteredPropertyNames.Length);

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(1, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("ПорядковыйНомер");
            secondView.AddProperty("Вес");
            secondView.AddProperty("Gender");
            secondView.AddProperty("Папа.ЦветГлаз");
            secondView.AddProperty("ЛесОбитания.Название");
            secondView.AddProperty("ЛесОбитания.Страна.Название");

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
        }

        #endregion

        #region Проверка на AlteredPropertyNames

        /// <summary>
        /// Если изменим свойство дальнего мастера, будет ли это видно в самом объекте?
        /// </summary>
        [Fact]
        public void SecondLoadTestAlteredProperiNames1()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "Вес", "ЛесОбитания.Площадь" });

            var alteredPropertyNames = медведь.GetAlteredPropertyNames();
            Assert.NotNull(alteredPropertyNames);
            Assert.Equal(0, alteredPropertyNames.Length);

            if (медведь.ЛесОбитания != null)
            {
                медведь.ЛесОбитания.Площадь = -300;
            }
            alteredPropertyNames = медведь.GetAlteredPropertyNames();
            Assert.Equal(0, alteredPropertyNames.Length);

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(2, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("ПорядковыйНомер");
            secondView.AddProperty("Вес");
            secondView.AddProperty("Gender");
            secondView.AddProperty("Папа.ЦветГлаз");
            secondView.AddProperty("ЛесОбитания.Название");
            secondView.AddProperty("ЛесОбитания.Страна.Название");

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
        }

        /// <summary>
        /// Если изменим свойство дальнего мастера, будет ли это видно в самом объекте?
        /// </summary>
        [Fact]
        public void SecondLoadTestAlteredProperiNames2()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "BirthDate", "ЛесОбитания.Площадь" });
            Assert.NotNull(медведь);
            var alteredPropertyNames = медведь.GetAlteredPropertyNames();
            Assert.NotNull(alteredPropertyNames);
            Assert.Equal(0, alteredPropertyNames.Length);
            //int orig = ((Медведь) медведь.GetDataCopy()).Вес;
            if (медведь.ЛесОбитания != null)
            {
                медведь.ЛесОбитания.Площадь = -300;
                ((Медведь)медведь.GetDataCopy()).Вес = -400;
            }
            alteredPropertyNames = медведь.GetAlteredPropertyNames();
            Assert.Equal(1, alteredPropertyNames.Length);

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(2, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("ПорядковыйНомер");
            secondView.AddProperty("Вес");
            secondView.AddProperty("Gender");
            secondView.AddProperty("Папа.ЦветГлаз");
            secondView.AddProperty("ЛесОбитания.Название");
            secondView.AddProperty("ЛесОбитания.Площадь");
            secondView.AddProperty("ЛесОбитания.Страна.Название");

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            loadedProperties = медведь.GetLoadedProperties();
            alteredPropertyNames = медведь.GetAlteredPropertyNames(true);
            Assert.Equal(1, alteredPropertyNames.Length);

            //Assert.Equal(((Медведь)медведь.GetDataCopy()).Вес, orig);

            Assert.NotNull(loadedProperties);

            Assert.Equal(-300, медведь.ЛесОбитания.Площадь);
        }

        #endregion

        #region Дочитка собственных свойств

        /// <summary>
        /// Дочитка собственных свойств. Без изменения.
        /// </summary>
        [Fact]
        public void SecondLoadTestOwn1()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "BirthDate", "Вес" });
            Assert.NotNull(медведь);
            var alteredPropertyNames = медведь.GetAlteredPropertyNames();
            Assert.NotNull(alteredPropertyNames);
            Assert.Equal(0, alteredPropertyNames.Length);

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(2, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("ПорядковыйНомер");
            secondView.AddProperty("Вес");
            secondView.AddProperty("Gender");

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            loadedProperties = медведь.GetLoadedProperties();
            alteredPropertyNames = медведь.GetAlteredPropertyNames(true);
            Assert.Equal(0, alteredPropertyNames.Length);

            Assert.NotNull(loadedProperties);

            Assert.Equal(4, loadedProperties.Length);
        }

        /// <summary>
        /// Дочитка собственных свойств. Загруженное свойство было изменено и оно же будет дочитано.
        /// </summary>
        [Fact]
        public void SecondLoadTestOwn2()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "BirthDate", "Вес" });
            Assert.NotNull(медведь);
            var alteredPropertyNames = медведь.GetAlteredPropertyNames();
            Assert.NotNull(alteredPropertyNames);
            Assert.Equal(0, alteredPropertyNames.Length);

            медведь.Вес = -14359;

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(loadedProperties.Length, 2);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("ПорядковыйНомер");
            secondView.AddProperty("Вес");
            secondView.AddProperty("Gender");

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            loadedProperties = медведь.GetLoadedProperties();
            alteredPropertyNames = медведь.GetAlteredPropertyNames(true);
            Assert.Equal(1, alteredPropertyNames.Length);
            Assert.Equal("Вес", alteredPropertyNames[0]);

            Assert.Equal(медведь.Вес, -14359);

            Assert.NotNull(loadedProperties);

            Assert.Equal(4, loadedProperties.Length);
        }

        /// <summary>
        /// Дочитка собственных свойств. Не загруженное свойство было изменено и оно будет дочитано.
        /// </summary>
        [Fact]
        public void SecondLoadTestOwn3()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "BirthDate", "ЦветГлаз" });
            Assert.NotNull(медведь);
            var alteredPropertyNames = медведь.GetAlteredPropertyNames();
            Assert.NotNull(alteredPropertyNames);
            Assert.Equal(0, alteredPropertyNames.Length);

            медведь.Вес = -14359;

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(2, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("ПорядковыйНомер");
            secondView.AddProperty("Вес");
            secondView.AddProperty("Gender");

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            loadedProperties = медведь.GetLoadedProperties();
            alteredPropertyNames = медведь.GetAlteredPropertyNames(true);
            Assert.Equal(1, alteredPropertyNames.Length);
            Assert.Equal("Вес", alteredPropertyNames[0]);

            Assert.Equal(медведь.Вес, -14359);

            Assert.NotNull(loadedProperties);

            Assert.Equal(5, loadedProperties.Length);
            Assert.Equal(ObjectStatus.Altered, медведь.GetStatus(true));

        }

        /// <summary>
        /// Дочитка собственных свойств. Не загруженное свойство было изменено и оно не будет дочитано.
        /// </summary>
        [Fact]
        public void SecondLoadTestOwn4()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "BirthDate", "ЦветГлаз" });
            Assert.NotNull(медведь);
            var alteredPropertyNames = медведь.GetAlteredPropertyNames();
            Assert.NotNull(alteredPropertyNames);
            Assert.Equal(0, alteredPropertyNames.Length);

            медведь.Вес = -14359;

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(2, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("ПорядковыйНомер");
            secondView.AddProperty("BirthDate");
            secondView.AddProperty("Gender");

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            loadedProperties = медведь.GetLoadedProperties();
            alteredPropertyNames = медведь.GetAlteredPropertyNames(true);
            Assert.Equal(1, alteredPropertyNames.Length);
            Assert.Equal("Вес", alteredPropertyNames[0]);

            Assert.Equal(медведь.Вес, -14359);

            Assert.NotNull(loadedProperties);

            Assert.Equal(4, loadedProperties.Length);
            Assert.Equal(ObjectStatus.Altered, медведь.GetStatus(true));
        }
        #endregion


        #region Дочитка собственных свойств: выкрутасы с копией данных

        /// <summary>
        /// Дочитка собственных свойств. Копию данных не инициализировали первый раз.
        /// </summary>
        [Fact]
        public void SecondLoadTestOwnDataCopy1()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "BirthDate", "Вес" }, false);
            Assert.NotNull(медведь);
            Assert.Null(медведь.GetDataCopy());

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(2, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("ПорядковыйНомер");
            secondView.AddProperty("Вес");
            secondView.AddProperty("Gender");

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            Assert.NotNull(медведь.GetDataCopy());

            loadedProperties = медведь.GetLoadedProperties();
            string[] alteredPropertyNames = медведь.GetAlteredPropertyNames(true);
            if (медведь.BirthDate != null)
            {
                Assert.Equal(1, alteredPropertyNames.Length);
            }
            else
            {
                Assert.Equal(0, alteredPropertyNames.Length);
            }

            Assert.NotNull(loadedProperties);

            Assert.Equal(4, loadedProperties.Length);
        }

        /// <summary>
        /// Дочитка собственных свойств. Копия данных не инициализируется на уровне объекта данных.
        /// </summary>
        [Fact]
        public void SecondLoadTestOwnDataCopy2()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "BirthDate", "Вес" }, false);
            Assert.NotNull(медведь);
            Assert.Null(медведь.GetDataCopy());

            медведь.DisableInitDataCopy();

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(2, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("ПорядковыйНомер");
            secondView.AddProperty("Вес");
            secondView.AddProperty("Gender");

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            Assert.Null(медведь.GetDataCopy());

            loadedProperties = медведь.GetLoadedProperties();
            Assert.NotNull(loadedProperties);

            Assert.Equal(4, loadedProperties.Length);
        }

        #endregion

        #region дочитка мастеров

        /// <summary>
        /// Дочитка мастеров.
        /// </summary>
        [Fact]
        public void SecondLoadTestMasters1()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "ЛесОбитания.Заповедник", "Мама", "Мама.ЦветГлаз" });
            Assert.NotNull(медведь);
            Assert.NotNull(медведь.GetDataCopy());

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(2, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("Папа.Папа.Вес");
            secondView.AddProperty("ЛесОбитания.Заповедник");
            secondView.AddProperty("ЛесОбитания.ДатаПоследнегоОсмотра");
            secondView.AddProperty("ЛесОбитания.Страна");
            secondView.AddProperty("ЛесОбитания.Страна.Название");

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            Assert.NotNull(медведь.GetDataCopy());

            loadedProperties = медведь.GetLoadedProperties();
            Assert.NotNull(loadedProperties);

            Assert.Equal(3, loadedProperties.Length);

            Assert.NotNull(медведь.ЛесОбитания.Страна);
        }
        #endregion

        #region дочитка детейлов

        /// <summary>
        /// Дочитка детейлов. Детейл не был зачитан, дочитывается
        /// </summary>
        [Fact]
        public void SecondLoadTestDetails1()
        {
            Медведь медведь;
            SQLDataService ds = GetObject(out медведь, new[] { "ЛесОбитания", "ЛесОбитания.Заповедник", "Мама", "Мама.ЦветГлаз", "Вес" });

            Assert.NotNull(медведь);
            Assert.NotNull(медведь.GetDataCopy());

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(3, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("Папа");
            secondView.AddProperty("Папа.Папа.Вес");
            secondView.AddProperty("ЛесОбитания");
            secondView.AddProperty("ЛесОбитания.Заповедник");
            secondView.AddProperty("ЛесОбитания.ДатаПоследнегоОсмотра");
            secondView.AddProperty("ЛесОбитания.Страна");
            secondView.AddProperty("ЛесОбитания.Страна.Название");

            View detailVeiw = new View();
            detailVeiw.DefineClassType = typeof(Берлога);
            detailVeiw.AddProperty("Наименование");
            detailVeiw.AddProperty("Комфортность");
            detailVeiw.AddProperty("Заброшена");
            secondView.AddDetailInView("Берлога", detailVeiw, true);

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            Assert.NotNull(медведь.GetDataCopy());

            loadedProperties = медведь.GetLoadedProperties();
            Assert.NotNull(loadedProperties);

            Assert.Equal(5, loadedProperties.Length);

            Assert.NotNull(медведь.ЛесОбитания.Страна);

            Assert.True(медведь.Берлога.Count > 0);

            List<string> detailLoadedProps = new List<string>(медведь.Берлога[0].GetLoadedProperties());

            Assert.True(detailLoadedProps.Contains("Комфортность"));
        }


        /// <summary>
        /// Дочитка детейлов. Детейлы были зачитаны, дочитываем.
        /// </summary>
        [Fact]
        public void SecondLoadTestDetails2()
        {
            Медведь медведь;
            SQLDataService ds = GetObjectWithDetails(out медведь, new[] { "ЛесОбитания", "ЛесОбитания.Заповедник", "Мама", "Мама.ЦветГлаз", "Вес" }, new[] { "Наименование" });

            Assert.NotNull(медведь);
            Assert.NotNull(медведь.GetDataCopy());

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(4, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("Папа");
            secondView.AddProperty("Папа.Папа.Вес");
            secondView.AddProperty("ЛесОбитания");
            secondView.AddProperty("ЛесОбитания.Заповедник");
            secondView.AddProperty("ЛесОбитания.ДатаПоследнегоОсмотра");
            secondView.AddProperty("ЛесОбитания.Страна");
            secondView.AddProperty("ЛесОбитания.Страна.Название");

            View detailVeiw = new View();
            detailVeiw.DefineClassType = typeof(Берлога);
            detailVeiw.AddProperty("Наименование");
            detailVeiw.AddProperty("Комфортность");
            detailVeiw.AddProperty("Заброшена");
            secondView.AddDetailInView("Берлога", detailVeiw, true);

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            Assert.NotNull(медведь.GetDataCopy());

            loadedProperties = медведь.GetLoadedProperties();
            Assert.NotNull(loadedProperties);

            Assert.Equal(5, loadedProperties.Length);

            Assert.NotNull(медведь.ЛесОбитания.Страна);

            Assert.True(медведь.Берлога.Count > 0);

            List<string> detailLoadedProps = new List<string>(медведь.Берлога[0].GetLoadedProperties());

            Assert.True(detailLoadedProps.Contains("Комфортность"));
        }

        /// <summary>
        /// Дочитка детейлов. Детейлы были зачитаны, дочитываем со свойствами мастеров детейлов.
        /// </summary>
        [Fact]
        public void SecondLoadTestDetails3()
        {
            Медведь медведь;
            SQLDataService ds = GetObjectWithDetails(out медведь, new[] { "ЛесОбитания", "ЛесОбитания.Заповедник", "Мама", "Мама.ЦветГлаз", "Вес" }, new[] { "Наименование" });

            Assert.NotNull(медведь);
            Assert.NotNull(медведь.GetDataCopy());

            var loadedProperties = медведь.GetLoadedProperties();

            Assert.NotNull(loadedProperties);
            Assert.Equal(4, loadedProperties.Length);

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("Папа");
            secondView.AddProperty("Папа.Папа.Вес");
            secondView.AddProperty("ЛесОбитания");
            secondView.AddProperty("ЛесОбитания.Заповедник");
            secondView.AddProperty("ЛесОбитания.ДатаПоследнегоОсмотра");
            secondView.AddProperty("ЛесОбитания.Страна");
            secondView.AddProperty("ЛесОбитания.Страна.Название");

            View detailVeiw = new View();
            detailVeiw.DefineClassType = typeof(Берлога);
            detailVeiw.AddProperty("Наименование");
            detailVeiw.AddProperty("Комфортность");
            detailVeiw.AddProperty("Заброшена");
            detailVeiw.AddProperty("ЛесРасположения.Страна.Название");
            secondView.AddDetailInView("Берлога", detailVeiw, true);

            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());

            Assert.NotNull(медведь.GetDataCopy());

            loadedProperties = медведь.GetLoadedProperties();
            Assert.NotNull(loadedProperties);

            Assert.Equal(5, loadedProperties.Length);

            Assert.NotNull(медведь.ЛесОбитания.Страна);

            Assert.True(медведь.Берлога.Count > 0);

            List<string> detailLoadedProps = new List<string>(медведь.Берлога[0].GetLoadedProperties());

            Assert.True(detailLoadedProps.Contains("Комфортность"));

            Assert.NotNull(медведь.Берлога[0].ЛесРасположения.Страна.Название);
        }
        #endregion

        #region Дочитка объектов в состоянии Created

        /// <summary>
        /// Объект был создан
        /// </summary>
        [Fact]
        public void SecondLoadTestCreated1()
        {
            Медведь медведь = new Медведь();
            медведь.Вес = 590;

            SQLDataService ds = (SQLDataService)DataServiceProvider.DataService;

            View secondView = new View();
            secondView.DefineClassType = typeof(Медведь);
            secondView.AddProperty("Папа");
            secondView.AddProperty("Папа.Папа.Вес");
            secondView.AddProperty("ЛесОбитания");
            secondView.AddProperty("ЛесОбитания.Заповедник");
            secondView.AddProperty("ЛесОбитания.ДатаПоследнегоОсмотра");
            secondView.AddProperty("ЛесОбитания.Страна");
            secondView.AddProperty("ЛесОбитания.Страна.Название");


            ds.SecondLoadObject(secondView, медведь, true, new DataObjectCache());



            Assert.Equal(0, медведь.GetLoadedProperties().Length);
        }

        #endregion
      * */
    }
}

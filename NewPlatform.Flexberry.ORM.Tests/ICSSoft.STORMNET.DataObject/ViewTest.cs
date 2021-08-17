namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Linq;

    using ICSSoft.STORMNET;

    using Xunit;

    /// <summary>
    /// Тесты для класса <see cref="ICSSoft.STORMNET.View"/>.
    /// </summary>
    public class ViewTest
    {
        /// <summary>
        /// Проверяется, что если в представлении для объекта включено невидимым свойство '*',
        /// то все свойства будут невидимыми.
        /// </summary>
        [Fact]
        public void TestInvisibleView()
        {
            // Act.
            View view = FullTypesMainAgregator.Views.OwnInvisibleView;
            int visibleCount = view.Properties.Count(x => x.Visible);
            int wholeCount = view.Properties.Count();
            int rightStartCount = view.Properties.Count(x => x.Name != null);

            // Assert.
            Assert.Equal(0, visibleCount);
            Assert.True(rightStartCount > 1);
            Assert.Equal(wholeCount, rightStartCount);
        }

        /// <summary>
        /// Проверяется, что если в представлении для объекта включено невидимым свойство '*' какого-либо мастера ('master.*'),
        /// то все свойства мастера будут невидимыми.
        /// </summary>
        [Fact]
        public void TestInvisibleViewMaster()
        {
            // Act.
            View view = FullTypesMainAgregator.Views.MasterInvisibleView;
            int visibleCount = view.Properties.Count(x => x.Visible);
            int wholeCount = view.Properties.Count();
            int rightStartCount = view.Properties.Count(x => x.Name != null && x.Name.StartsWith("FullTypesMaster1."));

            // Assert.
            Assert.Equal(1, visibleCount);
            Assert.True(rightStartCount > 1);
            Assert.Equal(wholeCount - 2, rightStartCount);
        }

        /// <summary>
        /// Проверяется, что если в представлении для объекта добавлено невидимым свойство '*',
        /// то все свойства будут невидимыми.
        /// </summary>
        [Fact]
        public void TestInvisibleViewOnAddProperty()
        {
            // Arrange.
            View view = new View { DefineClassType = typeof(FullTypesMainAgregator), Name = "TestView" };

            // Act.
            view.AddProperty("*", string.Empty, false, string.Empty);

            int visibleCount = view.Properties.Count(x => x.Visible);
            int wholeCount = view.Properties.Count();
            int rightStartCount = view.Properties.Count(x => x.Name != null);
            var namelessProps = view.Properties.Where(x => x.Name == null).ToList();

            // Assert.
            Assert.Equal(0, visibleCount);
            Assert.True(wholeCount > 1);
            Assert.Empty(namelessProps);
            Assert.Equal(wholeCount, rightStartCount);
        }

        /// <summary>
        /// Проверяется, что если в представлении для объекта добавлено невидимым свойство '*' какого-либо мастера ('master.*'),
        /// то все свойства мастера будут невидимыми.
        /// </summary>
        [Fact]
        public void TestInvisibleViewMasterOnAddProperty()
        {
            // Arrange.
            View view = new View { DefineClassType = typeof(FullTypesMainAgregator), Name = "TestView" };

            // Act.
            view.AddProperty("FullTypesMaster1.*", string.Empty, false, string.Empty);

            int visibleCount = view.Properties.Count(x => x.Visible);
            int wholeCount = view.Properties.Count();
            var namelessProps = view.Properties.Where(x => x.Name == null).ToList();
            int rightStartCount = view.Properties.Count(x => x.Name != null && x.Name.StartsWith("FullTypesMaster1."));

            // Assert.
            Assert.Equal(0, visibleCount);
            Assert.True(wholeCount > 1);
            Assert.Empty(namelessProps);
            Assert.Equal(wholeCount, rightStartCount);
        }

        /// <summary>
        /// Проверяется, что повторно свойства через "*" добавить нельзя.
        /// </summary>
        [Fact]
        public void TestDoubleAdd()
        {
            // Arrange.
            View view = new View { DefineClassType = typeof(FullTypesMainAgregator), Name = "TestView" };
            view.AddProperty("*", string.Empty, false, string.Empty);
            int wholeCount1 = view.Properties.Count();

            // Act.
            view.AddProperty("*", string.Empty, false, string.Empty);
            int wholeCount2 = view.Properties.Count();

            // Assert.
            Assert.Equal(wholeCount1, wholeCount2);
        }

        /// <summary>
        /// Проверяется, вычитание представлений с детейлами.
        /// </summary>
        [Fact]
        public void TestViewSubstraction()
        {
            // Arrange.
            View view = new View { DefineClassType = typeof(FullTypesMainAgregator), Name = "TestView" };
            View view2 = new View { DefineClassType = typeof(FullTypesMainAgregator), Name = "TestView2" };
            View detailView = new View { DefineClassType = typeof(FullTypesMainAgregator), Name = "DetailTestView" };
            view.AddProperty(Information.ExtractPropertyName<MasterClass>(a => a.IntMasterProperty));
            view.AddProperty(Information.ExtractPropertyName<MasterClass>(a => a.StringMasterProperty));
            view2.AddProperty(Information.ExtractPropertyName<MasterClass>(a => a.StringMasterProperty));
            view.AddDetailInView(Information.ExtractPropertyName<MasterClass>(a => a.DetailClass), detailView, true);
            view2.AddDetailInView(Information.ExtractPropertyName<MasterClass>(a => a.DetailClass), detailView, true);

            // Act.
            View subsView = view - view2;

            // Assert.
            Assert.Empty(subsView.Details);
            Assert.Single(subsView.Properties);
            Assert.True(subsView.CheckPropname(Information.ExtractPropertyName<MasterClass>(a => a.IntMasterProperty)));
        }

        /// <summary>
        /// Проверяется работа делегата настройки представления.
        /// </summary>
        [Fact]
        public void TestTuneStaticView()
        {
            // Arrange.
            View view = InformationTestClass.Views.InformationTestClassE;
            Assert.True(view.CheckPropname(Information.ExtractPropertyPath<InformationTestClass>(i => i.PublicStringProperty)));

            Information.TuneStaticViewDelegate = tuneStaticView;

            // Act.
            view = InformationTestClass.Views.InformationTestClassE;

            // Assert.
            Assert.False(view.CheckPropname(Information.ExtractPropertyPath<InformationTestClass>(i => i.PublicStringProperty)));

            Information.TuneStaticViewDelegate = null;
        }

        /// <summary>
        /// Проверяется обработка ситуации при добавлении повторяющегося свойства в представление методом <see cref="View.AddProperty(string)"/>.
        /// </summary>
        [Fact]
        public void TestAddPropertyToView()
        {
            // Arrange.
            View view = InformationTestClass.Views.InformationTestClassE;
            string propName = Information.ExtractPropertyPath<InformationTestClass>(i => i.PublicStringProperty);
            Assert.True(view.CheckPropname(propName));
            int propCount = view.Properties.Length;

            // Act.
            view.AddProperty(propName);

            // Assert.
            Assert.Equal(propCount, view.Properties.Length);
        }

        /// <summary>
        /// Проверяется обработка ситуации при добавлении повторяющегося свойства в представление методом <see cref="View.AddProperties(string[])"/>.
        /// </summary>
        [Fact]
        public void TestAddPropertiesToView()
        {
            // Arrange.
            View view = InformationTestClass.Views.InformationTestClassE;
            string propName = Information.ExtractPropertyPath<InformationTestClass>(i => i.PublicStringProperty);
            Assert.True(view.CheckPropname(propName));
            int propCount = view.Properties.Length;

            // Act.
            view.AddProperties(new[] { propName });

            // Assert.
            Assert.Equal(propCount, view.Properties.Length);
        }

        private View tuneStaticView(string viewName, Type type, View view)
        {
            view.RemoveProperty(Information.ExtractPropertyPath<InformationTestClass>(i => i.PublicStringProperty));
            return view;
        }
    }
}

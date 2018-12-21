namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Linq.Expressions;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.Windows.Forms;

    public abstract class BaseFunctionTest : BaseTest
    {
        protected const string PropertyName = "PropertyName";

        protected static readonly Function FuncTrue = FunctionBuilder.BuildTrue();

        protected static readonly Function FuncFalse = FunctionBuilder.BuildFalse();

        protected static readonly Function FuncSQL = FunctionBuilder.BuildSQL("sql");

        protected static readonly Guid Guid1 = Guid.NewGuid();

        protected static readonly Guid Guid2 = Guid.NewGuid();

        protected static readonly KeyGuid KeyGuid1 = KeyGuid.NewGuid();

        protected static readonly KeyGuid KeyGuid2 = KeyGuid.NewGuid();

        protected static readonly TestDataObject TestDataObject1 = new TestDataObject();

        protected static readonly TestDataObject TestDataObject2 = new TestDataObject();

        protected static readonly DateTime DateTime1 = DateTime.Now;

        protected static readonly DateTime DateTime2 = DateTime.Today;

        protected const bool Bool1 = true;

        protected const int Int1 = 1;

        protected const decimal Decimal1 = decimal.One;

        protected const string String1 = "asd";

        protected static readonly VariableDef PrimaryKeyVarDef = new VariableDef(LangDef.GuidType, SQLWhereLanguageDef.StormMainObjectKey);

        protected static readonly VariableDef GuidVarDef = new VariableDef(LangDef.GuidType, PropertyName);

        protected static readonly VariableDef BoolVarDef = new VariableDef(LangDef.BoolType, PropertyName);

        protected static readonly VariableDef NumericVarDef = new VariableDef(LangDef.NumericType, PropertyName);

        protected static readonly VariableDef StringVarDef = new VariableDef(LangDef.StringType, PropertyName);

        protected static readonly VariableDef DateVarDef = new VariableDef(LangDef.DateTimeType, PropertyName);

        protected static readonly VariableDef GuidGenVarDef = new VariableDef(LangDef.GuidType, Information.ExtractPropertyName<TestDataObject>(x => x.Hierarchy));

        protected static readonly VariableDef DateGenVarDef = new VariableDef(LangDef.DateTimeType, Information.ExtractPropertyPath<TestDataObject>(x => x.BirthDate));

        protected static readonly VariableDef DateGenVarDef1 = new VariableDef(LangDef.DateTimeType, Information.ExtractPropertyPath<TestDataObject>(x => x.DeathDate));

        protected static readonly VariableDef IntGenVarDef = new VariableDef(LangDef.NumericType, Information.ExtractPropertyPath<TestDataObject>(x => x.Height));

        protected static readonly VariableDef IntGenVarDef1 = new VariableDef(LangDef.NumericType, Information.ExtractPropertyPath<TestDataObject>(x => x.Weight));

        protected static readonly VariableDef StringGenVarDef = new VariableDef(LangDef.StringType, Information.ExtractPropertyPath<TestDataObject>(x => x.Name));

        protected static readonly VariableDef StringGenVarDef1 = new VariableDef(LangDef.StringType, Information.ExtractPropertyPath<TestDataObject>(x => x.NickName));

        protected static readonly VariableDef StringGenVarDef2 = new VariableDef(LangDef.StringType, Information.ExtractPropertyPath<TestDataObject>(x => x.Hierarchy.Name));

        protected const string NullString = null;

        protected const object NullObject = null;

        protected const object[] NullObjects = null;

        protected static readonly TestDataObject NullTestDataObject = null;

        protected static readonly Function NullFunction = null;

        protected static readonly ObjectType NullObjectType = null;

        protected static readonly VariableDef NullVarDef = null;

        protected static readonly DetailVariableDef NullDetailVarDef = null;

        protected static readonly View NullView = null;

        protected static readonly Expression<Func<TestDataObject, object>> NullLambda = null;
    }
}
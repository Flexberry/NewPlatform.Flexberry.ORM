// TODO: разобраться с этим классом.
//namespace ICSSoft.STORMNET.Tests.TestClasses.Tools
//{
//    using System;
//    using System.Windows.Forms;

//    using ICSSoft.STORMNET.FunctionalLanguage;
//    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
//    using ICSSoft.STORMNET.Windows.Forms;

//    using Xunit;

//    /// <summary>
//    ///This is a test class for UtilsTest and is intended
//    ///to contain all UtilsTest Unit Tests
//    ///</summary>
//    [TestClass()]
//    public class UtilsTest
//    {


//        private TestContext testContextInstance;

//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        /// <summary>
//        ///A test for ObjectToString
//        ///</summary>
//        [Fact]
//        public void ObjectToStringTest()
//        {
//            Assert.Inconclusive("Verify the correctness of this test method.");
//            object o = null; // TODO: Initialize to an appropriate value
//            string expected = string.Empty; // TODO: Initialize to an appropriate value
//            string actual;
//            actual = Windows.Forms.Utils.ObjectToString(o);
//            Assert.Equal(expected, actual);
//        }

//        /// <summary>
//        ///A test for ObjectFromString
//        ///</summary>
//        [Fact]
//        public void ObjectFromStringTest()
//        {
//            Assert.Inconclusive("Verify the correctness of this test method.");
//            string s = string.Empty; // TODO: Initialize to an appropriate value
//            object expected = null; // TODO: Initialize to an appropriate value
//            object actual;
//            actual = Windows.Forms.Utils.ObjectFromString(s);
//            Assert.Equal(expected, actual);
//        }

//        [Fact]
//        
//        public void ObjectToAndFromStringTest()
//        {
//            SQLWhereLanguageDef ldef = SQLWhereLanguageDef.LanguageDef;
//            Function fn = ldef.GetFunction(
//                ldef.funcAND,
//                ldef.GetFunction(
//                ldef.funcEQ, new VariableDef(ldef.StringType,"ПарамПамПам"), "кто ходит в гости по утрам"
//                ),
//                ldef.GetFunction(
//                ldef.funcOR,
//                ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.StringType, "ТотПоступаетМудро"), Environment.UserName),
//                ldef.GetFunction(ldef.funcIsNull, new VariableDef(ldef.StringType, "НаТоОноИУтро"))
//                )
//                );

//            string serializedFn = Windows.Forms.Utils.ObjectToString((new ExternalLangDef()).FunctionToSimpleStruct(fn));
//            Assert.NotNull(serializedFn);
//            Console.WriteLine(serializedFn);

//            Function восставшийИзНебытия = (
//                                new ExternalLangDef()).FunctionFromSimpleStruct(Windows.Forms.Utils.ObjectFromString(serializedFn));
//            Assert.NotNull(восставшийИзНебытия);
//        }


//        /// <summary>
//        ///A test for GetControlPath
//        ///</summary>
//        [Fact]
//        public void GetControlPathTest()
//        {
//            Assert.Inconclusive("Verify the correctness of this test method.");
//            Control cntrl = null; // TODO: Initialize to an appropriate value
//            string expected = string.Empty; // TODO: Initialize to an appropriate value
//            string actual;
//            actual = Windows.Forms.Utils.GetControlPath(cntrl);
//            Assert.Equal(expected, actual);
//        }

//        /// <summary>
//        ///A test for GetControl
//        ///</summary>
//        [Fact]
//        public void GetControlTest()
//        {
//            Assert.Inconclusive("Verify the correctness of this test method.");
//            string path = string.Empty; // TODO: Initialize to an appropriate value
//            Form baseForm = null; // TODO: Initialize to an appropriate value
//            Control expected = null; // TODO: Initialize to an appropriate value
//            Control actual;
//            actual = Windows.Forms.Utils.GetControl(path, baseForm);
//            Assert.Equal(expected, actual);
//        }

//        /// <summary>
//        ///A test for Utils Constructor
//        ///</summary>
//        [Fact]
//        //[DeploymentItem("ICSSoft.STORMNET.Windows.Forms.dll")]
//        public void UtilsConstructorTest()
//        {
//            // Creation of the private accessor for 'Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly' failed
//            Assert.Inconclusive("Creation of the private accessor for \'Microsoft.VisualStudio.TestTools.TypesAndSy" +
//                    "mbols.Assembly\' failed");
//        }
//    }
//}

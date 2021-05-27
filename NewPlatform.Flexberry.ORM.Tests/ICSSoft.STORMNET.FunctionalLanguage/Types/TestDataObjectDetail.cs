namespace NewPlatform.Flexberry.ORM.Tests
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FileType;

    [View("D", new[]
    {
        "Name",
        "Value",
        "File",
        "TestDataObject",
    })]
    public class TestDataObjectDetail : DataObject, IName
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public File File { get; set; }

        [Agregator]
        public TestDataObject TestDataObject { get; set; }

        /// <summary>
        ///     Class views container.
        /// </summary>
        public class Views
        {
            /// <summary>
            ///     "D" view.
            /// </summary>
            public static View D => Information.GetView("D", typeof(TestDataObjectDetail));
        }
    }
}

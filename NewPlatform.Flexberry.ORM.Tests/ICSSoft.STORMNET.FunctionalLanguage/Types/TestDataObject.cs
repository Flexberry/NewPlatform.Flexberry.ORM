namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.UserDataTypes;

    [View("E", new[]
    {
        "Name",
        "BirthDate",
        "Height",
        "Weight",
        "Photo",
        "Hierarchy",
    })]
    public class TestDataObject : DataObject, IName
    {
        public string Name { get; set; }

        public string NickName { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime DeathDate { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public WebFile Photo { get; set; }

        public TestDataObject Hierarchy { get; set; }

        /// <summary>
        ///     Class views container.
        /// </summary>
        public class Views
        {
            /// <summary>
            ///     "E" view.
            /// </summary>
            public static View E => Information.GetView("E", typeof(TestDataObject));
        }
    }
}

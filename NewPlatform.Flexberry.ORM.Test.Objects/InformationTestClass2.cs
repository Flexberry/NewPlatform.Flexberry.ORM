﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Xml;
    using ICSSoft.STORMNET;
    
    
    // *** Start programmer edit section *** (Using statements)

    // *** End programmer edit section *** (Using statements)


    /// <summary>
    /// InformationTestClass2.
    /// </summary>
    // *** Start programmer edit section *** (InformationTestClass2 CustomAttributes)

    // *** End programmer edit section *** (InformationTestClass2 CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class InformationTestClass2 : ICSSoft.STORMNET.DataObject
    {
        
        private string fStringPropertyForInformationTestClass2;
        
        private NewPlatform.Flexberry.ORM.Tests.InformationTestClass fInformationTestClass;
        
        // *** Start programmer edit section *** (InformationTestClass2 CustomMembers)

        // *** End programmer edit section *** (InformationTestClass2 CustomMembers)

        
        /// <summary>
        /// StringPropertyForInformationTestClass2.
        /// </summary>
        // *** Start programmer edit section *** (InformationTestClass2.StringPropertyForInformationTestClass2 CustomAttributes)

        // *** End programmer edit section *** (InformationTestClass2.StringPropertyForInformationTestClass2 CustomAttributes)
        [PropertyStorage("StringPropertyForInfTestClass2")]
        [StrLen(255)]
        public virtual string StringPropertyForInformationTestClass2
        {
            get
            {
                // *** Start programmer edit section *** (InformationTestClass2.StringPropertyForInformationTestClass2 Get start)

                // *** End programmer edit section *** (InformationTestClass2.StringPropertyForInformationTestClass2 Get start)
                string result = this.fStringPropertyForInformationTestClass2;
                // *** Start programmer edit section *** (InformationTestClass2.StringPropertyForInformationTestClass2 Get end)

                // *** End programmer edit section *** (InformationTestClass2.StringPropertyForInformationTestClass2 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (InformationTestClass2.StringPropertyForInformationTestClass2 Set start)

                // *** End programmer edit section *** (InformationTestClass2.StringPropertyForInformationTestClass2 Set start)
                this.fStringPropertyForInformationTestClass2 = value;
                // *** Start programmer edit section *** (InformationTestClass2.StringPropertyForInformationTestClass2 Set end)

                // *** End programmer edit section *** (InformationTestClass2.StringPropertyForInformationTestClass2 Set end)
            }
        }
        
        /// <summary>
        /// InformationTestClass2.
        /// </summary>
        // *** Start programmer edit section *** (InformationTestClass2.InformationTestClass CustomAttributes)

        // *** End programmer edit section *** (InformationTestClass2.InformationTestClass CustomAttributes)
        [PropertyStorage("InformationTestClass")]
        [TypeUsage(new string[] {
                "NewPlatform.Flexberry.ORM.Tests.InformationTestClass",
                "NewPlatform.Flexberry.ORM.Tests.InformationTestClassChild"})]
        [NotNull()]
        public virtual NewPlatform.Flexberry.ORM.Tests.InformationTestClass InformationTestClass
        {
            get
            {
                // *** Start programmer edit section *** (InformationTestClass2.InformationTestClass Get start)

                // *** End programmer edit section *** (InformationTestClass2.InformationTestClass Get start)
                NewPlatform.Flexberry.ORM.Tests.InformationTestClass result = this.fInformationTestClass;
                // *** Start programmer edit section *** (InformationTestClass2.InformationTestClass Get end)

                // *** End programmer edit section *** (InformationTestClass2.InformationTestClass Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (InformationTestClass2.InformationTestClass Set start)

                // *** End programmer edit section *** (InformationTestClass2.InformationTestClass Set start)
                this.fInformationTestClass = value;
                // *** Start programmer edit section *** (InformationTestClass2.InformationTestClass Set end)

                // *** End programmer edit section *** (InformationTestClass2.InformationTestClass Set end)
            }
        }
    }
}

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
    /// InformationTestClass3.
    /// </summary>
    // *** Start programmer edit section *** (InformationTestClass3 CustomAttributes)

    // *** End programmer edit section *** (InformationTestClass3 CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class InformationTestClass3 : ICSSoft.STORMNET.DataObject
    {
        
        private string fStringPropertyForInformationTestClass3;
        
        private NewPlatform.Flexberry.ORM.Tests.InformationTestClass2 fInformationTestClass2;
        
        // *** Start programmer edit section *** (InformationTestClass3 CustomMembers)

        // *** End programmer edit section *** (InformationTestClass3 CustomMembers)

        
        /// <summary>
        /// StringPropertyForInformationTestClass3.
        /// </summary>
        // *** Start programmer edit section *** (InformationTestClass3.StringPropertyForInformationTestClass3 CustomAttributes)

        // *** End programmer edit section *** (InformationTestClass3.StringPropertyForInformationTestClass3 CustomAttributes)
        [PropertyStorage("StringPropForInfTestClass3")]
        [StrLen(255)]
        public virtual string StringPropertyForInformationTestClass3
        {
            get
            {
                // *** Start programmer edit section *** (InformationTestClass3.StringPropertyForInformationTestClass3 Get start)

                // *** End programmer edit section *** (InformationTestClass3.StringPropertyForInformationTestClass3 Get start)
                string result = this.fStringPropertyForInformationTestClass3;
                // *** Start programmer edit section *** (InformationTestClass3.StringPropertyForInformationTestClass3 Get end)

                // *** End programmer edit section *** (InformationTestClass3.StringPropertyForInformationTestClass3 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (InformationTestClass3.StringPropertyForInformationTestClass3 Set start)

                // *** End programmer edit section *** (InformationTestClass3.StringPropertyForInformationTestClass3 Set start)
                this.fStringPropertyForInformationTestClass3 = value;
                // *** Start programmer edit section *** (InformationTestClass3.StringPropertyForInformationTestClass3 Set end)

                // *** End programmer edit section *** (InformationTestClass3.StringPropertyForInformationTestClass3 Set end)
            }
        }
        
        /// <summary>
        /// InformationTestClass3.
        /// </summary>
        // *** Start programmer edit section *** (InformationTestClass3.InformationTestClass2 CustomAttributes)

        // *** End programmer edit section *** (InformationTestClass3.InformationTestClass2 CustomAttributes)
        [PropertyStorage(new string[] {
                "InformationTestClass2"})]
        [NotNull()]
        public virtual NewPlatform.Flexberry.ORM.Tests.InformationTestClass2 InformationTestClass2
        {
            get
            {
                // *** Start programmer edit section *** (InformationTestClass3.InformationTestClass2 Get start)

                // *** End programmer edit section *** (InformationTestClass3.InformationTestClass2 Get start)
                NewPlatform.Flexberry.ORM.Tests.InformationTestClass2 result = this.fInformationTestClass2;
                // *** Start programmer edit section *** (InformationTestClass3.InformationTestClass2 Get end)

                // *** End programmer edit section *** (InformationTestClass3.InformationTestClass2 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (InformationTestClass3.InformationTestClass2 Set start)

                // *** End programmer edit section *** (InformationTestClass3.InformationTestClass2 Set start)
                this.fInformationTestClass2 = value;
                // *** Start programmer edit section *** (InformationTestClass3.InformationTestClass2 Set end)

                // *** End programmer edit section *** (InformationTestClass3.InformationTestClass2 Set end)
            }
        }
    }
}

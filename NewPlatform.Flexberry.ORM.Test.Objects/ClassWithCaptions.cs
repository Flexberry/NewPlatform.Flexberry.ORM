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
    /// ClassWithCaptions.
    /// </summary>
    // *** Start programmer edit section *** (ClassWithCaptions CustomAttributes)
    [InstanceCaptionProperty("Класс пятый для экземпляров")]
    // *** End programmer edit section *** (ClassWithCaptions CustomAttributes)
    [AutoAltered()]
    [Caption("Класс пятый")]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class ClassWithCaptions : ICSSoft.STORMNET.DataObject
    {
        
        private NewPlatform.Flexberry.ORM.Tests.InformationTestClass4 fInformationTestClass4;
        
        // *** Start programmer edit section *** (ClassWithCaptions CustomMembers)

        // *** End programmer edit section *** (ClassWithCaptions CustomMembers)

        
        /// <summary>
        /// ClassWithCaptions.
        /// </summary>
        // *** Start programmer edit section *** (ClassWithCaptions.InformationTestClass4 CustomAttributes)

        // *** End programmer edit section *** (ClassWithCaptions.InformationTestClass4 CustomAttributes)
        [PropertyStorage(new string[] {
                "InformationTestClass4"})]
        [NotNull()]
        public virtual NewPlatform.Flexberry.ORM.Tests.InformationTestClass4 InformationTestClass4
        {
            get
            {
                // *** Start programmer edit section *** (ClassWithCaptions.InformationTestClass4 Get start)

                // *** End programmer edit section *** (ClassWithCaptions.InformationTestClass4 Get start)
                NewPlatform.Flexberry.ORM.Tests.InformationTestClass4 result = this.fInformationTestClass4;
                // *** Start programmer edit section *** (ClassWithCaptions.InformationTestClass4 Get end)

                // *** End programmer edit section *** (ClassWithCaptions.InformationTestClass4 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (ClassWithCaptions.InformationTestClass4 Set start)

                // *** End programmer edit section *** (ClassWithCaptions.InformationTestClass4 Set start)
                this.fInformationTestClass4 = value;
                // *** Start programmer edit section *** (ClassWithCaptions.InformationTestClass4 Set end)

                // *** End programmer edit section *** (ClassWithCaptions.InformationTestClass4 Set end)
            }
        }
    }
}

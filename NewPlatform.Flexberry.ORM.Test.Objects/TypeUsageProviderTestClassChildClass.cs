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
    /// TypeUsageProviderTestClassChildClass.
    /// </summary>
    // *** Start programmer edit section *** (TypeUsageProviderTestClassChildClass CustomAttributes)

    // *** End programmer edit section *** (TypeUsageProviderTestClassChildClass CustomAttributes)
    [ClassStorage("TypeUsageProviderTestClassChil")]
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class TypeUsageProviderTestClassChildClass : NewPlatform.Flexberry.ORM.Tests.TypeUsageProviderTestClass
    {
        
        // *** Start programmer edit section *** (TypeUsageProviderTestClassChildClass CustomMembers)

        // *** End programmer edit section *** (TypeUsageProviderTestClassChildClass CustomMembers)

        
        /// <summary>
        /// SomeNotStoredObjectProperty.
        /// </summary>
        // *** Start programmer edit section *** (TypeUsageProviderTestClassChildClass.SomeNotStoredObjectProperty CustomAttributes)

        // *** End programmer edit section *** (TypeUsageProviderTestClassChildClass.SomeNotStoredObjectProperty CustomAttributes)
        [ICSSoft.STORMNET.NotStored()]
        public override object SomeNotStoredObjectProperty
        {
            get
            {
                // *** Start programmer edit section *** (TypeUsageProviderTestClassChildClass.SomeNotStoredObjectProperty Get start)

                // *** End programmer edit section *** (TypeUsageProviderTestClassChildClass.SomeNotStoredObjectProperty Get start)
                object result = base.SomeNotStoredObjectProperty;
                // *** Start programmer edit section *** (TypeUsageProviderTestClassChildClass.SomeNotStoredObjectProperty Get end)

                // *** End programmer edit section *** (TypeUsageProviderTestClassChildClass.SomeNotStoredObjectProperty Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TypeUsageProviderTestClassChildClass.SomeNotStoredObjectProperty Set start)

                // *** End programmer edit section *** (TypeUsageProviderTestClassChildClass.SomeNotStoredObjectProperty Set start)
                base.SomeNotStoredObjectProperty = value;
                // *** Start programmer edit section *** (TypeUsageProviderTestClassChildClass.SomeNotStoredObjectProperty Set end)

                // *** End programmer edit section *** (TypeUsageProviderTestClassChildClass.SomeNotStoredObjectProperty Set end)
            }
        }
    }
}
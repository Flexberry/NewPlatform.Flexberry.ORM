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
    /// Salad2.
    /// </summary>
    // *** Start programmer edit section *** (Salad2 CustomAttributes)

    // *** End programmer edit section *** (Salad2 CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("Salad2E", new string[] {
            "SaladName as \'Salad name\'",
            "Ingridient2 as \'Ingridient2\'",
            "Ingridient2.Name as \'Name\'",
            "Ingridient1 as \'Ingridient1\'",
            "Ingridient1.Name as \'Name\'"})]
    [View("Salad2L", new string[] {
            "SaladName as \'Salad name\'",
            "Ingridient2.Name as \'Name\'",
            "Ingridient1.Name as \'Name\'"})]
    public class Salad2 : ICSSoft.STORMNET.DataObject
    {
        
        private string fSaladName;
        
        private NewPlatform.Flexberry.ORM.Tests.Plant2 fIngridient1;
        
        private NewPlatform.Flexberry.ORM.Tests.Plant2 fIngridient2;
        
        // *** Start programmer edit section *** (Salad2 CustomMembers)

        // *** End programmer edit section *** (Salad2 CustomMembers)

        
        /// <summary>
        /// SaladName.
        /// </summary>
        // *** Start programmer edit section *** (Salad2.SaladName CustomAttributes)

        // *** End programmer edit section *** (Salad2.SaladName CustomAttributes)
        [StrLen(255)]
        public virtual string SaladName
        {
            get
            {
                // *** Start programmer edit section *** (Salad2.SaladName Get start)

                // *** End programmer edit section *** (Salad2.SaladName Get start)
                string result = this.fSaladName;
                // *** Start programmer edit section *** (Salad2.SaladName Get end)

                // *** End programmer edit section *** (Salad2.SaladName Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Salad2.SaladName Set start)

                // *** End programmer edit section *** (Salad2.SaladName Set start)
                this.fSaladName = value;
                // *** Start programmer edit section *** (Salad2.SaladName Set end)

                // *** End programmer edit section *** (Salad2.SaladName Set end)
            }
        }
        
        /// <summary>
        /// Salad2.
        /// </summary>
        // *** Start programmer edit section *** (Salad2.Ingridient1 CustomAttributes)

        // *** End programmer edit section *** (Salad2.Ingridient1 CustomAttributes)
        [PropertyStorage("Ingridient1")]
        [TypeUsage(new string[] {
                "NewPlatform.Flexberry.ORM.Tests.Cabbage2",
                "NewPlatform.Flexberry.ORM.Tests.Plant2"})]
        public virtual NewPlatform.Flexberry.ORM.Tests.Plant2 Ingridient1
        {
            get
            {
                // *** Start programmer edit section *** (Salad2.Ingridient1 Get start)

                // *** End programmer edit section *** (Salad2.Ingridient1 Get start)
                NewPlatform.Flexberry.ORM.Tests.Plant2 result = this.fIngridient1;
                // *** Start programmer edit section *** (Salad2.Ingridient1 Get end)

                // *** End programmer edit section *** (Salad2.Ingridient1 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Salad2.Ingridient1 Set start)

                // *** End programmer edit section *** (Salad2.Ingridient1 Set start)
                this.fIngridient1 = value;
                // *** Start programmer edit section *** (Salad2.Ingridient1 Set end)

                // *** End programmer edit section *** (Salad2.Ingridient1 Set end)
            }
        }
        
        /// <summary>
        /// Salad2.
        /// </summary>
        // *** Start programmer edit section *** (Salad2.Ingridient2 CustomAttributes)

        // *** End programmer edit section *** (Salad2.Ingridient2 CustomAttributes)
        [PropertyStorage("Ingridient2")]
        [TypeUsage(new string[] {
                "NewPlatform.Flexberry.ORM.Tests.Cabbage2",
                "NewPlatform.Flexberry.ORM.Tests.Plant2"})]
        [NotNull()]
        public virtual NewPlatform.Flexberry.ORM.Tests.Plant2 Ingridient2
        {
            get
            {
                // *** Start programmer edit section *** (Salad2.Ingridient2 Get start)

                // *** End programmer edit section *** (Salad2.Ingridient2 Get start)
                NewPlatform.Flexberry.ORM.Tests.Plant2 result = this.fIngridient2;
                // *** Start programmer edit section *** (Salad2.Ingridient2 Get end)

                // *** End programmer edit section *** (Salad2.Ingridient2 Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Salad2.Ingridient2 Set start)

                // *** End programmer edit section *** (Salad2.Ingridient2 Set start)
                this.fIngridient2 = value;
                // *** Start programmer edit section *** (Salad2.Ingridient2 Set end)

                // *** End programmer edit section *** (Salad2.Ingridient2 Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "Salad2E" view.
            /// </summary>
            public static ICSSoft.STORMNET.View Salad2E
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("Salad2E", typeof(NewPlatform.Flexberry.ORM.Tests.Salad2));
                }
            }
            
            /// <summary>
            /// "Salad2L" view.
            /// </summary>
            public static ICSSoft.STORMNET.View Salad2L
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("Salad2L", typeof(NewPlatform.Flexberry.ORM.Tests.Salad2));
                }
            }
        }
    }
}

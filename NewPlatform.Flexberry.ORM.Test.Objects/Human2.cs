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
    
    
    //  *** Start programmer edit section *** (Using statements)

    //  *** End programmer edit section *** (Using statements)


    /// <summary>
    /// Human2.
    /// </summary>
    //  *** Start programmer edit section *** (Human2 CustomAttributes)

    //  *** End programmer edit section *** (Human2 CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("Human2E", new string[] {
            "HumanName as \'Human name\'",
            "TodayHome as \'Today home\'",
            "TodayHome.XCoordinate as \'X coordinate\'"})]
    [View("Human2L", new string[] {
            "HumanName as \'Human name\'",
            "TodayHome.XCoordinate as \'X coordinate\'"})]
    public class Human2 : ICSSoft.STORMNET.DataObject
    {
        
        private string fHumanName;
        
        private NewPlatform.Flexberry.ORM.Tests.Territory2 fTodayHome;
        
        //  *** Start programmer edit section *** (Human2 CustomMembers)

        //  *** End programmer edit section *** (Human2 CustomMembers)

        
        /// <summary>
        /// HumanName.
        /// </summary>
        //  *** Start programmer edit section *** (Human2.HumanName CustomAttributes)

        //  *** End programmer edit section *** (Human2.HumanName CustomAttributes)
        [StrLen(255)]
        public virtual string HumanName
        {
            get
            {
                //  *** Start programmer edit section *** (Human2.HumanName Get start)

                //  *** End programmer edit section *** (Human2.HumanName Get start)
                string result = this.fHumanName;
                //  *** Start programmer edit section *** (Human2.HumanName Get end)

                //  *** End programmer edit section *** (Human2.HumanName Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Human2.HumanName Set start)

                //  *** End programmer edit section *** (Human2.HumanName Set start)
                this.fHumanName = value;
                //  *** Start programmer edit section *** (Human2.HumanName Set end)

                //  *** End programmer edit section *** (Human2.HumanName Set end)
            }
        }
        
        /// <summary>
        /// Human2.
        /// </summary>
        //  *** Start programmer edit section *** (Human2.TodayHome CustomAttributes)

        //  *** End programmer edit section *** (Human2.TodayHome CustomAttributes)
        [TypeUsage(new string[] {
                "NewPlatform.Flexberry.ORM.Tests.Country2",
                "NewPlatform.Flexberry.ORM.Tests.Territory2"})]
        public virtual NewPlatform.Flexberry.ORM.Tests.Territory2 TodayHome
        {
            get
            {
                //  *** Start programmer edit section *** (Human2.TodayHome Get start)

                //  *** End programmer edit section *** (Human2.TodayHome Get start)
                NewPlatform.Flexberry.ORM.Tests.Territory2 result = this.fTodayHome;
                //  *** Start programmer edit section *** (Human2.TodayHome Get end)

                //  *** End programmer edit section *** (Human2.TodayHome Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Human2.TodayHome Set start)

                //  *** End programmer edit section *** (Human2.TodayHome Set start)
                this.fTodayHome = value;
                //  *** Start programmer edit section *** (Human2.TodayHome Set end)

                //  *** End programmer edit section *** (Human2.TodayHome Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "Human2E" view.
            /// </summary>
            public static ICSSoft.STORMNET.View Human2E
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("Human2E", typeof(NewPlatform.Flexberry.ORM.Tests.Human2));
                }
            }
            
            /// <summary>
            /// "Human2L" view.
            /// </summary>
            public static ICSSoft.STORMNET.View Human2L
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("Human2L", typeof(NewPlatform.Flexberry.ORM.Tests.Human2));
                }
            }
        }
    }
}

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
    /// Cabbage2.
    /// </summary>
    //  *** Start programmer edit section *** (Cabbage2 CustomAttributes)

    //  *** End programmer edit section *** (Cabbage2 CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("Cabbage2E", new string[] {
            "Type as \'Type\'",
            "Name as \'Name\'"})]
    [AssociatedDetailViewAttribute("Cabbage2E", "CabbageParts", "CabbagePart2E", true, "", "Cabbage parts", true, new string[] {
            ""})]
    [View("Cabbage2L", new string[] {
            "Type as \'Type\'",
            "Name as \'Name\'"})]
    public class Cabbage2 : NewPlatform.Flexberry.ORM.Tests.Plant2
    {
        
        private string fType;
        
        private NewPlatform.Flexberry.ORM.Tests.DetailArrayOfCabbagePart2 fCabbageParts;
        
        //  *** Start programmer edit section *** (Cabbage2 CustomMembers)

        //  *** End programmer edit section *** (Cabbage2 CustomMembers)

        
        /// <summary>
        /// Type.
        /// </summary>
        //  *** Start programmer edit section *** (Cabbage2.Type CustomAttributes)

        //  *** End programmer edit section *** (Cabbage2.Type CustomAttributes)
        [StrLen(255)]
        public virtual string Type
        {
            get
            {
                //  *** Start programmer edit section *** (Cabbage2.Type Get start)

                //  *** End programmer edit section *** (Cabbage2.Type Get start)
                string result = this.fType;
                //  *** Start programmer edit section *** (Cabbage2.Type Get end)

                //  *** End programmer edit section *** (Cabbage2.Type Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Cabbage2.Type Set start)

                //  *** End programmer edit section *** (Cabbage2.Type Set start)
                this.fType = value;
                //  *** Start programmer edit section *** (Cabbage2.Type Set end)

                //  *** End programmer edit section *** (Cabbage2.Type Set end)
            }
        }
        
        /// <summary>
        /// Cabbage2.
        /// </summary>
        //  *** Start programmer edit section *** (Cabbage2.CabbageParts CustomAttributes)

        //  *** End programmer edit section *** (Cabbage2.CabbageParts CustomAttributes)
        public virtual NewPlatform.Flexberry.ORM.Tests.DetailArrayOfCabbagePart2 CabbageParts
        {
            get
            {
                //  *** Start programmer edit section *** (Cabbage2.CabbageParts Get start)

                //  *** End programmer edit section *** (Cabbage2.CabbageParts Get start)
                if ((this.fCabbageParts == null))
                {
                    this.fCabbageParts = new NewPlatform.Flexberry.ORM.Tests.DetailArrayOfCabbagePart2(this);
                }
                NewPlatform.Flexberry.ORM.Tests.DetailArrayOfCabbagePart2 result = this.fCabbageParts;
                //  *** Start programmer edit section *** (Cabbage2.CabbageParts Get end)

                //  *** End programmer edit section *** (Cabbage2.CabbageParts Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Cabbage2.CabbageParts Set start)

                //  *** End programmer edit section *** (Cabbage2.CabbageParts Set start)
                this.fCabbageParts = value;
                //  *** Start programmer edit section *** (Cabbage2.CabbageParts Set end)

                //  *** End programmer edit section *** (Cabbage2.CabbageParts Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "Cabbage2E" view.
            /// </summary>
            public static ICSSoft.STORMNET.View Cabbage2E
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("Cabbage2E", typeof(NewPlatform.Flexberry.ORM.Tests.Cabbage2));
                }
            }
            
            /// <summary>
            /// "Cabbage2L" view.
            /// </summary>
            public static ICSSoft.STORMNET.View Cabbage2L
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("Cabbage2L", typeof(NewPlatform.Flexberry.ORM.Tests.Cabbage2));
                }
            }
        }
    }
}

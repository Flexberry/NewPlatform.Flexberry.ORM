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
    /// SomeDetailClass.
    /// </summary>
    //  *** Start programmer edit section *** (SomeDetailClass CustomAttributes)

    //  *** End programmer edit section *** (SomeDetailClass CustomAttributes)
    [AutoAltered()]
    [Caption("Class B")]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("ClassBE", new string[] {
            "FieldB as \'Field B\'",
            "SomeMasterClass",
            "SomeMasterClass.FieldA"})]
    [View("ClassBL", new string[] {
            "FieldB as \'Field B\'",
            "SomeMasterClass",
            "SomeMasterClass.FieldA"})]
    public class SomeDetailClass : ICSSoft.STORMNET.DataObject
    {
        
        private string fFieldB;
        
        private NewPlatform.Flexberry.ORM.Tests.SomeMasterClass fSomeMasterClass;
        
        //  *** Start programmer edit section *** (SomeDetailClass CustomMembers)

        //  *** End programmer edit section *** (SomeDetailClass CustomMembers)

        
        /// <summary>
        /// FieldB.
        /// </summary>
        //  *** Start programmer edit section *** (SomeDetailClass.FieldB CustomAttributes)

        //  *** End programmer edit section *** (SomeDetailClass.FieldB CustomAttributes)
        [StrLen(255)]
        public virtual string FieldB
        {
            get
            {
                //  *** Start programmer edit section *** (SomeDetailClass.FieldB Get start)

                //  *** End programmer edit section *** (SomeDetailClass.FieldB Get start)
                string result = this.fFieldB;
                //  *** Start programmer edit section *** (SomeDetailClass.FieldB Get end)

                //  *** End programmer edit section *** (SomeDetailClass.FieldB Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (SomeDetailClass.FieldB Set start)

                //  *** End programmer edit section *** (SomeDetailClass.FieldB Set start)
                this.fFieldB = value;
                //  *** Start programmer edit section *** (SomeDetailClass.FieldB Set end)

                //  *** End programmer edit section *** (SomeDetailClass.FieldB Set end)
            }
        }
        
        /// <summary>
        /// SomeDetailClass.
        /// </summary>
        //  *** Start programmer edit section *** (SomeDetailClass.SomeMasterClass CustomAttributes)

        //  *** End programmer edit section *** (SomeDetailClass.SomeMasterClass CustomAttributes)
        [PropertyStorage(new string[] {
                "ClassA"})]
        [NotNull()]
        public virtual NewPlatform.Flexberry.ORM.Tests.SomeMasterClass SomeMasterClass
        {
            get
            {
                //  *** Start programmer edit section *** (SomeDetailClass.SomeMasterClass Get start)

                //  *** End programmer edit section *** (SomeDetailClass.SomeMasterClass Get start)
                NewPlatform.Flexberry.ORM.Tests.SomeMasterClass result = this.fSomeMasterClass;
                //  *** Start programmer edit section *** (SomeDetailClass.SomeMasterClass Get end)

                //  *** End programmer edit section *** (SomeDetailClass.SomeMasterClass Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (SomeDetailClass.SomeMasterClass Set start)

                //  *** End programmer edit section *** (SomeDetailClass.SomeMasterClass Set start)
                this.fSomeMasterClass = value;
                //  *** Start programmer edit section *** (SomeDetailClass.SomeMasterClass Set end)

                //  *** End programmer edit section *** (SomeDetailClass.SomeMasterClass Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "ClassBE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View ClassBE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("ClassBE", typeof(NewPlatform.Flexberry.ORM.Tests.SomeDetailClass));
                }
            }
            
            /// <summary>
            /// "ClassBL" view.
            /// </summary>
            public static ICSSoft.STORMNET.View ClassBL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("ClassBL", typeof(NewPlatform.Flexberry.ORM.Tests.SomeDetailClass));
                }
            }
        }
    }
}

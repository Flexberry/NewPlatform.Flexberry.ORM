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
    /// ИсточникФинансирования.
    /// </summary>
    // *** Start programmer edit section *** (ИсточникФинансирования CustomAttributes)

    // *** End programmer edit section *** (ИсточникФинансирования CustomAttributes)
    [ClassStorage("ИсточникФинанс")]
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("ИсточникФинансированияE", new string[] {
            "НомерИсточникаФинансирования"})]
    [View("ИсточникФинансированияL", new string[] {
            "НомерИсточникаФинансирования"})]
    public class ИсточникФинансирования : ICSSoft.STORMNET.DataObject
    {
        
        private int fНомерИсточникаФинансирования;
        
        // *** Start programmer edit section *** (ИсточникФинансирования CustomMembers)

        // *** End programmer edit section *** (ИсточникФинансирования CustomMembers)

        
        /// <summary>
        /// НомерИсточникаФинансирования.
        /// </summary>
        // *** Start programmer edit section *** (ИсточникФинансирования.НомерИсточникаФинансирования CustomAttributes)

        // *** End programmer edit section *** (ИсточникФинансирования.НомерИсточникаФинансирования CustomAttributes)
        [PropertyStorage("НомИсточникаФин")]
        public virtual int НомерИсточникаФинансирования
        {
            get
            {
                // *** Start programmer edit section *** (ИсточникФинансирования.НомерИсточникаФинансирования Get start)

                // *** End programmer edit section *** (ИсточникФинансирования.НомерИсточникаФинансирования Get start)
                int result = this.fНомерИсточникаФинансирования;
                // *** Start programmer edit section *** (ИсточникФинансирования.НомерИсточникаФинансирования Get end)

                // *** End programmer edit section *** (ИсточникФинансирования.НомерИсточникаФинансирования Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (ИсточникФинансирования.НомерИсточникаФинансирования Set start)

                // *** End programmer edit section *** (ИсточникФинансирования.НомерИсточникаФинансирования Set start)
                this.fНомерИсточникаФинансирования = value;
                // *** Start programmer edit section *** (ИсточникФинансирования.НомерИсточникаФинансирования Set end)

                // *** End programmer edit section *** (ИсточникФинансирования.НомерИсточникаФинансирования Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "ИсточникФинансированияE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View ИсточникФинансированияE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("ИсточникФинансированияE", typeof(NewPlatform.Flexberry.ORM.Tests.ИсточникФинансирования));
                }
            }
            
            /// <summary>
            /// "ИсточникФинансированияL" view.
            /// </summary>
            public static ICSSoft.STORMNET.View ИсточникФинансированияL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("ИсточникФинансированияL", typeof(NewPlatform.Flexberry.ORM.Tests.ИсточникФинансирования));
                }
            }
        }
    }
}

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
    /// ЗначениеКритерия.
    /// </summary>
    // *** Start programmer edit section *** (ЗначениеКритерия CustomAttributes)

    // *** End programmer edit section *** (ЗначениеКритерия CustomAttributes)
    [ClassStorage("ЗначениеКритер")]
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("ЗначениеКритерияE", new string[] {
            "Значение",
            "СредняяОценкаЭксперта",
            "Критерий"})]
    public class ЗначениеКритерия : ICSSoft.STORMNET.DataObject
    {
        
        private string fЗначение;
        
        private double fСредняяОценкаЭксперта;
        
        private NewPlatform.Flexberry.ORM.Tests.КритерийОценки fКритерий;
        
        private NewPlatform.Flexberry.ORM.Tests.Идея fИдея;
        
        // *** Start programmer edit section *** (ЗначениеКритерия CustomMembers)

        // *** End programmer edit section *** (ЗначениеКритерия CustomMembers)

        
        /// <summary>
        /// Значение.
        /// </summary>
        // *** Start programmer edit section *** (ЗначениеКритерия.Значение CustomAttributes)

        // *** End programmer edit section *** (ЗначениеКритерия.Значение CustomAttributes)
        [StrLen(255)]
        public virtual string Значение
        {
            get
            {
                // *** Start programmer edit section *** (ЗначениеКритерия.Значение Get start)

                // *** End programmer edit section *** (ЗначениеКритерия.Значение Get start)
                string result = this.fЗначение;
                // *** Start programmer edit section *** (ЗначениеКритерия.Значение Get end)

                // *** End programmer edit section *** (ЗначениеКритерия.Значение Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (ЗначениеКритерия.Значение Set start)

                // *** End programmer edit section *** (ЗначениеКритерия.Значение Set start)
                this.fЗначение = value;
                // *** Start programmer edit section *** (ЗначениеКритерия.Значение Set end)

                // *** End programmer edit section *** (ЗначениеКритерия.Значение Set end)
            }
        }
        
        /// <summary>
        /// СредняяОценкаЭксперта.
        /// </summary>
        // *** Start programmer edit section *** (ЗначениеКритерия.СредняяОценкаЭксперта CustomAttributes)

        // *** End programmer edit section *** (ЗначениеКритерия.СредняяОценкаЭксперта CustomAttributes)
        [PropertyStorage("СредОценкаЭксп")]
        public virtual double СредняяОценкаЭксперта
        {
            get
            {
                // *** Start programmer edit section *** (ЗначениеКритерия.СредняяОценкаЭксперта Get start)

                // *** End programmer edit section *** (ЗначениеКритерия.СредняяОценкаЭксперта Get start)
                double result = this.fСредняяОценкаЭксперта;
                // *** Start programmer edit section *** (ЗначениеКритерия.СредняяОценкаЭксперта Get end)

                // *** End programmer edit section *** (ЗначениеКритерия.СредняяОценкаЭксперта Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (ЗначениеКритерия.СредняяОценкаЭксперта Set start)

                // *** End programmer edit section *** (ЗначениеКритерия.СредняяОценкаЭксперта Set start)
                this.fСредняяОценкаЭксперта = value;
                // *** Start programmer edit section *** (ЗначениеКритерия.СредняяОценкаЭксперта Set end)

                // *** End programmer edit section *** (ЗначениеКритерия.СредняяОценкаЭксперта Set end)
            }
        }
        
        /// <summary>
        /// ЗначениеКритерия.
        /// </summary>
        // *** Start programmer edit section *** (ЗначениеКритерия.Критерий CustomAttributes)

        // *** End programmer edit section *** (ЗначениеКритерия.Критерий CustomAttributes)
        [NotNull()]
        public virtual NewPlatform.Flexberry.ORM.Tests.КритерийОценки Критерий
        {
            get
            {
                // *** Start programmer edit section *** (ЗначениеКритерия.Критерий Get start)

                // *** End programmer edit section *** (ЗначениеКритерия.Критерий Get start)
                NewPlatform.Flexberry.ORM.Tests.КритерийОценки result = this.fКритерий;
                // *** Start programmer edit section *** (ЗначениеКритерия.Критерий Get end)

                // *** End programmer edit section *** (ЗначениеКритерия.Критерий Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (ЗначениеКритерия.Критерий Set start)

                // *** End programmer edit section *** (ЗначениеКритерия.Критерий Set start)
                this.fКритерий = value;
                // *** Start programmer edit section *** (ЗначениеКритерия.Критерий Set end)

                // *** End programmer edit section *** (ЗначениеКритерия.Критерий Set end)
            }
        }
        
        /// <summary>
        /// мастеровая ссылка на шапку NewPlatform.Flexberry.ORM.Tests.Идея.
        /// </summary>
        // *** Start programmer edit section *** (ЗначениеКритерия.Идея CustomAttributes)

        // *** End programmer edit section *** (ЗначениеКритерия.Идея CustomAttributes)
        [Agregator()]
        [NotNull()]
        public virtual NewPlatform.Flexberry.ORM.Tests.Идея Идея
        {
            get
            {
                // *** Start programmer edit section *** (ЗначениеКритерия.Идея Get start)

                // *** End programmer edit section *** (ЗначениеКритерия.Идея Get start)
                NewPlatform.Flexberry.ORM.Tests.Идея result = this.fИдея;
                // *** Start programmer edit section *** (ЗначениеКритерия.Идея Get end)

                // *** End programmer edit section *** (ЗначениеКритерия.Идея Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (ЗначениеКритерия.Идея Set start)

                // *** End programmer edit section *** (ЗначениеКритерия.Идея Set start)
                this.fИдея = value;
                // *** Start programmer edit section *** (ЗначениеКритерия.Идея Set end)

                // *** End programmer edit section *** (ЗначениеКритерия.Идея Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "ЗначениеКритерияE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View ЗначениеКритерияE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("ЗначениеКритерияE", typeof(NewPlatform.Flexberry.ORM.Tests.ЗначениеКритерия));
                }
            }
        }
    }
    
    /// <summary>
    /// Detail array of ЗначениеКритерия.
    /// </summary>
    // *** Start programmer edit section *** (DetailArrayDetailArrayOfЗначениеКритерия CustomAttributes)

    // *** End programmer edit section *** (DetailArrayDetailArrayOfЗначениеКритерия CustomAttributes)
    public class DetailArrayOfЗначениеКритерия : ICSSoft.STORMNET.DetailArray
    {
        
        // *** Start programmer edit section *** (NewPlatform.Flexberry.ORM.Tests.DetailArrayOfЗначениеКритерия members)

        // *** End programmer edit section *** (NewPlatform.Flexberry.ORM.Tests.DetailArrayOfЗначениеКритерия members)

        
        /// <summary>
        /// Construct detail array.
        /// </summary>
        /// <summary>
        /// Returns object with type ЗначениеКритерия by index.
        /// </summary>
        /// <summary>
        /// Adds object with type ЗначениеКритерия.
        /// </summary>
        public DetailArrayOfЗначениеКритерия(NewPlatform.Flexberry.ORM.Tests.Идея fИдея) : 
                base(typeof(ЗначениеКритерия), ((ICSSoft.STORMNET.DataObject)(fИдея)))
        {
        }
        
        public NewPlatform.Flexberry.ORM.Tests.ЗначениеКритерия this[int index]
        {
            get
            {
                return ((NewPlatform.Flexberry.ORM.Tests.ЗначениеКритерия)(this.ItemByIndex(index)));
            }
        }
        
        public virtual void Add(NewPlatform.Flexberry.ORM.Tests.ЗначениеКритерия dataobject)
        {
            this.AddObject(((ICSSoft.STORMNET.DataObject)(dataobject)));
        }
    }
}

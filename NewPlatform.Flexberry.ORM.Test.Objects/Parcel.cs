﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
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
    /// Parcel.
    /// </summary>
    // *** Start programmer edit section *** (Parcel CustomAttributes)

    // *** End programmer edit section *** (Parcel CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class Parcel : ICSSoft.STORMNET.DataObject
    {
        
        private string fAddress;
        
        private double fWeight;
        
        private NewPlatform.Flexberry.ORM.Tests.Homer fDeliveredByHomer;
        
        private NewPlatform.Flexberry.ORM.Tests.Mailman fDeliveredByMailman;
        
        // *** Start programmer edit section *** (Parcel CustomMembers)

        // *** End programmer edit section *** (Parcel CustomMembers)

        
        /// <summary>
        /// Address.
        /// </summary>
        // *** Start programmer edit section *** (Parcel.Address CustomAttributes)

        // *** End programmer edit section *** (Parcel.Address CustomAttributes)
        [StrLen(255)]
        public virtual string Address
        {
            get
            {
                // *** Start programmer edit section *** (Parcel.Address Get start)

                // *** End programmer edit section *** (Parcel.Address Get start)
                string result = this.fAddress;
                // *** Start programmer edit section *** (Parcel.Address Get end)

                // *** End programmer edit section *** (Parcel.Address Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Parcel.Address Set start)

                // *** End programmer edit section *** (Parcel.Address Set start)
                this.fAddress = value;
                // *** Start programmer edit section *** (Parcel.Address Set end)

                // *** End programmer edit section *** (Parcel.Address Set end)
            }
        }
        
        /// <summary>
        /// Weight.
        /// </summary>
        // *** Start programmer edit section *** (Parcel.Weight CustomAttributes)

        // *** End programmer edit section *** (Parcel.Weight CustomAttributes)
        public virtual double Weight
        {
            get
            {
                // *** Start programmer edit section *** (Parcel.Weight Get start)

                // *** End programmer edit section *** (Parcel.Weight Get start)
                double result = this.fWeight;
                // *** Start programmer edit section *** (Parcel.Weight Get end)

                // *** End programmer edit section *** (Parcel.Weight Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Parcel.Weight Set start)

                // *** End programmer edit section *** (Parcel.Weight Set start)
                this.fWeight = value;
                // *** Start programmer edit section *** (Parcel.Weight Set end)

                // *** End programmer edit section *** (Parcel.Weight Set end)
            }
        }
        
        /// <summary>
        /// Parcel.
        /// </summary>
        // *** Start programmer edit section *** (Parcel.DeliveredByHomer CustomAttributes)

        // *** End programmer edit section *** (Parcel.DeliveredByHomer CustomAttributes)
        [PropertyStorage(new string[] {
                "DeliveredByHomer"})]
        public virtual NewPlatform.Flexberry.ORM.Tests.Homer DeliveredByHomer
        {
            get
            {
                // *** Start programmer edit section *** (Parcel.DeliveredByHomer Get start)

                // *** End programmer edit section *** (Parcel.DeliveredByHomer Get start)
                NewPlatform.Flexberry.ORM.Tests.Homer result = this.fDeliveredByHomer;
                // *** Start programmer edit section *** (Parcel.DeliveredByHomer Get end)

                // *** End programmer edit section *** (Parcel.DeliveredByHomer Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Parcel.DeliveredByHomer Set start)

                // *** End programmer edit section *** (Parcel.DeliveredByHomer Set start)
                this.fDeliveredByHomer = value;
                // *** Start programmer edit section *** (Parcel.DeliveredByHomer Set end)

                // *** End programmer edit section *** (Parcel.DeliveredByHomer Set end)
            }
        }
        
        /// <summary>
        /// Parcel.
        /// </summary>
        // *** Start programmer edit section *** (Parcel.DeliveredByMailman CustomAttributes)

        // *** End programmer edit section *** (Parcel.DeliveredByMailman CustomAttributes)
        [PropertyStorage(new string[] {
                "DeliveredByMailman"})]
        public virtual NewPlatform.Flexberry.ORM.Tests.Mailman DeliveredByMailman
        {
            get
            {
                // *** Start programmer edit section *** (Parcel.DeliveredByMailman Get start)

                // *** End programmer edit section *** (Parcel.DeliveredByMailman Get start)
                NewPlatform.Flexberry.ORM.Tests.Mailman result = this.fDeliveredByMailman;
                // *** Start programmer edit section *** (Parcel.DeliveredByMailman Get end)

                // *** End programmer edit section *** (Parcel.DeliveredByMailman Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Parcel.DeliveredByMailman Set start)

                // *** End programmer edit section *** (Parcel.DeliveredByMailman Set start)
                this.fDeliveredByMailman = value;
                // *** Start programmer edit section *** (Parcel.DeliveredByMailman Set end)

                // *** End programmer edit section *** (Parcel.DeliveredByMailman Set end)
            }
        }
    }
}

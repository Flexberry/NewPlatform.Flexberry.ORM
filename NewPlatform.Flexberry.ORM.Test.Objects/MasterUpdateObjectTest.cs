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
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET;
    
    
    // *** Start programmer edit section *** (Using statements)

    // *** End programmer edit section *** (Using statements)


    /// <summary>
    /// MasterUpdateObjectTest.
    /// </summary>
    // *** Start programmer edit section *** (MasterUpdateObjectTest CustomAttributes)

    // *** End programmer edit section *** (MasterUpdateObjectTest CustomAttributes)
    [BusinessServer("NewPlatform.Flexberry.ORM.Tests.UpdateObjectTestBS, NewPlatform.Flexberry.ORM.Tests.BusinessServers", ICSSoft.STORMNET.Business.DataServiceObjectEvents.OnAllEvents)]
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("MasterUpdateObjectTestE", new string[] {
            "MasterName",
            "AggregatorUpdateObjectTest",
            "Detail",
            "Detail.DetailName",
            "Detail.Master"}, Hidden=new string[] {
            "AggregatorUpdateObjectTest",
            "Detail",
            "Detail.DetailName",
            "Detail.Master"})]
    public class MasterUpdateObjectTest : ICSSoft.STORMNET.DataObject
    {
        
        private string fMasterName;
        
        private NewPlatform.Flexberry.ORM.Tests.DetailUpdateObjectTest fDetail;
        
        private NewPlatform.Flexberry.ORM.Tests.AggregatorUpdateObjectTest fAggregatorUpdateObjectTest;
        
        // *** Start programmer edit section *** (MasterUpdateObjectTest CustomMembers)

        // *** End programmer edit section *** (MasterUpdateObjectTest CustomMembers)

        
        /// <summary>
        /// MasterName.
        /// </summary>
        // *** Start programmer edit section *** (MasterUpdateObjectTest.MasterName CustomAttributes)

        // *** End programmer edit section *** (MasterUpdateObjectTest.MasterName CustomAttributes)
        [StrLen(255)]
        public virtual string MasterName
        {
            get
            {
                // *** Start programmer edit section *** (MasterUpdateObjectTest.MasterName Get start)

                // *** End programmer edit section *** (MasterUpdateObjectTest.MasterName Get start)
                string result = this.fMasterName;
                // *** Start programmer edit section *** (MasterUpdateObjectTest.MasterName Get end)

                // *** End programmer edit section *** (MasterUpdateObjectTest.MasterName Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (MasterUpdateObjectTest.MasterName Set start)

                // *** End programmer edit section *** (MasterUpdateObjectTest.MasterName Set start)
                this.fMasterName = value;
                // *** Start programmer edit section *** (MasterUpdateObjectTest.MasterName Set end)

                // *** End programmer edit section *** (MasterUpdateObjectTest.MasterName Set end)
            }
        }
        
        /// <summary>
        /// MasterUpdateObjectTest.
        /// </summary>
        // *** Start programmer edit section *** (MasterUpdateObjectTest.Detail CustomAttributes)

        // *** End programmer edit section *** (MasterUpdateObjectTest.Detail CustomAttributes)
        [PropertyStorage(new string[] {
                "Detail"})]
        public virtual NewPlatform.Flexberry.ORM.Tests.DetailUpdateObjectTest Detail
        {
            get
            {
                // *** Start programmer edit section *** (MasterUpdateObjectTest.Detail Get start)

                // *** End programmer edit section *** (MasterUpdateObjectTest.Detail Get start)
                NewPlatform.Flexberry.ORM.Tests.DetailUpdateObjectTest result = this.fDetail;
                // *** Start programmer edit section *** (MasterUpdateObjectTest.Detail Get end)

                // *** End programmer edit section *** (MasterUpdateObjectTest.Detail Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (MasterUpdateObjectTest.Detail Set start)

                // *** End programmer edit section *** (MasterUpdateObjectTest.Detail Set start)
                this.fDetail = value;
                // *** Start programmer edit section *** (MasterUpdateObjectTest.Detail Set end)

                // *** End programmer edit section *** (MasterUpdateObjectTest.Detail Set end)
            }
        }
        
        /// <summary>
        /// мастеровая ссылка на шапку NewPlatform.Flexberry.ORM.Tests.AggregatorUpdateObjectTest.
        /// </summary>
        // *** Start programmer edit section *** (MasterUpdateObjectTest.AggregatorUpdateObjectTest CustomAttributes)

        // *** End programmer edit section *** (MasterUpdateObjectTest.AggregatorUpdateObjectTest CustomAttributes)
        [Agregator()]
        [NotNull()]
        [PropertyStorage(new string[] {
                "AggregatorUpdateObjectTest"})]
        public virtual NewPlatform.Flexberry.ORM.Tests.AggregatorUpdateObjectTest AggregatorUpdateObjectTest
        {
            get
            {
                // *** Start programmer edit section *** (MasterUpdateObjectTest.AggregatorUpdateObjectTest Get start)

                // *** End programmer edit section *** (MasterUpdateObjectTest.AggregatorUpdateObjectTest Get start)
                NewPlatform.Flexberry.ORM.Tests.AggregatorUpdateObjectTest result = this.fAggregatorUpdateObjectTest;
                // *** Start programmer edit section *** (MasterUpdateObjectTest.AggregatorUpdateObjectTest Get end)

                // *** End programmer edit section *** (MasterUpdateObjectTest.AggregatorUpdateObjectTest Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (MasterUpdateObjectTest.AggregatorUpdateObjectTest Set start)

                // *** End programmer edit section *** (MasterUpdateObjectTest.AggregatorUpdateObjectTest Set start)
                this.fAggregatorUpdateObjectTest = value;
                // *** Start programmer edit section *** (MasterUpdateObjectTest.AggregatorUpdateObjectTest Set end)

                // *** End programmer edit section *** (MasterUpdateObjectTest.AggregatorUpdateObjectTest Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "MasterUpdateObjectTestE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View MasterUpdateObjectTestE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("MasterUpdateObjectTestE", typeof(NewPlatform.Flexberry.ORM.Tests.MasterUpdateObjectTest));
                }
            }
        }
    }
    
    /// <summary>
    /// Detail array of MasterUpdateObjectTest.
    /// </summary>
    // *** Start programmer edit section *** (DetailArrayDetailArrayOfMasterUpdateObjectTest CustomAttributes)

    // *** End programmer edit section *** (DetailArrayDetailArrayOfMasterUpdateObjectTest CustomAttributes)
    public class DetailArrayOfMasterUpdateObjectTest : ICSSoft.STORMNET.DetailArray
    {
        
        // *** Start programmer edit section *** (NewPlatform.Flexberry.ORM.Tests.DetailArrayOfMasterUpdateObjectTest members)

        // *** End programmer edit section *** (NewPlatform.Flexberry.ORM.Tests.DetailArrayOfMasterUpdateObjectTest members)

        
        /// <summary>
        /// Construct detail array.
        /// </summary>
        /// <summary>
        /// Returns object with type MasterUpdateObjectTest by index.
        /// </summary>
        /// <summary>
        /// Adds object with type MasterUpdateObjectTest.
        /// </summary>
        public DetailArrayOfMasterUpdateObjectTest(NewPlatform.Flexberry.ORM.Tests.AggregatorUpdateObjectTest fAggregatorUpdateObjectTest) : 
                base(typeof(MasterUpdateObjectTest), ((ICSSoft.STORMNET.DataObject)(fAggregatorUpdateObjectTest)))
        {
        }
        
        public NewPlatform.Flexberry.ORM.Tests.MasterUpdateObjectTest this[int index]
        {
            get
            {
                return ((NewPlatform.Flexberry.ORM.Tests.MasterUpdateObjectTest)(this.ItemByIndex(index)));
            }
        }
        
        public virtual void Add(NewPlatform.Flexberry.ORM.Tests.MasterUpdateObjectTest dataobject)
        {
            this.AddObject(((ICSSoft.STORMNET.DataObject)(dataobject)));
        }
    }
}

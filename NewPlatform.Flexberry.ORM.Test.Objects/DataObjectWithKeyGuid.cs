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
    /// DataObjectWithKeyGuid.
    /// </summary>
    //  *** Start programmer edit section *** (DataObjectWithKeyGuid CustomAttributes)
    [Serializable]
    //  *** End programmer edit section *** (DataObjectWithKeyGuid CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class DataObjectWithKeyGuid : ICSSoft.STORMNET.DataObject
    {
        
        private ICSSoft.STORMNET.KeyGen.KeyGuid fLinkToMaster1;
        
        private ICSSoft.STORMNET.KeyGen.KeyGuid fLinkToMaster2;
        
        //  *** Start programmer edit section *** (DataObjectWithKeyGuid CustomMembers)

        //  *** End programmer edit section *** (DataObjectWithKeyGuid CustomMembers)

        
        /// <summary>
        /// LinkToMaster1.
        /// </summary>
        //  *** Start programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster1 CustomAttributes)

        //  *** End programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster1 CustomAttributes)
        public virtual ICSSoft.STORMNET.KeyGen.KeyGuid LinkToMaster1
        {
            get
            {
                //  *** Start programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster1 Get start)

                //  *** End programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster1 Get start)
                ICSSoft.STORMNET.KeyGen.KeyGuid result = this.fLinkToMaster1;
                //  *** Start programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster1 Get end)

                //  *** End programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster1 Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster1 Set start)

                //  *** End programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster1 Set start)
                this.fLinkToMaster1 = value;
                //  *** Start programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster1 Set end)

                //  *** End programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster1 Set end)
            }
        }
        
        /// <summary>
        /// LinkToMaster2.
        /// </summary>
        //  *** Start programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster2 CustomAttributes)

        //  *** End programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster2 CustomAttributes)
        public virtual ICSSoft.STORMNET.KeyGen.KeyGuid LinkToMaster2
        {
            get
            {
                //  *** Start programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster2 Get start)

                //  *** End programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster2 Get start)
                ICSSoft.STORMNET.KeyGen.KeyGuid result = this.fLinkToMaster2;
                //  *** Start programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster2 Get end)

                //  *** End programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster2 Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster2 Set start)

                //  *** End programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster2 Set start)
                this.fLinkToMaster2 = value;
                //  *** Start programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster2 Set end)

                //  *** End programmer edit section *** (DataObjectWithKeyGuid.LinkToMaster2 Set end)
            }
        }
    }
}

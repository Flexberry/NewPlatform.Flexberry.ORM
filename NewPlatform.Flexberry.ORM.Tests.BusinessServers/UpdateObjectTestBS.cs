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
    
    
    //  *** Start programmer edit section *** (Using statements)
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    //  *** End programmer edit section *** (Using statements)


    /// <summary>
    /// UpdateObjectTestBS.
    /// </summary>
    //  *** Start programmer edit section *** (UpdateObjectTestBS CustomAttributes)

    //  *** End programmer edit section *** (UpdateObjectTestBS CustomAttributes)
    [ICSSoft.STORMNET.AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class UpdateObjectTestBS : ICSSoft.STORMNET.Business.BusinessServer
    {
        
        //  *** Start programmer edit section *** (UpdateObjectTestBS CustomMembers)

        //  *** End programmer edit section *** (UpdateObjectTestBS CustomMembers)

        
        //  *** Start programmer edit section *** (OnUpdateAggregatorUpdateObjectTest CustomAttributes)

        //  *** End programmer edit section *** (OnUpdateAggregatorUpdateObjectTest CustomAttributes)
        public virtual ICSSoft.STORMNET.DataObject[] OnUpdateAggregatorUpdateObjectTest(NewPlatform.Flexberry.ORM.Tests.AggregatorUpdateObjectTest UpdatedObject)
        {
            //  *** Start programmer edit section *** (OnUpdateAggregatorUpdateObjectTest)
            if (UpdatedObject.GetStatus() == ObjectStatus.Deleted)
            {
                if (UpdatedObject.Detail != null)
                {
                    UpdatedObject.Detail = null;
                }
            }

            return new ICSSoft.STORMNET.DataObject[0];
            //  *** End programmer edit section *** (OnUpdateAggregatorUpdateObjectTest)
        }
        
        //  *** Start programmer edit section *** (OnUpdateMasterUpdateObjectTest CustomAttributes)

        //  *** End programmer edit section *** (OnUpdateMasterUpdateObjectTest CustomAttributes)
        public virtual ICSSoft.STORMNET.DataObject[] OnUpdateMasterUpdateObjectTest(NewPlatform.Flexberry.ORM.Tests.MasterUpdateObjectTest UpdatedObject)
        {
            //  *** Start programmer edit section *** (OnUpdateMasterUpdateObjectTest)
            if (UpdatedObject.GetStatus() == ObjectStatus.Deleted)
            {
                if (UpdatedObject.Detail != null)
                {
                    UpdatedObject.Detail = null;
                }
            }

            return new ICSSoft.STORMNET.DataObject[0];
            //  *** End programmer edit section *** (OnUpdateMasterUpdateObjectTest)
        }
        
        //  *** Start programmer edit section *** (OnUpdateDetailUpdateObjectTest CustomAttributes)

        //  *** End programmer edit section *** (OnUpdateDetailUpdateObjectTest CustomAttributes)
        public virtual ICSSoft.STORMNET.DataObject[] OnUpdateDetailUpdateObjectTest(NewPlatform.Flexberry.ORM.Tests.DetailUpdateObjectTest UpdatedObject)
        {
            //  *** Start programmer edit section *** (OnUpdateDetailUpdateObjectTest)
            if (UpdatedObject.GetStatus() == ObjectStatus.Deleted)
            {
                if (UpdatedObject.Master != null)
                {
                    UpdatedObject.Master = null;
                }
            }

            return new ICSSoft.STORMNET.DataObject[0];
            //  *** End programmer edit section *** (OnUpdateDetailUpdateObjectTest)
        }
    }
}

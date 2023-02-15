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
    
    
    // *** Start programmer edit section *** (Using statements)
    using System.Linq;
    using System.Collections.Generic;

    using ICSSoft.STORMNET;
    // *** End programmer edit section *** (Using statements)


    /// <summary>
    /// МедведьБС.
    /// </summary>
    // *** Start programmer edit section *** (МедведьБС CustomAttributes)

    // *** End programmer edit section *** (МедведьБС CustomAttributes)
    [ICSSoft.STORMNET.AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class МедведьБС : ICSSoft.STORMNET.Business.BusinessServer
    {
        
        // *** Start programmer edit section *** (МедведьБС CustomMembers)

        // *** End programmer edit section *** (МедведьБС CustomMembers)

        
        // *** Start programmer edit section *** (OnUpdateМедведь CustomAttributes)

        // *** End programmer edit section *** (OnUpdateМедведь CustomAttributes)
        public virtual ICSSoft.STORMNET.DataObject[] OnUpdateМедведь(NewPlatform.Flexberry.ORM.Tests.Медведь UpdatedObject)
        {
            // *** Start programmer edit section *** (OnUpdateМедведь)
            var updatedObjects = new List<DataObject>();
            IEnumerable<Берлога> берлоги = UpdatedObject.Берлога.GetAllObjects().Cast<Берлога>();

            var новаяБерлога = берлоги.FirstOrDefault(б => б.GetStatus() == ObjectStatus.Created);
            if (новаяБерлога != null)
            {
                foreach (Берлога берлога in UpdatedObject.Берлога)
                {
                    if (берлога != новаяБерлога)
                    {
                        берлога.Заброшена = true;
                        updatedObjects.Add(берлога);
                    }
                }
            }

            var последняяБерлога = берлоги.FirstOrDefault(б => б.GetStatus() == ObjectStatus.Altered);
            if (последняяБерлога != null)
            {
                foreach (Берлога берлога in UpdatedObject.Берлога)
                {
                    if (берлога != последняяБерлога)
                    {
                        берлога.Заброшена = true;
                        updatedObjects.Add(берлога);
                    }
                }

                последняяБерлога.Заброшена = false;
            }

            var разрушеннаяБерлога = берлоги.FirstOrDefault(б => б.GetStatus() == ObjectStatus.Deleted);
            if (разрушеннаяБерлога != null)
            {
                foreach (Берлога берлога in UpdatedObject.Берлога)
                {
                    if (берлога != разрушеннаяБерлога)
                    {
                        берлога.Комфортность += 1;
                        updatedObjects.Add(берлога);
                    }
                }
            }

            return updatedObjects.Distinct().ToArray();
            // *** End programmer edit section *** (OnUpdateМедведь)
        }
    }
}

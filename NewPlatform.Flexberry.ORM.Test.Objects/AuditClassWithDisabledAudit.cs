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
    using ICSSoft.STORMNET.Business.Audit;
    
    
    // *** Start programmer edit section *** (Using statements)

    // *** End programmer edit section *** (Using statements)


    /// <summary>
    /// AuditClassWithDisabledAudit.
    /// </summary>
    // *** Start programmer edit section *** (AuditClassWithDisabledAudit CustomAttributes)

    // *** End programmer edit section *** (AuditClassWithDisabledAudit CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class AuditClassWithDisabledAudit : ICSSoft.STORMNET.DataObject, IDataObjectWithAuditFields
    {
        
        private string fName;
        
        private System.Nullable<System.DateTime> fCreateTime;
        
        private string fCreator;
        
        private System.Nullable<System.DateTime> fEditTime;
        
        private string fEditor;
        
        // *** Start programmer edit section *** (AuditClassWithDisabledAudit CustomMembers)

        // *** End programmer edit section *** (AuditClassWithDisabledAudit CustomMembers)

        
        /// <summary>
        /// Name.
        /// </summary>
        // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Name CustomAttributes)

        // *** End programmer edit section *** (AuditClassWithDisabledAudit.Name CustomAttributes)
        [StrLen(255)]
        public virtual string Name
        {
            get
            {
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Name Get start)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Name Get start)
                string result = this.fName;
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Name Get end)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Name Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Name Set start)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Name Set start)
                this.fName = value;
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Name Set end)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Name Set end)
            }
        }
        
        /// <summary>
        /// Время создания объекта.
        /// </summary>
        // *** Start programmer edit section *** (AuditClassWithDisabledAudit.CreateTime CustomAttributes)

        // *** End programmer edit section *** (AuditClassWithDisabledAudit.CreateTime CustomAttributes)
        public virtual System.Nullable<System.DateTime> CreateTime
        {
            get
            {
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.CreateTime Get start)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.CreateTime Get start)
                System.Nullable<System.DateTime> result = this.fCreateTime;
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.CreateTime Get end)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.CreateTime Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.CreateTime Set start)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.CreateTime Set start)
                this.fCreateTime = value;
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.CreateTime Set end)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.CreateTime Set end)
            }
        }
        
        /// <summary>
        /// Создатель объекта.
        /// </summary>
        // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Creator CustomAttributes)

        // *** End programmer edit section *** (AuditClassWithDisabledAudit.Creator CustomAttributes)
        [StrLen(255)]
        public virtual string Creator
        {
            get
            {
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Creator Get start)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Creator Get start)
                string result = this.fCreator;
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Creator Get end)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Creator Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Creator Set start)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Creator Set start)
                this.fCreator = value;
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Creator Set end)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Creator Set end)
            }
        }
        
        /// <summary>
        /// Время последнего редактирования объекта.
        /// </summary>
        // *** Start programmer edit section *** (AuditClassWithDisabledAudit.EditTime CustomAttributes)

        // *** End programmer edit section *** (AuditClassWithDisabledAudit.EditTime CustomAttributes)
        public virtual System.Nullable<System.DateTime> EditTime
        {
            get
            {
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.EditTime Get start)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.EditTime Get start)
                System.Nullable<System.DateTime> result = this.fEditTime;
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.EditTime Get end)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.EditTime Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.EditTime Set start)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.EditTime Set start)
                this.fEditTime = value;
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.EditTime Set end)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.EditTime Set end)
            }
        }
        
        /// <summary>
        /// Последний редактор объекта.
        /// </summary>
        // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Editor CustomAttributes)

        // *** End programmer edit section *** (AuditClassWithDisabledAudit.Editor CustomAttributes)
        [StrLen(255)]
        public virtual string Editor
        {
            get
            {
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Editor Get start)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Editor Get start)
                string result = this.fEditor;
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Editor Get end)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Editor Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Editor Set start)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Editor Set start)
                this.fEditor = value;
                // *** Start programmer edit section *** (AuditClassWithDisabledAudit.Editor Set end)

                // *** End programmer edit section *** (AuditClassWithDisabledAudit.Editor Set end)
            }
        }
    }
}

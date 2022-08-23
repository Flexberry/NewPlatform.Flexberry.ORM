﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.18010
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ICSSoft.STORMNET.Business.Audit.Objects
{
    using System;
    using System.Xml;
    using ICSSoft.STORMNET;
    
    
    // *** Start programmer edit section *** (Using statements)

    // *** End programmer edit section *** (Using statements)


    /// <summary>
    /// Класс, содержащий настройки аудита по приложению по сервисам данных
    /// </summary>
    // *** Start programmer edit section *** (AuditDSSetting CustomAttributes)

    // *** End programmer edit section *** (AuditDSSetting CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class AuditDSSetting : ICSSoft.STORMNET.DataObject
    {
        
        private System.Type fDataServiceType;
        
        private string fConnString;
        
        private string fConnStringName;

        private ICSSoft.STORMNET.Business.Audit.Objects.AuditAppSetting fAuditAppSetting;
        
        // *** Start programmer edit section *** (AuditDSSetting CustomMembers)

        public AuditDSSetting(Type dataServiceType, string connString, string connStringName)
        {
            this.DataServiceType = dataServiceType;
            this.ConnString = connString;
            this.ConnStringName = connStringName;
        }

        public AuditDSSetting(IDataService dataService, string connStringName)
        {
            this.DataServiceType = dataService.GetType();
            this.ConnString = dataService.CustomizationString;
            this.ConnStringName = connStringName;
        }

        // *** End programmer edit section *** (AuditDSSetting CustomMembers)

        
        /// <summary>
        /// Тип сервиса данных
        /// </summary>
        // *** Start programmer edit section *** (AuditDSSetting.DataServiceType CustomAttributes)

        // *** End programmer edit section *** (AuditDSSetting.DataServiceType CustomAttributes)
        [NotNull()]
        public virtual System.Type DataServiceType
        {
            get
            {
                // *** Start programmer edit section *** (AuditDSSetting.DataServiceType Get start)

                // *** End programmer edit section *** (AuditDSSetting.DataServiceType Get start)
                System.Type result = this.fDataServiceType;
                // *** Start programmer edit section *** (AuditDSSetting.DataServiceType Get end)

                // *** End programmer edit section *** (AuditDSSetting.DataServiceType Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditDSSetting.DataServiceType Set start)
                if (value == null || !typeof(IDataService).IsAssignableFrom(value))
                {
                    throw new Exception("Тип сервиса данных должен быть не null и наследником от IDataService");
                }

                // *** End programmer edit section *** (AuditDSSetting.DataServiceType Set start)
                this.fDataServiceType = value;
                // *** Start programmer edit section *** (AuditDSSetting.DataServiceType Set end)

                // *** End programmer edit section *** (AuditDSSetting.DataServiceType Set end)
            }
        }
        
        /// <summary>
        /// Строка соединения сервиса данных
        /// </summary>
        // *** Start programmer edit section *** (AuditDSSetting.ConnString CustomAttributes)

        // *** End programmer edit section *** (AuditDSSetting.ConnString CustomAttributes)
        [StrLen(255)]
        [NotNull()]
        public virtual string ConnString
        {
            get
            {
                // *** Start programmer edit section *** (AuditDSSetting.ConnString Get start)

                // *** End programmer edit section *** (AuditDSSetting.ConnString Get start)
                string result = this.fConnString;
                // *** Start programmer edit section *** (AuditDSSetting.ConnString Get end)

                // *** End programmer edit section *** (AuditDSSetting.ConnString Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditDSSetting.ConnString Set start)
                if (!HelpStructures.CheckHelper.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Строка соединения не может быть пустой");
                }
                // *** End programmer edit section *** (AuditDSSetting.ConnString Set start)
                this.fConnString = value;
                // *** Start programmer edit section *** (AuditDSSetting.ConnString Set end)

                // *** End programmer edit section *** (AuditDSSetting.ConnString Set end)
            }
        }
        
        /// <summary>
        /// Имя строки соединения (задаётся программистом)
        /// </summary>
        // *** Start programmer edit section *** (AuditDSSetting.ConnStringName CustomAttributes)

        // *** End programmer edit section *** (AuditDSSetting.ConnStringName CustomAttributes)
        [StrLen(255)]
        [NotNull()]
        public virtual string ConnStringName
        {
            get
            {
                // *** Start programmer edit section *** (AuditDSSetting.ConnStringName Get start)

                // *** End programmer edit section *** (AuditDSSetting.ConnStringName Get start)
                string result = this.fConnStringName;
                // *** Start programmer edit section *** (AuditDSSetting.ConnStringName Get end)

                // *** End programmer edit section *** (AuditDSSetting.ConnStringName Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditDSSetting.ConnStringName Set start)
                if (!HelpStructures.CheckHelper.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Имя строки соединения не может быть пустым");
                }
                // *** End programmer edit section *** (AuditDSSetting.ConnStringName Set start)
                this.fConnStringName = value;
                // *** Start programmer edit section *** (AuditDSSetting.ConnStringName Set end)

                // *** End programmer edit section *** (AuditDSSetting.ConnStringName Set end)
            }
        }
        
        /// <summary>
        /// мастеровая ссылка на шапку ICSSoft.STORMNET.Business.Audit.AuditAppSetting
        /// </summary>
        // *** Start programmer edit section *** (AuditDSSetting.AuditAppSetting CustomAttributes)

        // *** End programmer edit section *** (AuditDSSetting.AuditAppSetting CustomAttributes)
        [Agregator()]
        [NotNull()]
        public virtual ICSSoft.STORMNET.Business.Audit.Objects.AuditAppSetting AuditAppSetting
        {
            get
            {
                // *** Start programmer edit section *** (AuditDSSetting.AuditAppSetting Get start)

                // *** End programmer edit section *** (AuditDSSetting.AuditAppSetting Get start)
                ICSSoft.STORMNET.Business.Audit.Objects.AuditAppSetting result = this.fAuditAppSetting;
                // *** Start programmer edit section *** (AuditDSSetting.AuditAppSetting Get end)

                // *** End programmer edit section *** (AuditDSSetting.AuditAppSetting Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (AuditDSSetting.AuditAppSetting Set start)

                // *** End programmer edit section *** (AuditDSSetting.AuditAppSetting Set start)
                this.fAuditAppSetting = value;
                // *** Start programmer edit section *** (AuditDSSetting.AuditAppSetting Set end)

                // *** End programmer edit section *** (AuditDSSetting.AuditAppSetting Set end)
            }
        }
    }
    
    /// <summary>
    /// Detail array of AuditDSSetting
    /// </summary>
    // *** Start programmer edit section *** (DetailArrayDetailArrayOfAuditDSSetting CustomAttributes)

    // *** End programmer edit section *** (DetailArrayDetailArrayOfAuditDSSetting CustomAttributes)
    public class DetailArrayOfAuditDSSetting : ICSSoft.STORMNET.DetailArray
    {
        
        // *** Start programmer edit section *** (ICSSoft.STORMNET.Business.Audit.DetailArrayOfAuditDSSetting members)

        // *** End programmer edit section *** (ICSSoft.STORMNET.Business.Audit.DetailArrayOfAuditDSSetting members)

        
        /// <summary>
        /// Construct detail array
        /// </summary>
        public DetailArrayOfAuditDSSetting(ICSSoft.STORMNET.Business.Audit.Objects.AuditAppSetting fAuditAppSetting) : 
                base(typeof(AuditDSSetting), ((ICSSoft.STORMNET.DataObject)(fAuditAppSetting)))
        {
        }
        
        /// <summary>
        /// Returns object with type AuditDSSetting by index
        /// </summary>
        public ICSSoft.STORMNET.Business.Audit.Objects.AuditDSSetting this[int index]
        {
            get
            {
                return ((ICSSoft.STORMNET.Business.Audit.Objects.AuditDSSetting)(this.ItemByIndex(index)));
            }
        }
        
        /// <summary>
        /// Adds object with type AuditDSSetting
        /// </summary>
        public virtual void Add(ICSSoft.STORMNET.Business.Audit.Objects.AuditDSSetting dataobject)
        {
            this.AddObject(((ICSSoft.STORMNET.DataObject)(dataobject)));
        }
    }
}
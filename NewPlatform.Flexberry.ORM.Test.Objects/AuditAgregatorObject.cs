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
    /// AuditAgregatorObject.
    /// </summary>
    //  *** Start programmer edit section *** (AuditAgregatorObject CustomAttributes)

    //  *** End programmer edit section *** (AuditAgregatorObject CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class AuditAgregatorObject : ICSSoft.STORMNET.DataObject
    {
        
        private string fLogin;
        
        private string fName;
        
        private string fSurname;
        
        private NewPlatform.Flexberry.ORM.Tests.AuditMasterObject fMasterObject;
        
        //  *** Start programmer edit section *** (AuditAgregatorObject CustomMembers)

        //  *** End programmer edit section *** (AuditAgregatorObject CustomMembers)

        
        /// <summary>
        /// Login.
        /// </summary>
        //  *** Start programmer edit section *** (AuditAgregatorObject.Login CustomAttributes)

        //  *** End programmer edit section *** (AuditAgregatorObject.Login CustomAttributes)
        [StrLen(255)]
        public virtual string Login
        {
            get
            {
                //  *** Start programmer edit section *** (AuditAgregatorObject.Login Get start)

                //  *** End programmer edit section *** (AuditAgregatorObject.Login Get start)
                string result = this.fLogin;
                //  *** Start programmer edit section *** (AuditAgregatorObject.Login Get end)

                //  *** End programmer edit section *** (AuditAgregatorObject.Login Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (AuditAgregatorObject.Login Set start)

                //  *** End programmer edit section *** (AuditAgregatorObject.Login Set start)
                this.fLogin = value;
                //  *** Start programmer edit section *** (AuditAgregatorObject.Login Set end)

                //  *** End programmer edit section *** (AuditAgregatorObject.Login Set end)
            }
        }
        
        /// <summary>
        /// Name.
        /// </summary>
        //  *** Start programmer edit section *** (AuditAgregatorObject.Name CustomAttributes)

        //  *** End programmer edit section *** (AuditAgregatorObject.Name CustomAttributes)
        [StrLen(255)]
        public virtual string Name
        {
            get
            {
                //  *** Start programmer edit section *** (AuditAgregatorObject.Name Get start)

                //  *** End programmer edit section *** (AuditAgregatorObject.Name Get start)
                string result = this.fName;
                //  *** Start programmer edit section *** (AuditAgregatorObject.Name Get end)

                //  *** End programmer edit section *** (AuditAgregatorObject.Name Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (AuditAgregatorObject.Name Set start)

                //  *** End programmer edit section *** (AuditAgregatorObject.Name Set start)
                this.fName = value;
                //  *** Start programmer edit section *** (AuditAgregatorObject.Name Set end)

                //  *** End programmer edit section *** (AuditAgregatorObject.Name Set end)
            }
        }
        
        /// <summary>
        /// Surname.
        /// </summary>
        //  *** Start programmer edit section *** (AuditAgregatorObject.Surname CustomAttributes)

        //  *** End programmer edit section *** (AuditAgregatorObject.Surname CustomAttributes)
        [StrLen(255)]
        public virtual string Surname
        {
            get
            {
                //  *** Start programmer edit section *** (AuditAgregatorObject.Surname Get start)

                //  *** End programmer edit section *** (AuditAgregatorObject.Surname Get start)
                string result = this.fSurname;
                //  *** Start programmer edit section *** (AuditAgregatorObject.Surname Get end)

                //  *** End programmer edit section *** (AuditAgregatorObject.Surname Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (AuditAgregatorObject.Surname Set start)

                //  *** End programmer edit section *** (AuditAgregatorObject.Surname Set start)
                this.fSurname = value;
                //  *** Start programmer edit section *** (AuditAgregatorObject.Surname Set end)

                //  *** End programmer edit section *** (AuditAgregatorObject.Surname Set end)
            }
        }
        
        /// <summary>
        /// NameSurname.
        /// </summary>
        //  *** Start programmer edit section *** (AuditAgregatorObject.NameSurname CustomAttributes)

        //  *** End programmer edit section *** (AuditAgregatorObject.NameSurname CustomAttributes)
        [ICSSoft.STORMNET.NotStored()]
        [StrLen(255)]
        public virtual string NameSurname
        {
            get
            {
                //  *** Start programmer edit section *** (AuditAgregatorObject.NameSurname Get)
                return null;
                //  *** End programmer edit section *** (AuditAgregatorObject.NameSurname Get)
            }
            set
            {
                //  *** Start programmer edit section *** (AuditAgregatorObject.NameSurname Set)

                //  *** End programmer edit section *** (AuditAgregatorObject.NameSurname Set)
            }
        }
        
        /// <summary>
        /// AuditAgregatorObject.
        /// </summary>
        //  *** Start programmer edit section *** (AuditAgregatorObject.MasterObject CustomAttributes)

        //  *** End programmer edit section *** (AuditAgregatorObject.MasterObject CustomAttributes)
        [PropertyStorage(new string[] {
                "MasterObject"})]
        public virtual NewPlatform.Flexberry.ORM.Tests.AuditMasterObject MasterObject
        {
            get
            {
                //  *** Start programmer edit section *** (AuditAgregatorObject.MasterObject Get start)

                //  *** End programmer edit section *** (AuditAgregatorObject.MasterObject Get start)
                NewPlatform.Flexberry.ORM.Tests.AuditMasterObject result = this.fMasterObject;
                //  *** Start programmer edit section *** (AuditAgregatorObject.MasterObject Get end)

                //  *** End programmer edit section *** (AuditAgregatorObject.MasterObject Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (AuditAgregatorObject.MasterObject Set start)

                //  *** End programmer edit section *** (AuditAgregatorObject.MasterObject Set start)
                this.fMasterObject = value;
                //  *** Start programmer edit section *** (AuditAgregatorObject.MasterObject Set end)

                //  *** End programmer edit section *** (AuditAgregatorObject.MasterObject Set end)
            }
        }
    }
}

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
    /// AuditMasterObject.
    /// </summary>
    //  *** Start programmer edit section *** (AuditMasterObject CustomAttributes)

    //  *** End programmer edit section *** (AuditMasterObject CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class AuditMasterObject : ICSSoft.STORMNET.DataObject
    {
        
        private string fLogin;
        
        private string fName;
        
        private string fSurname;
        
        private NewPlatform.Flexberry.ORM.Tests.AuditMasterMasterObject fMasterObject;
        
        //  *** Start programmer edit section *** (AuditMasterObject CustomMembers)

        //  *** End programmer edit section *** (AuditMasterObject CustomMembers)

        
        /// <summary>
        /// Login.
        /// </summary>
        //  *** Start programmer edit section *** (AuditMasterObject.Login CustomAttributes)

        //  *** End programmer edit section *** (AuditMasterObject.Login CustomAttributes)
        [StrLen(255)]
        public virtual string Login
        {
            get
            {
                //  *** Start programmer edit section *** (AuditMasterObject.Login Get start)

                //  *** End programmer edit section *** (AuditMasterObject.Login Get start)
                string result = this.fLogin;
                //  *** Start programmer edit section *** (AuditMasterObject.Login Get end)

                //  *** End programmer edit section *** (AuditMasterObject.Login Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (AuditMasterObject.Login Set start)

                //  *** End programmer edit section *** (AuditMasterObject.Login Set start)
                this.fLogin = value;
                //  *** Start programmer edit section *** (AuditMasterObject.Login Set end)

                //  *** End programmer edit section *** (AuditMasterObject.Login Set end)
            }
        }
        
        /// <summary>
        /// Name.
        /// </summary>
        //  *** Start programmer edit section *** (AuditMasterObject.Name CustomAttributes)

        //  *** End programmer edit section *** (AuditMasterObject.Name CustomAttributes)
        [StrLen(255)]
        public virtual string Name
        {
            get
            {
                //  *** Start programmer edit section *** (AuditMasterObject.Name Get start)

                //  *** End programmer edit section *** (AuditMasterObject.Name Get start)
                string result = this.fName;
                //  *** Start programmer edit section *** (AuditMasterObject.Name Get end)

                //  *** End programmer edit section *** (AuditMasterObject.Name Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (AuditMasterObject.Name Set start)

                //  *** End programmer edit section *** (AuditMasterObject.Name Set start)
                this.fName = value;
                //  *** Start programmer edit section *** (AuditMasterObject.Name Set end)

                //  *** End programmer edit section *** (AuditMasterObject.Name Set end)
            }
        }
        
        /// <summary>
        /// Surname.
        /// </summary>
        //  *** Start programmer edit section *** (AuditMasterObject.Surname CustomAttributes)

        //  *** End programmer edit section *** (AuditMasterObject.Surname CustomAttributes)
        [StrLen(255)]
        public virtual string Surname
        {
            get
            {
                //  *** Start programmer edit section *** (AuditMasterObject.Surname Get start)

                //  *** End programmer edit section *** (AuditMasterObject.Surname Get start)
                string result = this.fSurname;
                //  *** Start programmer edit section *** (AuditMasterObject.Surname Get end)

                //  *** End programmer edit section *** (AuditMasterObject.Surname Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (AuditMasterObject.Surname Set start)

                //  *** End programmer edit section *** (AuditMasterObject.Surname Set start)
                this.fSurname = value;
                //  *** Start programmer edit section *** (AuditMasterObject.Surname Set end)

                //  *** End programmer edit section *** (AuditMasterObject.Surname Set end)
            }
        }
        
        /// <summary>
        /// NameSurname.
        /// </summary>
        //  *** Start programmer edit section *** (AuditMasterObject.NameSurname CustomAttributes)

        //  *** End programmer edit section *** (AuditMasterObject.NameSurname CustomAttributes)
        [ICSSoft.STORMNET.NotStored()]
        [StrLen(255)]
        public virtual string NameSurname
        {
            get
            {
                //  *** Start programmer edit section *** (AuditMasterObject.NameSurname Get)
                return null;
                //  *** End programmer edit section *** (AuditMasterObject.NameSurname Get)
            }
            set
            {
                //  *** Start programmer edit section *** (AuditMasterObject.NameSurname Set)

                //  *** End programmer edit section *** (AuditMasterObject.NameSurname Set)
            }
        }
        
        /// <summary>
        /// AuditMasterObject.
        /// </summary>
        //  *** Start programmer edit section *** (AuditMasterObject.MasterObject CustomAttributes)

        //  *** End programmer edit section *** (AuditMasterObject.MasterObject CustomAttributes)
        [PropertyStorage(new string[] {
                "MasterObject"})]
        public virtual NewPlatform.Flexberry.ORM.Tests.AuditMasterMasterObject MasterObject
        {
            get
            {
                //  *** Start programmer edit section *** (AuditMasterObject.MasterObject Get start)

                //  *** End programmer edit section *** (AuditMasterObject.MasterObject Get start)
                NewPlatform.Flexberry.ORM.Tests.AuditMasterMasterObject result = this.fMasterObject;
                //  *** Start programmer edit section *** (AuditMasterObject.MasterObject Get end)

                //  *** End programmer edit section *** (AuditMasterObject.MasterObject Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (AuditMasterObject.MasterObject Set start)

                //  *** End programmer edit section *** (AuditMasterObject.MasterObject Set start)
                this.fMasterObject = value;
                //  *** Start programmer edit section *** (AuditMasterObject.MasterObject Set end)

                //  *** End programmer edit section *** (AuditMasterObject.MasterObject Set end)
            }
        }
    }
}

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
    /// DataObjectForTest.
    /// </summary>
    // *** Start programmer edit section *** (DataObjectForTest CustomAttributes)

    // *** End programmer edit section *** (DataObjectForTest CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    public class DataObjectForTest : ICSSoft.STORMNET.DataObject
    {
        
        private string fName;
        
        private int fHeight;
        
        private System.Nullable<DateTime> fBirthDate;
        
        private bool fGender;
        
        // *** Start programmer edit section *** (DataObjectForTest CustomMembers)

        // *** End programmer edit section *** (DataObjectForTest CustomMembers)

        
        /// <summary>
        /// Name.
        /// </summary>
        // *** Start programmer edit section *** (DataObjectForTest.Name CustomAttributes)

        // *** End programmer edit section *** (DataObjectForTest.Name CustomAttributes)
        [StrLen(255)]
        public virtual string Name
        {
            get
            {
                // *** Start programmer edit section *** (DataObjectForTest.Name Get start)

                // *** End programmer edit section *** (DataObjectForTest.Name Get start)
                string result = this.fName;
                // *** Start programmer edit section *** (DataObjectForTest.Name Get end)

                // *** End programmer edit section *** (DataObjectForTest.Name Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (DataObjectForTest.Name Set start)

                // *** End programmer edit section *** (DataObjectForTest.Name Set start)
                this.fName = value;
                // *** Start programmer edit section *** (DataObjectForTest.Name Set end)

                // *** End programmer edit section *** (DataObjectForTest.Name Set end)
            }
        }
        
        /// <summary>
        /// Height.
        /// </summary>
        // *** Start programmer edit section *** (DataObjectForTest.Height CustomAttributes)

        // *** End programmer edit section *** (DataObjectForTest.Height CustomAttributes)
        public virtual int Height
        {
            get
            {
                // *** Start programmer edit section *** (DataObjectForTest.Height Get start)

                // *** End programmer edit section *** (DataObjectForTest.Height Get start)
                int result = this.fHeight;
                // *** Start programmer edit section *** (DataObjectForTest.Height Get end)

                // *** End programmer edit section *** (DataObjectForTest.Height Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (DataObjectForTest.Height Set start)

                // *** End programmer edit section *** (DataObjectForTest.Height Set start)
                this.fHeight = value;
                // *** Start programmer edit section *** (DataObjectForTest.Height Set end)

                // *** End programmer edit section *** (DataObjectForTest.Height Set end)
            }
        }
        
        /// <summary>
        /// BirthDate.
        /// </summary>
        // *** Start programmer edit section *** (DataObjectForTest.BirthDate CustomAttributes)

        // *** End programmer edit section *** (DataObjectForTest.BirthDate CustomAttributes)
        public virtual System.Nullable<DateTime> BirthDate
        {
            get
            {
                // *** Start programmer edit section *** (DataObjectForTest.BirthDate Get start)

                // *** End programmer edit section *** (DataObjectForTest.BirthDate Get start)
                System.Nullable<DateTime> result = this.fBirthDate;
                // *** Start programmer edit section *** (DataObjectForTest.BirthDate Get end)

                // *** End programmer edit section *** (DataObjectForTest.BirthDate Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (DataObjectForTest.BirthDate Set start)

                // *** End programmer edit section *** (DataObjectForTest.BirthDate Set start)
                this.fBirthDate = value;
                // *** Start programmer edit section *** (DataObjectForTest.BirthDate Set end)

                // *** End programmer edit section *** (DataObjectForTest.BirthDate Set end)
            }
        }
        
        /// <summary>
        /// Gender.
        /// </summary>
        // *** Start programmer edit section *** (DataObjectForTest.Gender CustomAttributes)

        // *** End programmer edit section *** (DataObjectForTest.Gender CustomAttributes)
        public virtual bool Gender
        {
            get
            {
                // *** Start programmer edit section *** (DataObjectForTest.Gender Get start)

                // *** End programmer edit section *** (DataObjectForTest.Gender Get start)
                bool result = this.fGender;
                // *** Start programmer edit section *** (DataObjectForTest.Gender Get end)

                // *** End programmer edit section *** (DataObjectForTest.Gender Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (DataObjectForTest.Gender Set start)

                // *** End programmer edit section *** (DataObjectForTest.Gender Set start)
                this.fGender = value;
                // *** Start programmer edit section *** (DataObjectForTest.Gender Set end)

                // *** End programmer edit section *** (DataObjectForTest.Gender Set end)
            }
        }
    }
}

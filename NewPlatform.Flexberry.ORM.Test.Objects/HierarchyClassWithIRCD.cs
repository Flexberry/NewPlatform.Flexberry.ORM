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
    /// HierarchyClassWithIRCD.
    /// </summary>
    // *** Start programmer edit section *** (HierarchyClassWithIRCD CustomAttributes)

    // *** End programmer edit section *** (HierarchyClassWithIRCD CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("HierarchyClassWithIRCDE", new string[] {
            "Name",
            "Parent",
            "Parent.Name"})]
    [AssociatedDetailViewAttribute("HierarchyClassWithIRCDE", "DetailForIRCD", "DetailForIRCDE", true, "", "DetailForIRCDE", true, new string[] {
            ""})]
    public class HierarchyClassWithIRCD : ICSSoft.STORMNET.DataObject, ICSSoft.STORMNET.Business.Interfaces.IReferencesCascadeDelete
    {
        
        private string fName;
        
        private NewPlatform.Flexberry.ORM.Tests.HierarchyClassWithIRCD fParent;
        
        private NewPlatform.Flexberry.ORM.Tests.DetailArrayOfDetailForIRCD fDetailForIRCD;
        
        // *** Start programmer edit section *** (HierarchyClassWithIRCD CustomMembers)

        // *** End programmer edit section *** (HierarchyClassWithIRCD CustomMembers)

        
        /// <summary>
        /// Name.
        /// </summary>
        // *** Start programmer edit section *** (HierarchyClassWithIRCD.Name CustomAttributes)

        // *** End programmer edit section *** (HierarchyClassWithIRCD.Name CustomAttributes)
        [StrLen(255)]
        public virtual string Name
        {
            get
            {
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.Name Get start)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.Name Get start)
                string result = this.fName;
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.Name Get end)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.Name Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.Name Set start)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.Name Set start)
                this.fName = value;
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.Name Set end)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.Name Set end)
            }
        }
        
        /// <summary>
        /// HierarchyClassWithIRCD.
        /// </summary>
        // *** Start programmer edit section *** (HierarchyClassWithIRCD.Parent CustomAttributes)

        // *** End programmer edit section *** (HierarchyClassWithIRCD.Parent CustomAttributes)
        [PropertyStorage(new string[] {
                "Parent"})]
        public virtual NewPlatform.Flexberry.ORM.Tests.HierarchyClassWithIRCD Parent
        {
            get
            {
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.Parent Get start)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.Parent Get start)
                NewPlatform.Flexberry.ORM.Tests.HierarchyClassWithIRCD result = this.fParent;
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.Parent Get end)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.Parent Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.Parent Set start)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.Parent Set start)
                this.fParent = value;
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.Parent Set end)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.Parent Set end)
            }
        }
        
        /// <summary>
        /// HierarchyClassWithIRCD.
        /// </summary>
        // *** Start programmer edit section *** (HierarchyClassWithIRCD.DetailForIRCD CustomAttributes)

        // *** End programmer edit section *** (HierarchyClassWithIRCD.DetailForIRCD CustomAttributes)
        public virtual NewPlatform.Flexberry.ORM.Tests.DetailArrayOfDetailForIRCD DetailForIRCD
        {
            get
            {
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.DetailForIRCD Get start)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.DetailForIRCD Get start)
                if ((this.fDetailForIRCD == null))
                {
                    this.fDetailForIRCD = new NewPlatform.Flexberry.ORM.Tests.DetailArrayOfDetailForIRCD(this);
                }
                NewPlatform.Flexberry.ORM.Tests.DetailArrayOfDetailForIRCD result = this.fDetailForIRCD;
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.DetailForIRCD Get end)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.DetailForIRCD Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.DetailForIRCD Set start)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.DetailForIRCD Set start)
                this.fDetailForIRCD = value;
                // *** Start programmer edit section *** (HierarchyClassWithIRCD.DetailForIRCD Set end)

                // *** End programmer edit section *** (HierarchyClassWithIRCD.DetailForIRCD Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "HierarchyClassWithIRCDE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View HierarchyClassWithIRCDE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("HierarchyClassWithIRCDE", typeof(NewPlatform.Flexberry.ORM.Tests.HierarchyClassWithIRCD));
                }
            }
        }
    }
}

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
    /// Порода.
    /// </summary>
    // *** Start programmer edit section *** (Порода CustomAttributes)

    // *** End programmer edit section *** (Порода CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("k_ПородаE", new string[] {
            "Название as \'Название\'",
            "Иерархия as \'Иерархия\'",
            "Иерархия.Название as \'Название\'"})]
    [View("k_ПородаL", new string[] {
            "Название as \'Название\'",
            "Иерархия as \'Название\'"}, Hidden=new string[] {
            "Иерархия"})]
    [View("ПородаE", new string[] {
            "Название",
            "Иерархия as \'\'",
            "Иерархия.Название as \'Иерархия\'"})]
    [View("ПородаL", new string[] {
            "Название as \'Название\'",
            "Иерархия as \'Название\'"}, Hidden=new string[] {
            "Иерархия"})]
    [View("ПородаwwwwL", new string[] {
            "Название",
            "Иерархия",
            "Иерархия.Название as \'Иерархия\'"})]
    public class Порода : ICSSoft.STORMNET.DataObject
    {
        
        private string fНазвание;
        
        private ICSSoft.STORMNET.KeyGen.KeyGuid fКлюч;
        
        private NewPlatform.Flexberry.ORM.Tests.Порода fИерархия;
        
        private NewPlatform.Flexberry.ORM.Tests.ТипПороды fТипПороды;
        
        // *** Start programmer edit section *** (Порода CustomMembers)

        // *** End programmer edit section *** (Порода CustomMembers)

        
        /// <summary>
        /// Название.
        /// </summary>
        // *** Start programmer edit section *** (Порода.Название CustomAttributes)

        // *** End programmer edit section *** (Порода.Название CustomAttributes)
        [StrLen(255)]
        [TrimmedStringStorage(false)]
        public virtual string Название
        {
            get
            {
                // *** Start programmer edit section *** (Порода.Название Get start)

                // *** End programmer edit section *** (Порода.Название Get start)
                string result = this.fНазвание;
                // *** Start programmer edit section *** (Порода.Название Get end)

                // *** End programmer edit section *** (Порода.Название Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Порода.Название Set start)

                // *** End programmer edit section *** (Порода.Название Set start)
                this.fНазвание = value;
                // *** Start programmer edit section *** (Порода.Название Set end)

                // *** End programmer edit section *** (Порода.Название Set end)
            }
        }
        
        /// <summary>
        /// Ключ.
        /// </summary>
        // *** Start programmer edit section *** (Порода.Ключ CustomAttributes)

        // *** End programmer edit section *** (Порода.Ключ CustomAttributes)
        public virtual ICSSoft.STORMNET.KeyGen.KeyGuid Ключ
        {
            get
            {
                // *** Start programmer edit section *** (Порода.Ключ Get start)

                // *** End programmer edit section *** (Порода.Ключ Get start)
                ICSSoft.STORMNET.KeyGen.KeyGuid result = this.fКлюч;
                // *** Start programmer edit section *** (Порода.Ключ Get end)

                // *** End programmer edit section *** (Порода.Ключ Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Порода.Ключ Set start)

                // *** End programmer edit section *** (Порода.Ключ Set start)
                this.fКлюч = value;
                // *** Start programmer edit section *** (Порода.Ключ Set end)

                // *** End programmer edit section *** (Порода.Ключ Set end)
            }
        }
        
        /// <summary>
        /// Порода.
        /// </summary>
        // *** Start programmer edit section *** (Порода.Иерархия CustomAttributes)

        // *** End programmer edit section *** (Порода.Иерархия CustomAttributes)
        [PropertyStorage(new string[] {
                "Иерархия"})]
        public virtual NewPlatform.Flexberry.ORM.Tests.Порода Иерархия
        {
            get
            {
                // *** Start programmer edit section *** (Порода.Иерархия Get start)

                // *** End programmer edit section *** (Порода.Иерархия Get start)
                NewPlatform.Flexberry.ORM.Tests.Порода result = this.fИерархия;
                // *** Start programmer edit section *** (Порода.Иерархия Get end)

                // *** End programmer edit section *** (Порода.Иерархия Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Порода.Иерархия Set start)

                // *** End programmer edit section *** (Порода.Иерархия Set start)
                this.fИерархия = value;
                // *** Start programmer edit section *** (Порода.Иерархия Set end)

                // *** End programmer edit section *** (Порода.Иерархия Set end)
            }
        }
        
        /// <summary>
        /// Порода.
        /// </summary>
        // *** Start programmer edit section *** (Порода.ТипПороды CustomAttributes)

        // *** End programmer edit section *** (Порода.ТипПороды CustomAttributes)
        [PropertyStorage(new string[] {
                "ТипПороды"})]
        public virtual NewPlatform.Flexberry.ORM.Tests.ТипПороды ТипПороды
        {
            get
            {
                // *** Start programmer edit section *** (Порода.ТипПороды Get start)

                // *** End programmer edit section *** (Порода.ТипПороды Get start)
                NewPlatform.Flexberry.ORM.Tests.ТипПороды result = this.fТипПороды;
                // *** Start programmer edit section *** (Порода.ТипПороды Get end)

                // *** End programmer edit section *** (Порода.ТипПороды Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (Порода.ТипПороды Set start)

                // *** End programmer edit section *** (Порода.ТипПороды Set start)
                this.fТипПороды = value;
                // *** Start programmer edit section *** (Порода.ТипПороды Set end)

                // *** End programmer edit section *** (Порода.ТипПороды Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "k_ПородаE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View k_ПородаE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("k_ПородаE", typeof(NewPlatform.Flexberry.ORM.Tests.Порода));
                }
            }
            
            /// <summary>
            /// "k_ПородаL" view.
            /// </summary>
            public static ICSSoft.STORMNET.View k_ПородаL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("k_ПородаL", typeof(NewPlatform.Flexberry.ORM.Tests.Порода));
                }
            }
            
            /// <summary>
            /// "ПородаE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View ПородаE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("ПородаE", typeof(NewPlatform.Flexberry.ORM.Tests.Порода));
                }
            }
            
            /// <summary>
            /// "ПородаL" view.
            /// </summary>
            public static ICSSoft.STORMNET.View ПородаL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("ПородаL", typeof(NewPlatform.Flexberry.ORM.Tests.Порода));
                }
            }
            
            /// <summary>
            /// "ПородаwwwwL" view.
            /// </summary>
            public static ICSSoft.STORMNET.View ПородаwwwwL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("ПородаwwwwL", typeof(NewPlatform.Flexberry.ORM.Tests.Порода));
                }
            }
        }
    }
}

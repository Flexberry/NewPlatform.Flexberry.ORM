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
    /// ИсходящийЗапрос.
    /// </summary>
    //  *** Start programmer edit section *** (ИсходящийЗапрос CustomAttributes)

    //  *** End programmer edit section *** (ИсходящийЗапрос CustomAttributes)
    [AutoAltered()]
    [ICSSoft.STORMNET.NotStored()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("ИсходящийЗапросView", new string[] {
            "ПервоеДлинноеПолеДляПроверки",
            "ВтороеДлинноеПолеДляПроверки",
            "ПятоеДлинноеПолеДляПроверки"})]
    public class ИсходящийЗапрос : ICSSoft.STORMNET.DataObject
    {
        
        private bool fПервоеДлинноеПолеДляПроверки;
        
        private NewPlatform.Flexberry.ORM.Tests.СтатусЗапроса fВтороеДлинноеПолеДляПроверки;
        
        private int fПятоеДлинноеПолеДляПроверки;
        
        private NewPlatform.Flexberry.ORM.Tests.DetailArrayOfЭтапИсходящегоЗапроса fЭтапы;
        
        //  *** Start programmer edit section *** (ИсходящийЗапрос CustomMembers)

        //  *** End programmer edit section *** (ИсходящийЗапрос CustomMembers)

        
        /// <summary>
        /// ПервоеДлинноеПолеДляПроверки.
        /// </summary>
        //  *** Start programmer edit section *** (ИсходящийЗапрос.ПервоеДлинноеПолеДляПроверки CustomAttributes)

        //  *** End programmer edit section *** (ИсходящийЗапрос.ПервоеДлинноеПолеДляПроверки CustomAttributes)
        public virtual bool ПервоеДлинноеПолеДляПроверки
        {
            get
            {
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ПервоеДлинноеПолеДляПроверки Get start)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ПервоеДлинноеПолеДляПроверки Get start)
                bool result = this.fПервоеДлинноеПолеДляПроверки;
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ПервоеДлинноеПолеДляПроверки Get end)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ПервоеДлинноеПолеДляПроверки Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ПервоеДлинноеПолеДляПроверки Set start)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ПервоеДлинноеПолеДляПроверки Set start)
                this.fПервоеДлинноеПолеДляПроверки = value;
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ПервоеДлинноеПолеДляПроверки Set end)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ПервоеДлинноеПолеДляПроверки Set end)
            }
        }
        
        /// <summary>
        /// ВтороеДлинноеПолеДляПроверки.
        /// </summary>
        //  *** Start programmer edit section *** (ИсходящийЗапрос.ВтороеДлинноеПолеДляПроверки CustomAttributes)

        //  *** End programmer edit section *** (ИсходящийЗапрос.ВтороеДлинноеПолеДляПроверки CustomAttributes)
        public virtual NewPlatform.Flexberry.ORM.Tests.СтатусЗапроса ВтороеДлинноеПолеДляПроверки
        {
            get
            {
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ВтороеДлинноеПолеДляПроверки Get start)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ВтороеДлинноеПолеДляПроверки Get start)
                NewPlatform.Flexberry.ORM.Tests.СтатусЗапроса result = this.fВтороеДлинноеПолеДляПроверки;
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ВтороеДлинноеПолеДляПроверки Get end)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ВтороеДлинноеПолеДляПроверки Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ВтороеДлинноеПолеДляПроверки Set start)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ВтороеДлинноеПолеДляПроверки Set start)
                this.fВтороеДлинноеПолеДляПроверки = value;
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ВтороеДлинноеПолеДляПроверки Set end)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ВтороеДлинноеПолеДляПроверки Set end)
            }
        }
        
        /// <summary>
        /// ПятоеДлинноеПолеДляПроверки.
        /// </summary>
        //  *** Start programmer edit section *** (ИсходящийЗапрос.ПятоеДлинноеПолеДляПроверки CustomAttributes)

        //  *** End programmer edit section *** (ИсходящийЗапрос.ПятоеДлинноеПолеДляПроверки CustomAttributes)
        public virtual int ПятоеДлинноеПолеДляПроверки
        {
            get
            {
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ПятоеДлинноеПолеДляПроверки Get start)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ПятоеДлинноеПолеДляПроверки Get start)
                int result = this.fПятоеДлинноеПолеДляПроверки;
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ПятоеДлинноеПолеДляПроверки Get end)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ПятоеДлинноеПолеДляПроверки Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ПятоеДлинноеПолеДляПроверки Set start)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ПятоеДлинноеПолеДляПроверки Set start)
                this.fПятоеДлинноеПолеДляПроверки = value;
                //  *** Start programmer edit section *** (ИсходящийЗапрос.ПятоеДлинноеПолеДляПроверки Set end)

                //  *** End programmer edit section *** (ИсходящийЗапрос.ПятоеДлинноеПолеДляПроверки Set end)
            }
        }
        
        /// <summary>
        /// ИсходящийЗапрос.
        /// </summary>
        //  *** Start programmer edit section *** (ИсходящийЗапрос.Этапы CustomAttributes)

        //  *** End programmer edit section *** (ИсходящийЗапрос.Этапы CustomAttributes)
        public virtual NewPlatform.Flexberry.ORM.Tests.DetailArrayOfЭтапИсходящегоЗапроса Этапы
        {
            get
            {
                //  *** Start programmer edit section *** (ИсходящийЗапрос.Этапы Get start)

                //  *** End programmer edit section *** (ИсходящийЗапрос.Этапы Get start)
                if ((this.fЭтапы == null))
                {
                    this.fЭтапы = new NewPlatform.Flexberry.ORM.Tests.DetailArrayOfЭтапИсходящегоЗапроса(this);
                }
                NewPlatform.Flexberry.ORM.Tests.DetailArrayOfЭтапИсходящегоЗапроса result = this.fЭтапы;
                //  *** Start programmer edit section *** (ИсходящийЗапрос.Этапы Get end)

                //  *** End programmer edit section *** (ИсходящийЗапрос.Этапы Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (ИсходящийЗапрос.Этапы Set start)

                //  *** End programmer edit section *** (ИсходящийЗапрос.Этапы Set start)
                this.fЭтапы = value;
                //  *** Start programmer edit section *** (ИсходящийЗапрос.Этапы Set end)

                //  *** End programmer edit section *** (ИсходящийЗапрос.Этапы Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "ИсходящийЗапросView" view.
            /// </summary>
            public static ICSSoft.STORMNET.View ИсходящийЗапросView
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("ИсходящийЗапросView", typeof(NewPlatform.Flexberry.ORM.Tests.ИсходящийЗапрос));
                }
            }
        }
    }
}

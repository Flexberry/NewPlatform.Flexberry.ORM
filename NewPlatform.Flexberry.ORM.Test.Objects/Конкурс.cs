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
    /// Конкурс.
    /// </summary>
    //  *** Start programmer edit section *** (Конкурс CustomAttributes)

    //  *** End programmer edit section *** (Конкурс CustomAttributes)
    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("КонкурсE", new string[] {
            "Название",
            "Описание",
            "ДатаНачала",
            "ДатаОкончания",
            "НачалоОценки",
            "ОкончаниеОценки",
            "Организатор",
            "Состояние"})]
    [AssociatedDetailViewAttribute("КонкурсE", "Документы", "ДокументацияККонкурсуE", true, "", "", true, new string[] {
            ""})]
    [AssociatedDetailViewAttribute("КонкурсE", "КритерииОценки", "КритерийОценкиE", true, "", "", true, new string[] {
            ""})]
    public class Конкурс : ICSSoft.STORMNET.DataObject
    {
        
        private string fНазвание;
        
        private string fОписание;
        
        private ICSSoft.STORMNET.UserDataTypes.NullableDateTime fДатаНачала = ICSSoft.STORMNET.UserDataTypes.NullableDateTime.Now;
        
        private ICSSoft.STORMNET.UserDataTypes.NullableDateTime fДатаОкончания = ICSSoft.STORMNET.UserDataTypes.NullableDateTime.Now;
        
        private ICSSoft.STORMNET.UserDataTypes.NullableDateTime fНачалоОценки = ICSSoft.STORMNET.UserDataTypes.NullableDateTime.Now;
        
        private ICSSoft.STORMNET.UserDataTypes.NullableDateTime fОкончаниеОценки = ICSSoft.STORMNET.UserDataTypes.NullableDateTime.Now;
        
        private NewPlatform.Flexberry.ORM.Tests.Состояние fСостояние;
        
        private NewPlatform.Flexberry.ORM.Tests.Пользователь fОрганизатор;
        
        private NewPlatform.Flexberry.ORM.Tests.DetailArrayOfДокументацияККонкурсу fДокументы;
        
        private NewPlatform.Flexberry.ORM.Tests.DetailArrayOfКритерийОценки fКритерииОценки;
        
        //  *** Start programmer edit section *** (Конкурс CustomMembers)

        //  *** End programmer edit section *** (Конкурс CustomMembers)

        
        /// <summary>
        /// Название.
        /// </summary>
        //  *** Start programmer edit section *** (Конкурс.Название CustomAttributes)

        //  *** End programmer edit section *** (Конкурс.Название CustomAttributes)
        [StrLen(255)]
        public virtual string Название
        {
            get
            {
                //  *** Start programmer edit section *** (Конкурс.Название Get start)

                //  *** End programmer edit section *** (Конкурс.Название Get start)
                string result = this.fНазвание;
                //  *** Start programmer edit section *** (Конкурс.Название Get end)

                //  *** End programmer edit section *** (Конкурс.Название Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Конкурс.Название Set start)

                //  *** End programmer edit section *** (Конкурс.Название Set start)
                this.fНазвание = value;
                //  *** Start programmer edit section *** (Конкурс.Название Set end)

                //  *** End programmer edit section *** (Конкурс.Название Set end)
            }
        }
        
        /// <summary>
        /// Описание.
        /// </summary>
        //  *** Start programmer edit section *** (Конкурс.Описание CustomAttributes)

        //  *** End programmer edit section *** (Конкурс.Описание CustomAttributes)
        [StrLen(255)]
        public virtual string Описание
        {
            get
            {
                //  *** Start programmer edit section *** (Конкурс.Описание Get start)

                //  *** End programmer edit section *** (Конкурс.Описание Get start)
                string result = this.fОписание;
                //  *** Start programmer edit section *** (Конкурс.Описание Get end)

                //  *** End programmer edit section *** (Конкурс.Описание Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Конкурс.Описание Set start)

                //  *** End programmer edit section *** (Конкурс.Описание Set start)
                this.fОписание = value;
                //  *** Start programmer edit section *** (Конкурс.Описание Set end)

                //  *** End programmer edit section *** (Конкурс.Описание Set end)
            }
        }
        
        /// <summary>
        /// ДатаНачала.
        /// </summary>
        //  *** Start programmer edit section *** (Конкурс.ДатаНачала CustomAttributes)

        //  *** End programmer edit section *** (Конкурс.ДатаНачала CustomAttributes)
        public virtual ICSSoft.STORMNET.UserDataTypes.NullableDateTime ДатаНачала
        {
            get
            {
                //  *** Start programmer edit section *** (Конкурс.ДатаНачала Get start)

                //  *** End programmer edit section *** (Конкурс.ДатаНачала Get start)
                ICSSoft.STORMNET.UserDataTypes.NullableDateTime result = this.fДатаНачала;
                //  *** Start programmer edit section *** (Конкурс.ДатаНачала Get end)

                //  *** End programmer edit section *** (Конкурс.ДатаНачала Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Конкурс.ДатаНачала Set start)

                //  *** End programmer edit section *** (Конкурс.ДатаНачала Set start)
                this.fДатаНачала = value;
                //  *** Start programmer edit section *** (Конкурс.ДатаНачала Set end)

                //  *** End programmer edit section *** (Конкурс.ДатаНачала Set end)
            }
        }
        
        /// <summary>
        /// ДатаОкончания.
        /// </summary>
        //  *** Start programmer edit section *** (Конкурс.ДатаОкончания CustomAttributes)

        //  *** End programmer edit section *** (Конкурс.ДатаОкончания CustomAttributes)
        public virtual ICSSoft.STORMNET.UserDataTypes.NullableDateTime ДатаОкончания
        {
            get
            {
                //  *** Start programmer edit section *** (Конкурс.ДатаОкончания Get start)

                //  *** End programmer edit section *** (Конкурс.ДатаОкончания Get start)
                ICSSoft.STORMNET.UserDataTypes.NullableDateTime result = this.fДатаОкончания;
                //  *** Start programmer edit section *** (Конкурс.ДатаОкончания Get end)

                //  *** End programmer edit section *** (Конкурс.ДатаОкончания Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Конкурс.ДатаОкончания Set start)

                //  *** End programmer edit section *** (Конкурс.ДатаОкончания Set start)
                this.fДатаОкончания = value;
                //  *** Start programmer edit section *** (Конкурс.ДатаОкончания Set end)

                //  *** End programmer edit section *** (Конкурс.ДатаОкончания Set end)
            }
        }
        
        /// <summary>
        /// НачалоОценки.
        /// </summary>
        //  *** Start programmer edit section *** (Конкурс.НачалоОценки CustomAttributes)

        //  *** End programmer edit section *** (Конкурс.НачалоОценки CustomAttributes)
        public virtual ICSSoft.STORMNET.UserDataTypes.NullableDateTime НачалоОценки
        {
            get
            {
                //  *** Start programmer edit section *** (Конкурс.НачалоОценки Get start)

                //  *** End programmer edit section *** (Конкурс.НачалоОценки Get start)
                ICSSoft.STORMNET.UserDataTypes.NullableDateTime result = this.fНачалоОценки;
                //  *** Start programmer edit section *** (Конкурс.НачалоОценки Get end)

                //  *** End programmer edit section *** (Конкурс.НачалоОценки Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Конкурс.НачалоОценки Set start)

                //  *** End programmer edit section *** (Конкурс.НачалоОценки Set start)
                this.fНачалоОценки = value;
                //  *** Start programmer edit section *** (Конкурс.НачалоОценки Set end)

                //  *** End programmer edit section *** (Конкурс.НачалоОценки Set end)
            }
        }
        
        /// <summary>
        /// ОкончаниеОценки.
        /// </summary>
        //  *** Start programmer edit section *** (Конкурс.ОкончаниеОценки CustomAttributes)

        //  *** End programmer edit section *** (Конкурс.ОкончаниеОценки CustomAttributes)
        public virtual ICSSoft.STORMNET.UserDataTypes.NullableDateTime ОкончаниеОценки
        {
            get
            {
                //  *** Start programmer edit section *** (Конкурс.ОкончаниеОценки Get start)

                //  *** End programmer edit section *** (Конкурс.ОкончаниеОценки Get start)
                ICSSoft.STORMNET.UserDataTypes.NullableDateTime result = this.fОкончаниеОценки;
                //  *** Start programmer edit section *** (Конкурс.ОкончаниеОценки Get end)

                //  *** End programmer edit section *** (Конкурс.ОкончаниеОценки Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Конкурс.ОкончаниеОценки Set start)

                //  *** End programmer edit section *** (Конкурс.ОкончаниеОценки Set start)
                this.fОкончаниеОценки = value;
                //  *** Start programmer edit section *** (Конкурс.ОкончаниеОценки Set end)

                //  *** End programmer edit section *** (Конкурс.ОкончаниеОценки Set end)
            }
        }
        
        /// <summary>
        /// Состояние.
        /// </summary>
        //  *** Start programmer edit section *** (Конкурс.Состояние CustomAttributes)

        //  *** End programmer edit section *** (Конкурс.Состояние CustomAttributes)
        public virtual NewPlatform.Flexberry.ORM.Tests.Состояние Состояние
        {
            get
            {
                //  *** Start programmer edit section *** (Конкурс.Состояние Get start)

                //  *** End programmer edit section *** (Конкурс.Состояние Get start)
                NewPlatform.Flexberry.ORM.Tests.Состояние result = this.fСостояние;
                //  *** Start programmer edit section *** (Конкурс.Состояние Get end)

                //  *** End programmer edit section *** (Конкурс.Состояние Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Конкурс.Состояние Set start)

                //  *** End programmer edit section *** (Конкурс.Состояние Set start)
                this.fСостояние = value;
                //  *** Start programmer edit section *** (Конкурс.Состояние Set end)

                //  *** End programmer edit section *** (Конкурс.Состояние Set end)
            }
        }
        
        /// <summary>
        /// Конкурс.
        /// </summary>
        //  *** Start programmer edit section *** (Конкурс.Организатор CustomAttributes)

        //  *** End programmer edit section *** (Конкурс.Организатор CustomAttributes)
        [NotNull()]
        public virtual NewPlatform.Flexberry.ORM.Tests.Пользователь Организатор
        {
            get
            {
                //  *** Start programmer edit section *** (Конкурс.Организатор Get start)

                //  *** End programmer edit section *** (Конкурс.Организатор Get start)
                NewPlatform.Flexberry.ORM.Tests.Пользователь result = this.fОрганизатор;
                //  *** Start programmer edit section *** (Конкурс.Организатор Get end)

                //  *** End programmer edit section *** (Конкурс.Организатор Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Конкурс.Организатор Set start)

                //  *** End programmer edit section *** (Конкурс.Организатор Set start)
                this.fОрганизатор = value;
                //  *** Start programmer edit section *** (Конкурс.Организатор Set end)

                //  *** End programmer edit section *** (Конкурс.Организатор Set end)
            }
        }
        
        /// <summary>
        /// Конкурс.
        /// </summary>
        //  *** Start programmer edit section *** (Конкурс.Документы CustomAttributes)

        //  *** End programmer edit section *** (Конкурс.Документы CustomAttributes)
        public virtual NewPlatform.Flexberry.ORM.Tests.DetailArrayOfДокументацияККонкурсу Документы
        {
            get
            {
                //  *** Start programmer edit section *** (Конкурс.Документы Get start)

                //  *** End programmer edit section *** (Конкурс.Документы Get start)
                if ((this.fДокументы == null))
                {
                    this.fДокументы = new NewPlatform.Flexberry.ORM.Tests.DetailArrayOfДокументацияККонкурсу(this);
                }
                NewPlatform.Flexberry.ORM.Tests.DetailArrayOfДокументацияККонкурсу result = this.fДокументы;
                //  *** Start programmer edit section *** (Конкурс.Документы Get end)

                //  *** End programmer edit section *** (Конкурс.Документы Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Конкурс.Документы Set start)

                //  *** End programmer edit section *** (Конкурс.Документы Set start)
                this.fДокументы = value;
                //  *** Start programmer edit section *** (Конкурс.Документы Set end)

                //  *** End programmer edit section *** (Конкурс.Документы Set end)
            }
        }
        
        /// <summary>
        /// Конкурс.
        /// </summary>
        //  *** Start programmer edit section *** (Конкурс.КритерииОценки CustomAttributes)

        //  *** End programmer edit section *** (Конкурс.КритерииОценки CustomAttributes)
        public virtual NewPlatform.Flexberry.ORM.Tests.DetailArrayOfКритерийОценки КритерииОценки
        {
            get
            {
                //  *** Start programmer edit section *** (Конкурс.КритерииОценки Get start)

                //  *** End programmer edit section *** (Конкурс.КритерииОценки Get start)
                if ((this.fКритерииОценки == null))
                {
                    this.fКритерииОценки = new NewPlatform.Flexberry.ORM.Tests.DetailArrayOfКритерийОценки(this);
                }
                NewPlatform.Flexberry.ORM.Tests.DetailArrayOfКритерийОценки result = this.fКритерииОценки;
                //  *** Start programmer edit section *** (Конкурс.КритерииОценки Get end)

                //  *** End programmer edit section *** (Конкурс.КритерииОценки Get end)
                return result;
            }
            set
            {
                //  *** Start programmer edit section *** (Конкурс.КритерииОценки Set start)

                //  *** End programmer edit section *** (Конкурс.КритерииОценки Set start)
                this.fКритерииОценки = value;
                //  *** Start programmer edit section *** (Конкурс.КритерииОценки Set end)

                //  *** End programmer edit section *** (Конкурс.КритерииОценки Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "КонкурсE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View КонкурсE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("КонкурсE", typeof(NewPlatform.Flexberry.ORM.Tests.Конкурс));
                }
            }
        }
    }
}

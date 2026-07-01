namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET;

    [AutoAltered()]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("DateOnlyFieldE", new string[]
    {
        "DateOnlyProp",
        "DateOnlyNullableProp",
        "TimeOnlyProp",
        "TimeOnlyNullableProp",
    })]
    public class DateOnlyField : ICSSoft.STORMNET.DataObject
    {
#if NET6_0_OR_GREATER
        private System.DateOnly fDateOnlyProp;
        private System.DateOnly? fDateOnlyNullableProp;
        private System.TimeOnly fTimeOnlyProp;
        private System.TimeOnly? fTimeOnlyNullableProp;
#endif

        // *** Start programmer edit section *** (DateOnlyField CustomMembers)

        // *** End programmer edit section *** (DateOnlyField CustomMembers)

#if NET6_0_OR_GREATER
        public virtual System.DateOnly DateOnlyProp
        {
            get
            {
                return this.fDateOnlyProp;
            }

            set
            {
                this.fDateOnlyProp = value;
            }
        }

        public virtual System.DateOnly? DateOnlyNullableProp
        {
            get
            {
                return this.fDateOnlyNullableProp;
            }

            set
            {
                this.fDateOnlyNullableProp = value;
            }
        }

        public virtual System.TimeOnly TimeOnlyProp
        {
            get
            {
                return this.fTimeOnlyProp;
            }

            set
            {
                this.fTimeOnlyProp = value;
            }
        }

        public virtual System.TimeOnly? TimeOnlyNullableProp
        {
            get
            {
                return this.fTimeOnlyNullableProp;
            }

            set
            {
                this.fTimeOnlyNullableProp = value;
            }
        }
#endif

        public class Views
        {
            public static ICSSoft.STORMNET.View DateOnlyFieldE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("DateOnlyFieldE", typeof(DateOnlyField));
                }
            }
        }
    }
}

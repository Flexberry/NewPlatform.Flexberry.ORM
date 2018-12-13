using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace ICSSoft.STORMNET.FunctionalLanguage
{
    internal class FunctionForControlsConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            FunctionForControls mf = value as FunctionForControls;
            if (mf != null && destinationType == typeof(InstanceDescriptor))
            {
                MethodInfo ci = value.GetType().GetMethod("Parse", new Type[] { typeof(string), typeof(Type), typeof(string) });
                return new InstanceDescriptor(ci, new object[] { mf.ToString(), mf.View.DefineClassType, mf.View.Name });
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
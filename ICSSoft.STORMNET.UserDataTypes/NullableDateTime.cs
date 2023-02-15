namespace ICSSoft.STORMNET.UserDataTypes
{
    using System;
    using System.Configuration;
    using System.Xml;
    using System.Xml.Schema;

    using ICSSoft.STORMNET.Windows.Forms.Binders;

    /// <summary>
    /// DateTime с поддержкой null (в ту эпоху, когда ещё не было DateTime? у нас уже был этот класс - так и повелось).
    /// </summary>
    [ControlProvider("ICSSoft.STORMNET.UserDataTypes.NullableDateTimeControlProvider, ICSSoft.STORMNET.Windows.Forms")]
    [StoreInstancesInType(typeof(Business.SQLDataService), typeof(DateTime))]
    [Serializable]
    public class NullableDateTime : IFormattable, IComparable, IConvertible, IComparableType, System.Xml.Serialization.IXmlSerializable
    {
        private DateTime fValue;
        private const string DateTimeFormatForXmlSerializable = "dd.MM.yyyy HH:mm:ss";

        private NullableDateTime(DateTime val)
        {
            Value = val;
        }

        /// <summary>
        /// Конструктор без параметров, нужен для Activator.CreateInstance.
        /// </summary>
        public NullableDateTime()
        {
        }

        /// <summary>
        /// Меньше.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <returns>x меньше y.</returns>
        public static bool operator <(NullableDateTime x, NullableDateTime y)
        {
            if ((object)x == null)
            {
                return (object)y != null;
            }

            return x.CompareTo(y) < 0;
        }

        /// <summary>
        /// Меньше либо равно.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <returns>x меньше либо равен y.</returns>
        public static bool operator <=(NullableDateTime x, NullableDateTime y)
        {
            if ((object)x == null)
            {
                return true;
            }

            return x.CompareTo(y) <= 0;
        }

        /// <summary>
        /// Больше.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <returns>x больше y.</returns>
        public static bool operator >(NullableDateTime x, NullableDateTime y)
        {
            if ((object)x == null)
            {
                return false;
            }

            return x.CompareTo(y) > 0;
        }

        /// <summary>
        /// Больше либо равно.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <returns>x больше либо равен y.</returns>
        public static bool operator >=(NullableDateTime x, NullableDateTime y)
        {
            if ((object)x == null)
            {
                return (object)y == null;
            }

            return x.CompareTo(y) >= 0;
        }

        /// <summary>
        /// Равно.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <returns>x == y.</returns>
        public static bool operator ==(NullableDateTime x, NullableDateTime y)
        {
            if ((object)x == null)
            {
                return (object)y == null;
            }

            return x.CompareTo(y) == 0;
        }

        /// <summary>
        /// Не равно.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <returns>x != y.</returns>
        public static bool operator !=(NullableDateTime x, NullableDateTime y)
        {
            if ((object)x == null)
            {
                return (object)y != null;
            }

            return x.CompareTo(y) != 0;
        }

        /// <summary>
        /// Преобразование NullableDateTime в DateTime.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator DateTime(NullableDateTime value)
        {
            return value.Value;
        }

        /// <summary>
        /// Преобразование NullableDateTime в DateTime?.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator DateTime?(NullableDateTime value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Value;
        }

        /// <summary>
        /// Преобразование  DateTime? в NullableDateTime.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator NullableDateTime(DateTime? value)
        {
            if (value == null)
            {
                return null;
            }

            return new NullableDateTime(value.Value);
        }

        /// <summary>
        /// Преобразование DateTime в NullableDateTime.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator NullableDateTime(DateTime value)
        {
            return new NullableDateTime(value);
        }

        /// <summary>
        /// Преобразование DBNull в NullableDateTime.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator NullableDateTime(DBNull value)
        {
            return null;
        }

        /// <summary>
        /// Разбор строки и создание NullableDateTime.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static NullableDateTime Parse(string value)
        {
            if (value == null || value == string.Empty)
            {
                return null;
            }

            DateTime dt;
            if (DateTime.TryParse(value, out dt))
            {
                return new NullableDateTime(dt);
            }

            return null;

            // try
            // {
            //    DateTime dt = DateTime.Parse(value);
            //    return new NullableDateTime(dt);
            // }
            // catch
            // {
            //    return null;
            // }
        }

        /// <summary>
        /// В строку.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // if (format==null)
            string format = System.Configuration.ConfigurationManager.AppSettings["NullableDateTimeDefaultFormat"];
            if (format != null)
            {
                return Value.ToString(format);
            }

            return Value.ToString();
        }

        /// <summary>
        /// Значение.
        /// </summary>
        public virtual DateTime Value
        {
            get
            {
                DateTime result = fValue;
                return result;
            }

            set
            {
                fValue = value;
            }
        }

        /// <summary>
        /// Сегодня (DateTime.Today).
        /// </summary>
        public static NullableDateTime Today
        {
            get
            {
                return (NullableDateTime)DateTime.Today;
            }
        }

        /// <summary>
        /// DateTime.Now.
        /// </summary>
        public static NullableDateTime Now
        {
            get
            {
                return (NullableDateTime)DateTime.Now;
            }
        }

        /// <summary>
        /// DateTime.UtcNow.
        /// </summary>
        public static NullableDateTime UtcNow
        {
            get
            {
                return (NullableDateTime)DateTime.UtcNow;
            }
        }

        /// <summary>
        /// сравнение.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is NullableDateTime)
            {
                return Value.Equals(((NullableDateTime)obj).Value);
            }

            return false;
        }

        public int Compare(object x)
        {
            return CompareTo(x);
        }

        #region IFormattable Members

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
            {
                format = ConfigurationManager.AppSettings["NullableDateTimeDefaultFormat"];
            }

            return Value.ToString(format, formatProvider);
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// сравнение.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            NullableDateTime ndt = (NullableDateTime)obj;

            if (ndt == null)
            {
                return 1;
            }

            return fValue.CompareTo(ndt.Value);
        }

        #endregion

        #region IConvertible Members

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public ulong ToUInt64(IFormatProvider provider)
        {
            return 0;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public sbyte ToSByte(IFormatProvider provider)
        {
            return 0;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public double ToDouble(IFormatProvider provider)
        {
            return 0;
        }

        /// <summary>
        /// преобразование в DateTime.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Value;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public float ToSingle(IFormatProvider provider)
        {
            return 0;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public bool ToBoolean(IFormatProvider provider)
        {
            return false;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public int ToInt32(IFormatProvider provider)
        {
            return 0;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public ushort ToUInt16(IFormatProvider provider)
        {
            return 0;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public short ToInt16(IFormatProvider provider)
        {
            return 0;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        string IConvertible.ToString(IFormatProvider provider)
        {
            return null;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public byte ToByte(IFormatProvider provider)
        {
            return 0;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public char ToChar(IFormatProvider provider)
        {
            return '\0';
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public long ToInt64(IFormatProvider provider)
        {
            return 0;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <returns></returns>
        public TypeCode GetTypeCode()
        {
            return new TypeCode();
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public decimal ToDecimal(IFormatProvider provider)
        {
            return 0;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="conversionType"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return null;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public uint ToUInt32(IFormatProvider provider)
        {
            return 0;
        }

        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// Не реализовано.
        /// </summary>
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            string value = reader.ReadString();

            if (!string.IsNullOrEmpty(value))
            {
                DateTime val;

                if (DateTime.TryParse(value, out val))
                {
                    fValue = val;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(fValue.ToString(DateTimeFormatForXmlSerializable));
        }

        #endregion
    }
}

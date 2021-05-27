using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;

namespace ICSSoft.STORMNET.UserDataTypes
{
    /// <summary>
    /// Int с поддержкой null (в ту эпоху, когда ещё не было int? у нас уже был этот класс - так и повелось).
    /// </summary>
    [ICSSoft.STORMNET.Windows.Forms.Binders.ControlProvider("ICSSoft.STORMNET.UserDataTypes.NullableDecimalControlProvider, ICSSoft.STORMNET.Windows.Forms")]
    [Serializable]
    [ICSSoft.STORMNET.StoreInstancesInType(typeof(ICSSoft.STORMNET.Business.SQLDataService), typeof(int))]
    public class NullableInt : IComparable, IConvertible, System.Xml.Serialization.IXmlSerializable
    {
        private int fValue;

        /// <summary>
        /// Конструктор без параметров, нужен для Activator.CreateInstance.
        /// </summary>
        public NullableInt()
        {
        }

        private NullableInt(int val)
        {
            Value = val;
        }

        /// <summary>
        /// Преобразование возможно только явное, поскольку программист должен отдавать себе отчёт в том, что NullableInt может быть null, поэтому не забывайте проверять перед таким преобразованием.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator int(NullableInt value)
        {
            return value.Value;
        }

        /// <summary>
        /// Неявное преобразование тут можно использовать, поскольку невозможно сломать NullableInt засунув ему int.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator NullableInt(int value)
        {
            return new NullableInt(value);
        }

        /// <summary>
        /// Int64 в NullableInt.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator NullableInt(long value)
        {
            int intValue = (int)Convert.ChangeType(value, typeof(int));

            return new NullableInt(intValue);
        }

        /// <summary>
        /// Преобразование возможно только явное, поскольку программист должен отдавать себе отчёт в том, что NullableInt может быть null, поэтому не забывайте проверять перед таким преобразованием.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator int?(NullableInt value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Value;
        }

        /// <summary>
        /// Неявное преобразование тут можно использовать, поскольку невозможно сломать NullableInt засунув ему int.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator NullableInt(int? value)
        {
            if (value == null)
            {
                return null;
            }

            return new NullableInt(value.Value);
        }

        /// <summary>
        /// DBNull в NullableInt.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator NullableInt(DBNull value)
        {
            return null;
        }

        /// <summary>
        /// Decimal в NullableInt.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator NullableInt(decimal value)
        {
            return new NullableInt((int)value);
        }

        /// <summary>
        /// в строку.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return fValue.ToString();
        }

        /// <summary>
        /// Значение.
        /// </summary>
        public virtual int Value
        {
            get
            {
                int result = this.fValue;
                return result;
            }

            set
            {
                this.fValue = value;
            }
        }

        /// <summary>
        /// Разбор строки и создание NullableInt.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static NullableInt Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            return (NullableInt)int.Parse(s);
        }

        /// <summary>
        /// Разбор строки и создание NullableInt с провайдером формата.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static NullableInt Parse(string s, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            return (NullableInt)decimal.Parse(s, provider);
        }

        /// <summary>
        /// Сравнение.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is NullableInt)
            {
                return this.Value.Equals(((NullableInt)obj).Value);
            }

            return false;
        }

        /// <summary>
        /// ==.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(NullableInt x, NullableInt y)
        {
            return new NullableInt(0).Compare(x, y) == 0;
        }

        /// <summary>
        /// !=.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(NullableInt x, NullableInt y)
        {
            return new NullableInt(0).Compare(x, y) != 0;
        }

        /// <summary>
        /// >.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >(NullableInt x, NullableInt y)
        {
            return new NullableInt(0).Compare(x, y) > 0;
        }

        /// <summary>
        /// >=.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >=(NullableInt x, NullableInt y)
        {
            return new NullableInt(0).Compare(x, y) >= 0;
        }

        /// <summary>
        /// меньше.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <(NullableInt x, NullableInt y)
        {
            return new NullableInt(0).Compare(x, y) < 0;
        }

        /// <summary>
        /// меньше =.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <=(NullableInt x, NullableInt y)
        {
            return new NullableInt(0).Compare(x, y) <= 0;
        }

        /// <summary>
        /// сравнение.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            if ((x == null) && (y == null))
            {
                return 0;
            }

            if ((x == null) && (y != null))
            {
                return -1;
            }

            if ((x != null) && (y == null))
            {
                return 1;
            }

            return decimal.Compare(((NullableInt)x).fValue, ((NullableInt)y).fValue);
        }

        /// <summary>
        /// сравнение.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return Compare(this, obj);
        }

        /// <summary>
        /// -.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int operator -(NullableInt x, NullableInt y)
        {
            return ((x != null) ? x.Value : 0) - ((y != null) ? y.Value : 0);
        }

        /// <summary>
        /// -.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int operator -(int x, NullableInt y)
        {
            return x - ((y != null) ? y.Value : 0);
        }

        /// <summary>
        /// -.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int operator -(NullableInt x, int y)
        {
            return ((x != null) ? x.Value : 0) - y;
        }

        /// <summary>
        /// +.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int operator +(NullableInt x, NullableInt y)
        {
            return ((x != null) ? x.Value : 0) + ((y != null) ? y.Value : 0);
        }

        /// <summary>
        /// +.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int operator +(int x, NullableInt y)
        {
            return x + ((y != null) ? y.Value : 0);
        }

        /// <summary>
        /// +.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int operator +(NullableInt x, int y)
        {
            return ((x != null) ? x.Value : 0) + y;
        }

        #region IConvertible Members

        public ulong ToUInt64(IFormatProvider provider)
        {
            return (ulong)Value;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return (sbyte)Value;
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
        /// Не реализовано.
        /// </summary>
        public DateTime ToDateTime(IFormatProvider provider)
        {
            return new DateTime();
        }

        public float ToSingle(IFormatProvider provider)
        {
            return (float)Value;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        public bool ToBoolean(IFormatProvider provider)
        {
            return false;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Value;
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return (ushort)Value;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return (short)Value;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        string IConvertible.ToString(IFormatProvider provider)
        {
            return null;
        }

        public byte ToByte(IFormatProvider provider)
        {
            return (byte)Value;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        public char ToChar(IFormatProvider provider)
        {
            return '\0';
        }

        public long ToInt64(IFormatProvider provider)
        {
            return (long)Value;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        public TypeCode GetTypeCode()
        {
            return new TypeCode();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return (decimal)Value;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return null;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return (uint)Value;
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
                fValue = Parse(value).Value;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(fValue.ToString());
        }

        #endregion
    }

    /// <summary>
    /// Decimal с поддержкой null (в ту эпоху, когда ещё не было Decimal? у нас уже был этот класс - так и повелось).
    /// </summary>
    [ICSSoft.STORMNET.Windows.Forms.Binders.ControlProvider("ICSSoft.STORMNET.UserDataTypes.NullableDecimalControlProvider, ICSSoft.STORMNET.Windows.Forms")]
    [Serializable]
    [ICSSoft.STORMNET.StoreInstancesInType(typeof(ICSSoft.STORMNET.Business.SQLDataService), typeof(decimal))]
    public class NullableDecimal : IComparable, IComparableType, IConvertible, System.Xml.Serialization.IXmlSerializable
    {
        private decimal fValue;

        /// <summary>
        /// Конструктор без параметров, нужен для Activator.CreateInstance.
        /// </summary>
        public NullableDecimal()
        {
        }

        private NullableDecimal(decimal val)
        {
            Value = val;
        }

        /// <summary>
        /// Явное преобразование к Decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator decimal(NullableDecimal value)
        {
            return value.Value;
        }

        /// <summary>
        /// Явное преобразование Decimal к NullableDecimal.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator NullableDecimal(decimal value)
        {
            return new NullableDecimal(value);
        }

        /// <summary>
        /// Явное преобразование к Decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator decimal?(NullableDecimal value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Value;
        }

        /// <summary>
        /// Явное преобразование Decimal к NullableDecimal.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator NullableDecimal(decimal? value)
        {
            if (value == null)
            {
                return null;
            }

            return new NullableDecimal(value.Value);
        }

        /// <summary>
        /// Явное преобразование DBNull к NullableDecimal.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator NullableDecimal(DBNull value)
        {
            return null;
        }

        /// <summary>
        /// ==.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(NullableDecimal x, NullableDecimal y)
        {
            return new NullableDecimal(0).Compare(x, y) == 0;
        }

        /// <summary>
        /// !=.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(NullableDecimal x, NullableDecimal y)
        {
            return new NullableDecimal(0).Compare(x, y) != 0;
        }

        /// <summary>
        /// >.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >(NullableDecimal x, NullableDecimal y)
        {
            return new NullableDecimal(0).Compare(x, y) > 0;
        }

        /// <summary>
        /// >=.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator >=(NullableDecimal x, NullableDecimal y)
        {
            return new NullableDecimal(0).Compare(x, y) >= 0;
        }

        /// <summary>
        /// меньше.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <(NullableDecimal x, NullableDecimal y)
        {
            return new NullableDecimal(0).Compare(x, y) < 0;
        }

        /// <summary>
        /// меньше =.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator <=(NullableDecimal x, NullableDecimal y)
        {
            return new NullableDecimal(0).Compare(x, y) <= 0;
        }

        /// <summary>
        /// В строку.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return fValue.ToString();
        }

        /// <summary>
        /// Собственно значение.
        /// </summary>
        public virtual decimal Value
        {
            get
            {
                decimal result = this.fValue;
                return result;
            }

            set
            {
                this.fValue = value;
            }
        }

        /// <summary>
        /// Разобрать и получить NullableDecimal из строки.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static NullableDecimal Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            return (NullableDecimal)decimal.Parse(s);
        }

        /// <summary>
        /// Разобрать и получить NullableDecimal из строки с провайдером формата.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static NullableDecimal Parse(string s, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            return (NullableDecimal)decimal.Parse(s, provider);
        }

        /// <summary>
        /// Разобрать и получить NullableDecimal из строки.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out NullableDecimal result)
        {
            decimal res;
            bool parsed = decimal.TryParse(s, out res);
            result = parsed ? (NullableDecimal)res : null;
            return parsed;
        }

        /// <summary>
        /// Разобрать и получить NullableDecimal из строки с провайдером формата.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="styles"></param>
        /// <param name="provider"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string s, NumberStyles styles, IFormatProvider provider, out NullableDecimal result)
        {
            decimal res;
            bool parsed = decimal.TryParse(s, styles, provider, out res);
            result = parsed ? (NullableDecimal)res : null;
            return parsed;
        }

        /// <summary>
        /// Сравнение.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is NullableDecimal)
            {
                return this.Value.Equals(((NullableDecimal)obj).Value);
            }
            else
            {
                return false;
            }
        }

        public int Compare(object x)
        {
            return CompareTo(x);
        }

        /// <summary>
        /// Сравнение.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (!(obj is NullableDecimal))
            {
                throw new ArgumentException();
            }

            if (obj == null)
            {
                return 1;
            }

            return Value.CompareTo(((NullableDecimal)obj).Value);
        }

        /// <summary>
        /// сравнение.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            if ((x == null) && (y == null))
            {
                return 0;
            }

            if ((x == null) && (y != null))
            {
                return -1;
            }

            if ((x != null) && (y == null))
            {
                return 1;
            }

            return decimal.Compare(((NullableDecimal)x).fValue, ((NullableDecimal)y).fValue);
        }

        /// <summary>
        /// -.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static decimal operator -(NullableDecimal x, NullableDecimal y)
        {
            return ((x != null) ? x.Value : 0) - ((y != null) ? y.Value : 0);
        }

        /// <summary>
        /// -.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static decimal operator -(decimal x, NullableDecimal y)
        {
            return x - ((y != null) ? y.Value : 0);
        }

        /// <summary>
        /// -.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static decimal operator -(NullableDecimal x, decimal y)
        {
            return ((x != null) ? x.Value : 0) - y;
        }

        /// <summary>
        /// +.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static decimal operator +(NullableDecimal x, NullableDecimal y)
        {
            return ((x != null) ? x.Value : 0) + ((y != null) ? y.Value : 0);
        }

        /// <summary>
        /// +.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static decimal operator +(decimal x, NullableDecimal y)
        {
            return x + ((y != null) ? y.Value : 0);
        }

        /// <summary>
        /// +.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static decimal operator +(NullableDecimal x, decimal y)
        {
            return ((x != null) ? x.Value : 0) + y;
        }

        #region IConvertible Members

        public ulong ToUInt64(IFormatProvider provider)
        {
            return (ulong)Value;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return (sbyte)Value;
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
        /// Не реализовано.
        /// </summary>
        public DateTime ToDateTime(IFormatProvider provider)
        {
            return new DateTime();
        }

        public float ToSingle(IFormatProvider provider)
        {
            return (float)Value;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        public bool ToBoolean(IFormatProvider provider)
        {
            return false;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return (int)Value;
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return (ushort)Value;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return (short)Value;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        string IConvertible.ToString(IFormatProvider provider)
        {
            return null;
        }

        public byte ToByte(IFormatProvider provider)
        {
            return (byte)Value;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        public char ToChar(IFormatProvider provider)
        {
            return '\0';
        }

        public long ToInt64(IFormatProvider provider)
        {
            return (long)Value;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        public TypeCode GetTypeCode()
        {
            return new TypeCode();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Value;
        }

        /// <summary>
        /// Не реализовано.
        /// </summary>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return null;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return (uint)Value;
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
                fValue = Parse(value).Value;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(fValue.ToString());
        }

        #endregion
    }
}

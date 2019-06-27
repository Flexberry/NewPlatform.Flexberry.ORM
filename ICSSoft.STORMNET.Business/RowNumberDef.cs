namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Структура для указания начала и конца страницы для вычитки
    /// </summary>
    [Serializable]
    public class RowNumberDef : ISerializable
    {
        private int _startRow;
        private int _endRow;


        /// <summary>
        /// Со строчки номер
        /// </summary>
        public int StartRow
        {
            get { return _startRow; }
            set { _startRow = value; }
        }

        /// <summary>
        /// По строчку номер
        /// </summary>
        public int EndRow
        {
            get { return _endRow; }
            set { _endRow = value; }
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("StartRow", _startRow);
            info.AddValue("EndRow", _endRow);
        }



        #endregion

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public RowNumberDef()
        {
        }




        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="StartRow">Загрузим с объекта (включительно)</param>
        /// <param name="EndRow">Загрузим до объекта (включительно)</param>
        public RowNumberDef(int StartRow, int EndRow)
        {
            _startRow = StartRow;
            _endRow = EndRow;
        }

        /// <summary>
        /// Десереализация
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public RowNumberDef(SerializationInfo info, StreamingContext context)
        {
            _startRow = info.GetInt32("StartRow");
            _endRow = info.GetInt32("EndRow");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(RowNumberDef))
            {
                return false;
            }

            return Equals((RowNumberDef)obj);
        }

        public bool Equals(RowNumberDef other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other._startRow == _startRow && other._endRow == _endRow;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_startRow * 397) ^ _endRow;
            }
        }
    }
}
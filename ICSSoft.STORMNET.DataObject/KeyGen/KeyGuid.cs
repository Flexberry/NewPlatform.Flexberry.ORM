namespace ICSSoft.STORMNET.KeyGen
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Guid, отличающийся от стандартного наличием метода Parse
    /// и отсутствием половины ненужных конструкторов.
    /// Кому надо другой конструктор -- передайте в конструктор KeyGuid Guid,
    /// созданный его собственным конструктором.
    /// </summary>
    [Serializable]
    [StoreInstancesInType("ICSSoft.STORMNET.Business.SQLDataService, ICSSoft.STORMNET.Business", "System.Guid")]
    [DataContract]
    public class KeyGuid : IComparable
    {
        /// <summary>
        /// Значение ключевого поля.
        /// </summary>
        protected Guid guid;

        /// <summary>
        /// Создать новый уникальный идентификатор. Равносильно. <code>Guid.NewGuid()</code>.
        /// </summary>
        public KeyGuid()
        {
            guid = Guid.NewGuid();
        }

        /// <summary>
        /// Создать новый уникальный идентификатор по образцу.
        /// </summary>
        /// <param name="guid"></param>
        public KeyGuid(Guid guid)
        {
            this.guid = guid;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="guid"></param>
        public KeyGuid(string guid)
        {
            this.guid = new Guid(guid);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="guid"></param>
        public KeyGuid(byte[] guid)
        {
            this.guid = new Guid(guid);
        }

        /// <summary>
        /// преобрзовать string->keyGuid.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static KeyGuid Parse(string guid)
        {
            if (!string.IsNullOrEmpty(guid))
            {
                return new KeyGuid(guid);
            }

            return null;
        }

        public static bool operator <(KeyGuid x, KeyGuid y)
        {
            if ((object)x == null)
            {
                return (object)y != null;
            }

            return x.CompareTo(y) < 0;
        }

        public static bool operator >(KeyGuid x, KeyGuid y)
        {
            if ((object)x == null)
            {
                return false;
            }

            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(KeyGuid x, KeyGuid y)
        {
            if ((object)x == null)
            {
                return true;
            }

            return x.CompareTo(y) <= 0;
        }

        public static bool operator >=(KeyGuid x, KeyGuid y)
        {
            if ((object)x == null)
            {
                return (object)y == null;
            }

            return x.CompareTo(y) >= 0;
        }

        private static readonly System.Text.RegularExpressions.Regex GuidPattern1 = new System.Text.RegularExpressions.Regex(@"^[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}$", System.Text.RegularExpressions.RegexOptions.Compiled);

        private static readonly System.Text.RegularExpressions.Regex GuidPattern2 = new System.Text.RegularExpressions.Regex(@"^\{[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}\}$", System.Text.RegularExpressions.RegexOptions.Compiled);

        /// <summary>
        /// Проверка на гуидность.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsGuid(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            return GuidPattern1.IsMatch(input) || GuidPattern2.IsMatch(input);
        }

        /// <summary>
        /// генерация нового KeyGuid.
        /// </summary>
        /// <returns></returns>
        public static KeyGuid NewGuid()
        {
            return new KeyGuid(Guid.NewGuid());
        }

        /// <summary>
        ///
        /// </summary>
        [DataMember]
        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        #region IComparable

        /// <summary>
        /// Метод сравнения KeyGuid.
        /// </summary>
        /// <param name="obj">KeyGuid с которым сравниваем.</param>
        /// <returns>Результат сравнения см. Guid.CompareTo(o).</returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentException("obj");
            }

            return guid.CompareTo(((KeyGuid)obj).Guid);
        }
        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return guid.ToString("B");
        }

        /// <summary>
        /// Неявно преобразовать из Guid в KeyGuid.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator KeyGuid(Guid value)
        {
            return new KeyGuid(value);
        }

        /// <summary>
        /// Неявно преобразовать из KeyGuid в Guid.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Guid(KeyGuid value)
        {
            return value.Guid;
        }

        /// <summary>
        /// Неявно преобразовать из Guid? в KeyGuid.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator KeyGuid(Guid? value)
        {
            return value == null ? null : new KeyGuid(value.Value);
        }

        /// <summary>
        /// Неявно преобразовать из KeyGuid в Guid?.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Guid?(KeyGuid value)
        {
            return value == null ? null : new Guid?(value.Guid);
        }

        /// <summary>
        /// Неявно преобразовать из String в KeyGuid.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator KeyGuid(string value)
        {
            return new KeyGuid(value);
        }

        /// <summary>
        /// Неявно преобразовать из KeyGuid в string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator string(KeyGuid value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Неявно преобразовать из String в KeyGuid.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator KeyGuid(byte[] value)
        {
            return new KeyGuid(value);
        }

        /// <summary>
        /// Неявно преобразовать из KeyGuid в string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator byte[](KeyGuid value)
        {
            return value.Guid.ToByteArray();
        }

        /// <summary>
        /// Сравнение инстанций. Сравнение происходит по значению <see cref="Guid"/>.
        /// </summary>
        /// <param name="obj">Объект для сравнения.</param>
        /// <returns>Результат сравнения объектов. Если равны, то <c>true</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is KeyGuid)
            {
                return (obj as KeyGuid).Guid.Equals(Guid);
            }

            return false;
        }

        /// <summary>
        /// Получить значение хэш-функции. Результат напрямую зависит от значения <see cref="Guid"/>. Если в процессе жизни инстанции этого объекта кто-то поменяет <see cref="Guid"/>, то и этот метод будет возвращать уже другое значение.
        /// </summary>
        /// <returns>Значение хэш-функции.</returns>
        public override int GetHashCode()
        {
            return guid.GetHashCode();
        }

        #region operators == и !=

        /// <summary>
        /// ==.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(KeyGuid x, KeyGuid y)
        {
            if ((object)x == null && (object)y == null)
            {
                return true;
            }

            if ((object)x != null && (object)y != null)
            {
                return x.Equals(y);
            }

            return false;
        }

        /// <summary>
        /// !=.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(KeyGuid x, KeyGuid y)
        {
            return !(x == y);
        }

        #endregion
    }
}

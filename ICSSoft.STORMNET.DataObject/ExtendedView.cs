namespace ICSSoft.STORMNET
{
    using System;
    using System.Collections;
    using System.Runtime.Serialization;

    /// <summary>
    /// Класс для хранения собственных свойств, мастеров и детейлов представления + псевдодетейлов с учётом их порядка.
    /// </summary>
    [Serializable]
    public class ExtendedView : ISerializable
    {
        /// <summary>
        /// Получение представления, на базе которого сформирован ExtendedView.
        /// </summary>
        public View View
        {
            get
            {
                return Information.GetView(ViewName, DefineClassType);
            }
        }

        /// <summary>
        /// Имя представления.
        /// </summary>
        public string ViewName { get; private set; }

        /// <summary>
        /// Тип, для которого задано представление.
        /// </summary>
        public Type DefineClassType { get; private set; }

        /// <summary>
        /// Упорядоченный список собственных свойств, мастеров и детейлов представления + псевдодетейлов.
        /// </summary>
        public ArrayList ViewPropertiesOrderedList { get; private set; }

        /// <summary>
        /// Неявное преобразование из View в ExtendedView.
        /// </summary>
        /// <param name="view"> Представление. </param>
        /// <returns> ExtendedView с автоматически сформированным order-лист. </returns>
        public static implicit operator ExtendedView(View view)
        {
            return new ExtendedView(view);
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="view"> Представление, откуда будут сохранены имя, тип и, если упорядоченный список не задан, то его свойства и детейлы будут добавлены в упорядоченный список. </param>
        /// <param name="viewPropertiesOrderedList"> Упорядоченный список. </param>
        public ExtendedView(View view, ArrayList viewPropertiesOrderedList = null)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            ViewName = view.Name;
            DefineClassType = view.DefineClassType;

            if (view.Name == null 
                || string.IsNullOrEmpty(view.Name.Trim())
                || !Information.CheckViewForClasses(view.Name, new Type[] {DefineClassType}))
            {
                throw new ArgumentException("Передаваемое представление должно быть сохранено в классе.");
            }

            if (viewPropertiesOrderedList == null)
            {
                ViewPropertiesOrderedList = new ArrayList();
                if (view.Properties != null)
                {
                    ViewPropertiesOrderedList.AddRange(view.Properties);
                }

                if (view.Details != null)
                {
                    ViewPropertiesOrderedList.AddRange(view.Details);
                }
            }
            else
            {
                ViewPropertiesOrderedList = viewPropertiesOrderedList;
            }
        }

        /// <summary>
        /// Конструктор класса при десериализации.
        /// </summary>
        /// <param name="info"> Сериализованные данные. </param>
        /// <param name="context"> Контекст сериализации. </param>
        public ExtendedView(SerializationInfo info, StreamingContext context)
        {
            ViewName = info.GetString("ViewName");
            DefineClassType = (Type)info.GetValue("DefineClassType", typeof(Type));
            ViewPropertiesOrderedList = (ArrayList)info.GetValue("ViewPropertiesOrderedList", typeof(ArrayList));
        }

        /// <summary>
        /// Метод, сериализующий данный объект.
        /// </summary>
        /// <param name="info"> Сериализованные данные. </param>
        /// <param name="context"> Контекст сериализации. </param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ViewName", ViewName);
            info.AddValue("DefineClassType", DefineClassType);
            info.AddValue("ViewPropertiesOrderedList", ViewPropertiesOrderedList);
        }
    }
}

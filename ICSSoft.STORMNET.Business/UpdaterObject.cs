using System;
using ICSSoft.STORMNET;
using ICSSoft.STORMNET.FunctionalLanguage;

namespace ICSSoft.STORMNET.Business
{
    /// <summary>
    /// Спецкласс, предназначенный для выполнения групповых операций
    /// Update или Delete в источнике данных.
    /// </summary>
    [NotStored]
    public class UpdaterObject : DataObject
    {
        private DataObject fTemplateObject;
        private Function fFunction;

        /// <summary>
        /// Шаблонный объект данных. Если его статус Altered, тогда будет групповое
        /// обновление объектов, все изменённые атрибуты будут изменены в группе.
        /// Если статус Deleted, то просто будет групповое удаление по условию.
        /// </summary>
        [NotStored]
        public DataObject TemplateObject
        {
            get
            {
                return fTemplateObject;
            }

            set
            {
                fTemplateObject = value;
            }
        }

        /// <summary>
        /// Функция ограничения, в соответствии с которой происходит групповое
        /// изменение объектов или удаление.
        /// </summary>
        [NotStored]
        public Function Function
        {
            get
            {
                return fFunction;
            }

            set
            {
                fFunction = value;
            }
        }

        public UpdaterObject()
        {
        }

        /// <summary>
        /// Конструктор шаблонного "изменятеля".
        /// </summary>
        /// <param name="fTemplateObject">Объект-шаблон.</param>
        /// <param name="fFunction">Функция условия, по которому проводить удаление или обновление.</param>
        public UpdaterObject(DataObject fTemplateObject, Function fFunction)
        {
            if (fFunction == null)
            {
                throw new Exception("fFunction parameter can't be null!");
            }

            this.fTemplateObject = fTemplateObject;
            this.fFunction = fFunction;
        }

        /*
        public override ObjectStatus GetStatus()
        {
            return this.GetStatus(true);
        }

        public override ObjectStatus GetStatus(bool RecountIfAutoaltered)
        {
            ObjectStatus status = TemplateObject.GetStatus();
            if (status == ObjectStatus.Created)//Всё равно будет альтеред
            {
                status = ObjectStatus.Altered;
            }
            return status;

        }*/

        // public override string[] GetAlteredPropertyNames()
        //      {
        //          return this.GetAlteredPropertyNames (true);
        //      }
        //
        //      public override string[] GetAlteredPropertyNames(bool Recount)
        //      {
        //          return TemplateObject.GetAlteredPropertyNames(true);
        //      }
    }
}

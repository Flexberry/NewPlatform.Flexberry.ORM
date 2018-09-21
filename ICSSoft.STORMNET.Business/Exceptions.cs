using System;
using System.Runtime.Serialization;

namespace ICSSoft.STORMNET.Exceptions
{
    /// <summary>
    /// Исключительная ситуация, возникающая при пустом значении в свойстве, которое должно быть заполнено
    /// </summary>
    [ Serializable ]
    public class PropertyCouldnotBeNullException:Exception, ISerializable
    {
        /// <summary>
        /// Имя свойства
        /// </summary>
        public string propName ="";

        /// <summary>
        /// объект
        /// </summary>
        public DataObject dataobject = null;

        /// <summary>
        /// Исключительная ситуация, возникающая при пустом значении в свойстве, которое должно быть заполнено
        /// </summary>
        /// <param name="prop">свойство</param>
        /// <param name="obj">объект</param>
        public PropertyCouldnotBeNullException(string prop,DataObject obj)
        {
            propName = prop;
            dataobject = obj;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public PropertyCouldnotBeNullException( SerializationInfo info, StreamingContext context )
        {
            propName = ( string )info.GetValue( "propName", typeof( string ) );

            dataobject = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue( "propName", this.propName );
        }

        public override string Message
        {
            get
            {
                return string.Format("Property '{0}' could not be NULL in {1}",propName,dataobject.GetType().Name);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    [ Serializable ]
    public class DataServiceNotFoundException:Exception, ISerializable
    {
        /// <summary>
        ///
        /// </summary>
        public DataServiceNotFoundException(){}

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public DataServiceNotFoundException( SerializationInfo info, StreamingContext context )
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }

    /// <summary>
    ///
    /// </summary>
    [ Serializable ]
    public class ObjectNotAlteredException:Exception, ISerializable
    {
        /// <summary>
        ///
        /// </summary>
        public ObjectNotAlteredException(){}

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ObjectNotAlteredException( SerializationInfo info, StreamingContext context )
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }

    /// <summary>
    /// Все изменяемые поля должны быть первоначально загружены(исключительная ситуация)
    /// </summary>
    [ Serializable ]
    public class CantUpdateNotLoadedPropertiesException:Exception, ISerializable
    {
        /// <summary>
        /// объект данных
        /// </summary>
        public ICSSoft.STORMNET.DataObject dobject;

        /// <summary>
        /// свойства
        /// </summary>
        public string[] props;

        /// <summary>
        ///
        /// </summary>
        /// <param name="aDataObject"></param>
        /// <param name="aProps"></param>
        public CantUpdateNotLoadedPropertiesException(ICSSoft.STORMNET.DataObject aDataObject,params string[] aProps)
        {
            dobject = aDataObject;
            props = aProps;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aDataObject"></param>
        /// <param name="aProps"></param>
        public CantUpdateNotLoadedPropertiesException(ICSSoft.STORMNET.DataObject aDataObject,System.Collections.Specialized.StringCollection aProps)
        {
            dobject = aDataObject;
            props = new string[aProps.Count];
            aProps.CopyTo(props,0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public CantUpdateNotLoadedPropertiesException( SerializationInfo info, StreamingContext context )
        {
            dobject = null;
            props = ( string[] )info.GetValue( "props", typeof( string[] ) );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue( "props", props );
        }

        public override string Message
        {
            get
            {
                return string.Format( "Can't update object {0}. There are not loaded properties :{1} ",
                    dobject.GetType().Name,string.Join(",",props));
            }
        }
    }

    /// <summary>
    /// Исключение, которое возникает при отсутствии в хранилище данных объекта
    /// </summary>
    [ Serializable ]
    public class CantFindDataObjectException:Exception, ISerializable
    {
        private System.Type type;
        private object key;

        /// <summary>
        ///
        /// </summary>
        /// <param name="doType">тип объекта</param>
        /// <param name="doKey">ключ объекта</param>
        public CantFindDataObjectException(System.Type doType,object doKey){type=doType;key=doKey;}

        /// <summary>
        /// тип объекта
        /// </summary>
        public System.Type DataObjectType {get {return type;}}

        /// <summary>
        /// ключ объекта
        /// </summary>
        public object DataObjectKey {get {return key;}}

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public CantFindDataObjectException( SerializationInfo info, StreamingContext context )
        {
            type = ( Type )info.GetValue( "type", typeof( Type ) );
        }

        public override string Message
        {
            get
            {
                 return "Невозможно найти объект " + type.FullName + " с ключом " + key.ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue( "type", type );
        }
    }
}
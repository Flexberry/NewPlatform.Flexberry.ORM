// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataObjectsEventArgs.cs" company="IIS">
//   Copyright (c) IIS. All rights reserved.
// </copyright>
// <summary>
//   Аргументы события с массивом объектов данных
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICSSoft.STORMNET.Business
{
    using System;

    /// <summary>
    /// Аргументы события с массивом объектов данных
    /// Для передачи данных возможно использовать свойство Tag
    /// </summary>
    public class DataObjectsEventArgs : EventArgs
    {
        /// <summary>
        /// Поле для передачи данных
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Массив объектов данных
        /// </summary>
        public DataObject[] DataObjects;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataObjects">
        /// </param>
        public DataObjectsEventArgs(DataObject[] dataObjects)
        {
            this.DataObjects = dataObjects;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataObjects">
        /// </param>
        public DataObjectsEventArgs(DataObject[] dataObjects, object tag)
        {
            this.DataObjects = dataObjects;
            Tag = tag;
        }
    }
}
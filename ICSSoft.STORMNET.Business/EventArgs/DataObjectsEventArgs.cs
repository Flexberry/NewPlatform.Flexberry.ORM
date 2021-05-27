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
    /// Аргументы события с массивом объектов данных.
    /// </summary>
    public class DataObjectsEventArgs : EventArgs
    {
        /// <summary>
        /// Массив объектов данных.
        /// </summary>
        public DataObject[] DataObjects;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dataObjects">
        /// </param>
        public DataObjectsEventArgs(DataObject[] dataObjects)
        {
            this.DataObjects = dataObjects;
        }
    }
}
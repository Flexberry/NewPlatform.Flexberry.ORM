// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenerateSQLSelectQueryEventArgs.cs" company="IIS">
//   Copyright (c) IIS. All rights reserved.
// </copyright>
// <summary>
//   Аргумент события при генерации SQL Select запроса
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICSSoft.STORMNET.Business
{
    using System;

    /// <summary>
    /// Аргумент события при генерации SQL Select запроса
    /// </summary>
    public class GenerateSQLSelectQueryEventArgs : EventArgs
    {
        /// <summary>
        /// Структура настройки сервиса данных <see cref="LoadingCustomizationStruct"/>
        /// </summary>
        public LoadingCustomizationStruct CustomizationStruct = null;

        /// <summary>
        /// Структура загрузки <see cref="ICSSoft.STORMNET.Business.StorageStructForView"/>
        /// </summary>
        public StorageStructForView[] StorageStruct = null;
        
        /// <summary>
        /// Запрос
        /// </summary>
        public string GeneratedQuery = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateSQLSelectQueryEventArgs"/> class.
        /// </summary>
        /// <param name="customizationStruct">
        /// The customization struct.
        /// </param>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="StorageStruct">
        /// The storage struct.
        /// </param>
        public GenerateSQLSelectQueryEventArgs(
            LoadingCustomizationStruct customizationStruct, string query, StorageStructForView[] StorageStruct)
        {
            this.CustomizationStruct = customizationStruct;
            this.GeneratedQuery = query;
            this.StorageStruct = StorageStruct;
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateCommandEventArgs.cs" company="IIS">
//   Copyright (c) IIS. All rights reserved.
// </copyright>
// <summary>
//   Аргументы события создания команды
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ICSSoft.STORMNET.Business
{
    using System;

    /// <summary>
    /// Аргументы события создания команды.
    /// </summary>
    public class CreateCommandEventArgs : EventArgs
    {
        /// <summary>
        /// Команда.
        /// </summary>
        public System.Data.IDbCommand Command = null;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="Cmnd">Команда.</param>
        public CreateCommandEventArgs(System.Data.IDbCommand Cmnd)
        {
            Command = Cmnd;
        }
    }
}

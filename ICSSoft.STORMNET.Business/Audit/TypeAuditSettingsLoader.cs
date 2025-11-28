namespace ICSSoft.STORMNET.Business.Audit
{
    using System;

    using ICSSoft.STORMNET.Business.Audit.Exceptions;
    using ICSSoft.STORMNET.Business.Audit.Objects;

    /// <summary>
    /// Base class for loading audit settings for types.
    /// </summary>
    public abstract class TypeAuditSettingsLoader
    {
        /// <summary>
        /// Determines whether the specified data object type has audit settings.
        /// </summary>
        /// <remarks>Doesn't throw exceptions if settings don't exist at all.</remarks>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <returns>Returns <c>true</c> if data object type has audit settings.</returns>
        public abstract bool HasSettings(Type dataObjectType);

        /// <summary>
        /// Determines whether audit is enabled for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>Returns <c>true</c> if audit is enabled.</returns>
        public abstract bool IsAuditEnabled(Type dataObjectType);

        /// <summary>
        /// Determines whether audit is enabled for the specified data object type and audit operation.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <param name="auditOperation">The audit operation.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>Returns <c>true</c> if audit is enabled.</returns>
        public abstract bool IsAuditEnabled(Type dataObjectType, tTypeOfAuditOperation auditOperation);

        /// <summary>
        /// Determines whether it's need to use default audit view.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>Returns <c>true</c> if it's need to use default audit view.</returns>
        public abstract bool UseDefaultAuditView(Type dataObjectType);

        /// <summary>
        /// Gets the audit connection string for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>Audit connection string for the specified data object type.</returns>
        public abstract string GetAuditConnectionString(Type dataObjectType);

        /// <summary>
        /// Gets the audit service for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>Audit service for the specified data object type or <c>null</c> if it doesn't specified.</returns>
        public abstract IAudit GetAuditService(Type dataObjectType);

        /// <summary>
        /// Gets the audit write mode for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>The audit write mode for the specified data object type.</returns>
        public abstract tWriteMode GetAuditWriteMode(Type dataObjectType);

        /// <summary>
        /// Gets the name of the audit view for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <param name="auditOperation">The audit operation.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>The name of the audit view for the specified data object type.</returns>
        public abstract string GetAuditViewName(Type dataObjectType, tTypeOfAuditOperation auditOperation);

        /// <summary>
        /// Gets the audit view for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <param name="auditOperation">The audit operation.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <exception cref="DataNotFoundAuditException">Thrown when view doesn't exist.</exception>
        /// <returns>The audit view for the specified data object type.</returns>
        public abstract View GetAuditView(Type dataObjectType, tTypeOfAuditOperation auditOperation);
    }
}
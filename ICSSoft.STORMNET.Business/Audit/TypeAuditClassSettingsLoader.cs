namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using ICSSoft.STORMNET.Business.Audit.Exceptions;
    using ICSSoft.STORMNET.Business.Audit.Objects;

    /// <summary>
    /// Implementation of <see cref="TypeAuditClassSettingsLoader"/> that uses internal class
    /// with audit settings in it.
    /// </summary>
    /// <seealso cref="TypeAuditSettingsLoader" />
    internal class TypeAuditClassSettingsLoader : TypeAuditSettingsLoader
    {
        /// <summary>
        /// Determines whether the specified data object type has audit settings.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <returns>Returns <c>true</c> if data object type has audit settings.</returns>
        /// <remarks>Doesn't throw exceptions if settings don't exist at all.</remarks>
        public override bool HasSettings(Type dataObjectType)
        {
            return GetAuditSettingsType(dataObjectType) != null;
        }

        /// <summary>
        /// Determines whether audit is enabled for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>Returns <c>true</c> if audit is enabled.</returns>
        public override bool IsAuditEnabled(Type dataObjectType)
        {
            return (bool)GetAuditSettingValue(dataObjectType, AuditConstants.AuditEnabledFieldName);
        }

        /// <summary>
        /// Determines whether audit is enabled for the specified data object type and audit operation.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <param name="auditOperation">The audit operation.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>Returns <c>true</c> if audit is enabled.</returns>
        public override bool IsAuditEnabled(Type dataObjectType, tTypeOfAuditOperation auditOperation)
        {
            return (bool)GetAuditSettingValue(dataObjectType, _settingsClassPropNames[auditOperation].EnabledField);
        }

        /// <summary>
        /// Determines whether it's need to use default audit view.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>Returns <c>true</c> if it's need to use default audit view.</returns>
        public override bool UseDefaultAuditView(Type dataObjectType)
        {
            return (bool)GetAuditSettingValue(dataObjectType, AuditConstants.UseDefaultViewFieldName);
        }

        /// <summary>
        /// Gets the audit connection string for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>Audit connection string for the specified data object type.</returns>
        public override string GetAuditConnectionString(Type dataObjectType)
        {
            return (string)GetAuditSettingValue(dataObjectType, AuditConstants.AuditClassConnectionStringNameFieldName, false);
        }

        /// <summary>
        /// Gets the audit service for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>Audit service for the specified data object type or <c>null</c> if it doesn't specified.</returns>
        public override IAudit GetAuditService(Type dataObjectType)
        {
            return (IAudit)GetAuditSettingValue(dataObjectType, AuditConstants.AuditClassServiceFieldName, false);
        }

        /// <summary>
        /// Gets the audit write mode for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>The audit write mode for the specified data object type.</returns>
        public override tWriteMode GetAuditWriteMode(Type dataObjectType)
        {
            return (tWriteMode)Enum.Parse(typeof(tWriteMode), GetAuditSettingValue(dataObjectType, AuditConstants.WriteModeFieldName).ToString());
        }

        /// <summary>
        /// Gets the name of the audit view for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <param name="auditOperation">The audit operation.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <returns>The name of the audit view for the specified data object type.</returns>
        public override string GetAuditViewName(Type dataObjectType, tTypeOfAuditOperation auditOperation)
        {
            return GetViewName(dataObjectType, _settingsClassPropNames[auditOperation].ViewNameField);
        }

        /// <summary>
        /// Gets the audit view for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <param name="auditOperation">The audit operation.</param>
        /// <exception cref="InvalidOperationException">Thrown when settings don't exist at all.</exception>
        /// <exception cref="DataNotFoundAuditException">Thrown when view doesn't exist.</exception>
        /// <returns>The audit view for the specified data object type or <c>null</c> if it doesn't exist.</returns>
        public override View GetAuditView(Type dataObjectType, tTypeOfAuditOperation auditOperation)
        {
            string viewName = GetViewName(dataObjectType, _settingsClassPropNames[auditOperation].ViewNameField);
            View view = Information.GetView(viewName, dataObjectType);
            if (view == null)
            {
                throw new DataNotFoundAuditException($"В классе {dataObjectType} не найдено представление {viewName}");
            }

            return view;
        }

        private string GetViewName(Type dataObjectType, string propertyName)
        {
            return UseDefaultAuditView(dataObjectType)
                ? AuditConstants.DefaultAuditViewName
                : GetAuditSettingValue(dataObjectType, propertyName).ToString();
        }

        /// <summary>
        /// Internal structure for internal class with audit settings.
        /// </summary>
        private struct SettingsClassDef
        {
            /// <summary>
            /// The view name field.
            /// </summary>
            public readonly string ViewNameField;

            /// <summary>
            /// The enabled field.
            /// </summary>
            public readonly string EnabledField;

            /// <summary>
            /// Initializes a new instance of the <see cref="SettingsClassDef"/> struct.
            /// </summary>
            /// <param name="viewNameField">The view name field.</param>
            /// <param name="enbaledFiled">The enabled filed.</param>
            public SettingsClassDef(string viewNameField, string enbaledFiled)
            {
                ViewNameField = viewNameField;
                EnabledField = enbaledFiled;
            }
        }

        /// <summary>
        /// The names of properties in audit settings class for audit operations.
        /// </summary>
        private static readonly Dictionary<tTypeOfAuditOperation, SettingsClassDef> _settingsClassPropNames = new Dictionary<tTypeOfAuditOperation, SettingsClassDef>
        {
            { tTypeOfAuditOperation.DELETE, new SettingsClassDef(AuditConstants.DeleteAuditViewNameFieldName, AuditConstants.DeleteAuditFieldName) },
            { tTypeOfAuditOperation.INSERT, new SettingsClassDef(AuditConstants.InsertAuditViewNameFieldName, AuditConstants.InsertAuditFieldName) },
            { tTypeOfAuditOperation.SELECT, new SettingsClassDef(AuditConstants.SelectAuditViewNameFieldName, AuditConstants.SelectAuditFieldName) },
            { tTypeOfAuditOperation.UPDATE, new SettingsClassDef(AuditConstants.UpdateAuditViewNameFieldName, AuditConstants.UpdateAuditFieldName) },
        };

        /// <summary>
        /// Gets the audit setting value for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the data object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="needFieldExist">Flag of required property.</param>
        /// <returns>
        /// Setting value for the specified property.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown when audit settings class doesn't exist.</exception>
        /// <exception cref="DataNotFoundAuditException">Thrown whe attribute is required and doesn't exist at settings class.</exception>
        private static object GetAuditSettingValue(Type dataObjectType, string propertyName, bool needFieldExist = true)
        {
            Type auditSettingsType = GetAuditSettingsType(dataObjectType);
            if (auditSettingsType == null)
            {
                throw new InvalidOperationException($"Type \"{dataObjectType.FullName}\" doesn't contain audit settings (nested \"{AuditConstants.AuditSettingsClassName}\" class).");
            }

            FieldInfo fieldInfo = auditSettingsType.GetField(propertyName);
            if (needFieldExist && fieldInfo == null)
            {
                throw new DataNotFoundAuditException($"Audit settings class doesn't contain property \"{propertyName}\".");
            }

            if (needFieldExist || fieldInfo != null)
            {
                return fieldInfo.GetValue(null);
            }

            return null;
        }

        /// <summary>
        /// Gets the internal type with audit settings for the specified data object type.
        /// </summary>
        /// <param name="dataObjectType">Type of the current.</param>
        /// <returns>Internal type with audit settings or <c>null</c> if it doesn't exist.</returns>
        private static Type GetAuditSettingsType(Type dataObjectType)
        {
            return dataObjectType.GetNestedType(AuditConstants.AuditSettingsClassName);
        }
    }
}

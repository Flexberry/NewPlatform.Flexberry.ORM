namespace ICSSoft.STORMNET
{
    using System;

    /// <summary>
    ///     Делегат для проверки совместимости хранилищ свойств у указанных типов.
    /// </summary>
    /// <param name="dobjType">Тип проверяемого объекта данных.</param>
    /// <param name="propName">Проверяемое свойство.</param>
    /// <param name="propValType">Тип значения, присвоенного свойству.</param>
    /// <param name="allowedType">Тип, являющийся допустимым для свойства.</param>
    /// <returns>Возвращает <c>true</c>, если совместимы.</returns>
    public delegate bool CheckCompatiblePropertyStorageTypesDelegate(Type dobjType, string propName, Type propValType, Type allowedType);
}
namespace ICSSoft.STORMNET.Windows.Forms
{
    using System;

    /// <summary>
    /// Делегат для получения типа по его имени (используется в особых случаях, когда стандартные методы почему-то не помогают).
    /// </summary>
    /// <param name="typeName">Имя типа.</param>
    /// <returns>Сформированный по имени тип.</returns>
    public delegate Type TypeResolveDelegate(string typeName);
}
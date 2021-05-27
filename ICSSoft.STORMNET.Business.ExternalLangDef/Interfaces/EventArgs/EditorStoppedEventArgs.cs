using System;

namespace ICSSoft.STORMNET.UI
{
    /// <summary>
    /// Аргументы для событий остановки работы редактора.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.EditorStoppedEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class EditorStoppedEventArgs : System.EventArgs
    {
    }

    /// <summary>
    /// Делегат для событий остановки работы редактора.
    /// </summary>
    public delegate void EditorStoppedEventArgsHandler(object sender, ICSSoft.STORMNET.UI.EditorStoppedEventArgs e);
}

using System;

namespace ICSSoft.STORMNET.UI
{
    /// <summary>
    /// Аргументы к событию остановки инициатора.
    /// </summary>
    [EventArgCatcherType("ICSSoft.STORMNET.UI.InitiatorStoppedEventArgsCatcher, ICSSoft.STORMNET.UI, Version=1.0.0.1, Culture=neutral, PublicKeyToken=21ce651d390c1fa0")]
    public class InitiatorStoppedEventArgs : System.EventArgs
    {
    }

    /// <summary>
    /// Делегат для события остановки инициатора.
    /// </summary>
    public delegate void InitiatorStoppedEventArgsHandler(object sender, ICSSoft.STORMNET.UI.InitiatorStoppedEventArgs e);

    /// <summary>
    /// Делегат для события остановки инициатора.
    /// </summary>
    public delegate void PrintInitiatorStoppedEventArgsHandler(object sender, ICSSoft.STORMNET.UI.PrintInitiatorStoppedEventArgs e);
}

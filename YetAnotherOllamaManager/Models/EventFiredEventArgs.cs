namespace YetAnotherOllamaManager.Models;

using System;

public class EventFiredEventArgs(object[] args) : EventArgs
{

    public object[] Args { get; set; } = args;
}

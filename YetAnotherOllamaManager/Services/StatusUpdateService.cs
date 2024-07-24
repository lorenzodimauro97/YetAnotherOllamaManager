namespace YetAnotherOllamaManager.Services;

using Microsoft.VisualStudio.Threading;
using Models;
using System;
using System.Threading.Tasks;

public class StatusUpdateService
{
    public event AsyncEventHandler<EventFiredEventArgs> EventFired = null!;

    public async Task InvokeEventFiredAsync(object source, EventFiredEventArgs args)
    {
        try
        {
            await EventFired.InvokeAsync(source, args);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

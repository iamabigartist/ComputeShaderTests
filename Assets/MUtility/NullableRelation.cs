using System;
public class A
{
    public event Action CallMeHandler;
    public event EventHandler<EventArgs> CallYouHandler;
    protected virtual void OnCallMeHandler()
    {
        CallMeHandler?.Invoke();
    }
    protected virtual void OnCallYouHandler() { CallYouHandler?.Invoke( this, new EventArgs()); }
}

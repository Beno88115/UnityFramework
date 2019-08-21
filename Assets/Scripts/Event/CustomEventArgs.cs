using GameFramework;
using GameFramework.Event;

public class CustomEventArgs : GameEventArgs
{
    private int m_Id;
    private object m_UserData;

    public CustomEventArgs()
    {
        m_Id = 0;
        m_UserData = null;
    }

    public override int Id
    {
        get { return m_Id; }
    }

    public object UserData
    {
        get { return m_UserData; }
    }

    public static CustomEventArgs Create(int Id, object userData = null)
    {
        CustomEventArgs eventArgs = ReferencePool.Acquire<CustomEventArgs>();
        eventArgs.m_Id = Id;
        eventArgs.m_UserData = userData;
        return eventArgs;
    }

    public override void Clear()
    {
        m_Id = 0;
        m_UserData = null;
    }
}

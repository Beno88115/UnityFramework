using GameFramework;
using GameFramework.Event;

public class DataEventArgs : GameEventArgs
{
    private int m_Id;

    public override int Id
    {
        get { return m_Id; }
    }

    public static DataEventArgs Create(int Id)
    {
        DataEventArgs eventArgs = ReferencePool.Acquire<DataEventArgs>();
        eventArgs.m_Id = Id;
        return eventArgs;
    }

    public override void Clear()
    {
        m_Id = 0;
    }
}

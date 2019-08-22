using GameFramework.Data;

public class UserData : DataBase
{
    public int ID { get; private set; }
    public string Name { get; private set; }

    public override bool Parse(object dataObject)
    {
        return true;
    }

    public override void Clear()
    {
    }
}
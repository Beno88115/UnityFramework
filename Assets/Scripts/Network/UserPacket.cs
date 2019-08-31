using UnityEngine;
using GameFramework.Network;
using SimpleJSON;

public class UserPacket : Packet
{
    public override int Id { get { return 1000; } }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }

    public override object Serialize()
    {
        JSONObject jsonObject = new JSONObject();
        jsonObject["name"] = UserName;
        jsonObject["password"] = Password;
        jsonObject["address"] = Address;
        return jsonObject;
    }

    public override void Deserialize(object data)
    {
        JSONNode node = (JSONNode)data;
        UserName = node["name"];
        Password = node["password"];
        Address = node["address"];
    }

    public override void Clear()
    {
        UserName = string.Empty;
        Password = string.Empty;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Network;
using SimpleJSON;
using GameFramework;

public class AuthPacket : Packet
{
    public override int Id { get { return 9999; } }
    public string UserName { get; set; }
    public string SeverName { get; set; }
    public int SubID { get; set; }

    public override object Serialize()
    {
        string token = string.Format("{0}@{1}#{2}:{3}", Utility.Crypt.Base64Encode(UserName), 
            Utility.Crypt.Base64Encode(SeverName), Utility.Crypt.Base64Encode(SubID), 1);
        ulong hmac = Utility.Crypt.HMac64(Utility.Crypt.HashKey(token), AppConst.kSecret);
        return new JSONString(token + ":" + Utility.Crypt.Base64Encode(hmac));
    }

    public override void Deserialize(object data)
    {
    }

    public override void Clear()
    {
    }
}
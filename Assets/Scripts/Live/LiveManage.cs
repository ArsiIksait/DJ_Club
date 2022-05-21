using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LiveManage
{
    /// <summary>
    /// 获取消息类型
    /// </summary>
    public Dictionary<string, List<LiveGift>> Gifts { get; } = new Dictionary<string, List<LiveGift>>(); //key:用户 value:用户所有发送的礼物
    /// <summary>
    /// 储存所有玩家对象
    /// </summary>
    public List<GameObject> Players = new List<GameObject>();
    /// <summary>
    /// 储存所有玩家UID
    /// </summary>
    public List<int> PlayerUIDs = new List<int>();

    public void Join(int UID, string Name)
    {
        if (!PlayerUIDs.Contains(UID))
        {
            //float sizeX = MessageHandle.Ground.GetComponent<MeshFilter>().mesh.bounds.size.x * MessageHandle.Ground.transform.localScale.x;
            //float sizeZ = MessageHandle.Ground.GetComponent<MeshFilter>().mesh.bounds.size.z * MessageHandle.Ground.transform.localScale.z;
            //Instantiate(MessageHandle.Player).transform.position = new Vector2(UnityEngine.Random.Range(-sizeX, sizeX), UnityEngine.Random.Range(-sizeZ, sizeZ));
            //manage.PlayerUIDs.Add(UID);
            //Debug.Log($"加入玩家 {UID}: {Name}");
        }
    }

    public bool AddGifts(LiveGift gift)
    {
         if (Gifts.TryGetValue(gift.Name,out var user))
        {
            user.Add(gift);
        }
        else
        {
            Gifts.Add(gift.Name, new List<LiveGift> { gift });
        }
        return true;
    }

    public static (MessageType type, string str) GetMessageType(string input)
    {
        if (input.StartsWith("msg"))
        {
            return (MessageType.Message, input.Remove(0, 4));
        }
        else if (input.StartsWith("gift"))
        {
            return (MessageType.Gift, input.Remove(0, 5));
        }
        else
        {
            return (MessageType.None, null);
        }
    }
}

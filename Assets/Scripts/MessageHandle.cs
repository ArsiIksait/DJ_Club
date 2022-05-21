using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using Ilyfairy.LoliSocket;
using System.Linq;
using MusicAndAudios;

public class MessageHandle : MonoBehaviour
{
    public LoliSocketServer Server { get; set; }
    public LiveManage manage = new LiveManage();
    public Text giftTop;
    public static GameObject Ground;
    public static GameObject Player;

    private void OnDestroy()
    {
        Debug.Log($"[{DateTime.Now}] [ServerInfo]: 正在关闭LoliSocketServer服务器...");
        if (Server.Close())
        {
            Debug.Log($"[{DateTime.Now}] [ServerInfo]: 已关闭LoliSocketServer服务器!");
        }
        else
        {
            Debug.LogError($"[{DateTime.Now}] [ServerError]: LoliSocketServer服务器关闭失败! 正在退出程序...");
#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    void Start()
    {
        Debug.Log($"[{DateTime.Now}] [ServerInfo]: 正在启动LoliSocketServer服务器...");
        Server = new LoliSocketServer("127.0.0.1", 25565);
        Server.SocketAccept += Server_SocketAccept; //收到客户端连接 事件
        Server.SocketReceive += Server_SocketReceive;   //收到客户端发送过来了的信息 事件
        var isStart = Server.Start();
        if (isStart)
        {
            Debug.Log($"[{DateTime.Now}] [ServerInfo]: LoliSocketServer服务器启动成功!");
        }
        else
        {
            Debug.LogError($"[{DateTime.Now}] [ServerError]: LoliSocketServer服务器启动失败!");
        }
        InvokeRepeating("Flsah_GiftTop", 0, 5);
    }

    public void Flsah_GiftTop()
    {
        try
        {
            var userlist = manage.Gifts.ToList();
            var t = userlist.Select(v => (v.Key, v.Value.Select(v2 => v2.TotalPrice).Sum()));
            var users = t.Select(v => v.Item2 == 0 ? (null, 0) : (v.Key, Math.Round(v.Item2, 2))).Where(v => v.Key != null && v.Item2 != 0).ToList();
            users.Sort(new UserComparer());
            var result = users.Select(v => $"{v.Key} {v.Item2}").ToArray();
            Array.Resize(ref result, 5);

            var r = Enumerable.Range(1, 5).Select(v => result[v - 1] == null ? $"{v}. 虚位以待" : $"{v}. {result[v - 1]}");
            giftTop.text = string.Join("\n", r);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void CommandAnalysis(LiveMessage cmd)
    {
        string command = cmd.Message;
        var split = command.Split(' ');
        switch (split[0])
        {
            case "点歌":
                //Debug.Log($"CMD点歌 s2={split[0]} s1={split[1]}");
                int index = 0;
                if (int.TryParse(split[1], out int i))
                {
                    index = i;
                }
                else {
                    foreach (string item in Audios.MusicNames)
                    {
                        //Debug.Log($"{split[1]} == {item}: {index}");
                        if (split[1] == item)
                        {
                            break;
                        }
                        index++;
                    }
                }
                Audios.state = index;
                break;
            case "切歌":
                Audios.r1state = true;
                break;
            case "烟雾":
                Audios.r2state = true;
                break;
            case "燃起来了":
                Audios.r3state = true;
                break;
            case "烟花":
                Audios.r4state = true;
                break;
            case "嗨起来":
                Audios.r5state = true;
                break;
            default:
                Debug.Log("CMD不是命令 " + split[0] + " " + split[1]);
                break;
        }
    }

    private class UserComparer : IComparer<(string, double)>
    {
        public int Compare((string, double) x, (string, double) y)
        {
            if (x.Item2 > y.Item2)
            {
                return -1;
            }
            else if (x.Item2 == y.Item2)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }

    //如果有客户端发送信息 就会执行这个
    private void Server_SocketReceive(object sender, ServerReceiveEventArgs e)
    {
        string msg = Encoding.UTF8.GetString(e.Data, 0, e.ActualLength);

        var result = LiveManage.GetMessageType(msg); //这里已经去掉了开头的msg、gift
        switch (result.type)
        {
            case MessageType.None:
                Debug.LogError("Unknow Message");
                break;
            case MessageType.Message:
                {
                    var m = new LiveMessage(result.str);
                    //然后再来分割
                    Debug.Log($"[{DateTime.Now}] 收到消息: UID: {m.UID}  Name: {m.Name}  Message: {m.Message}");
                    CommandAnalysis(m);
                    manage.Join(m.UID, m.Name);
                }
                break;
            case MessageType.Gift:
                {
                    try
                    {
                        LiveGift g = new LiveGift(result.str);
                        Debug.Log($"[{DateTime.Now}] 收到礼物: UID: {g.UID}  Name: {g.Name}  GiftName: {g.GiftName}  GiftNum: {g.GiftNum}  GiftPrice: {g.GiftPrice}  GiftCoinType: {g.GiftCoinType}  TimeStamp: {g.TimeStamp}");
                        manage.AddGifts(g);
                        manage.Join(g.UID, g.Name);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }
                }
                break;
        }
    }
    //如果有客户端连接 就会执行这个
    private void Server_SocketAccept(object sender, ServerAcceptEventArgs e)
    {
        //Debug.Log($"[{DateTime.Now}] [ServerInfo]: NEW Client Login: {e.ClientInfo.ID}");
    }
}

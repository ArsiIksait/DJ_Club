using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Ilyfairy.LoliSocket
{
    public class LoliSocketServer
    {
        private readonly Socket socket;
        private IPEndPoint IPEndPoint;
        private bool isStart = false;

        private Dictionary<string, Socket> clients = new Dictionary<string, Socket>();

        public event ServerReceiveEventHandler SocketReceive;
        public event ServerAcceptEventHandler SocketAccept;
        public event ServerCloseEventHandler SocketClose;

        public LoliSocketServer(string ipAddress, int port) : this(new IPEndPoint(IPAddress.Parse(ipAddress), port), ProtocolType.Tcp)
        {

        }
        public LoliSocketServer(int port) : this(new IPEndPoint(IPAddress.Any, port), ProtocolType.Tcp)
        {

        }
        private LoliSocketServer(IPEndPoint iPEndPoint, ProtocolType protocolType)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocolType);
            this.IPEndPoint = iPEndPoint;
        }

        public bool Start(int bufferSize = 4096)
        {
            return this.Start(20, bufferSize);
        }
        public bool Start(int backlog, int bufferSize = 4096)
        {
            try
            {
                socket.Bind(IPEndPoint);
                socket.Listen(backlog);
                isStart = true;
            }
            catch (Exception)
            {
                return false;
            }
            Task.Run(() =>
            {
                while (isStart) //循环监听客户端连接
                {
                    try
                    {
                        Socket client = socket.Accept(); //客户端已连接
                        Task.Run(() =>
                        {
                            string ip = (client.RemoteEndPoint as IPEndPoint).Address.ToString();
                            int port = (client.RemoteEndPoint as IPEndPoint).Port;
                            string id = $"{ip}:{port}";
                            clients.Add(id, client);
                            var e = new ServerAcceptEventArgs(ip, port, id);
                            this.SocketAccept(this, e);

                            while (isStart)
                            {
                                byte[] tmp = new byte[bufferSize];
                                int len = 0;
                                try
                                {
                                    len = client.Receive(tmp);
                                    if (len == 0)
                                    {
                                        throw new Exception("off");
                                    }
                                }
                                catch (Exception)
                                {
                                    clients.Remove(id);
                                    SocketClose?.Invoke(this, new ServerCloseEventArgs(ip, port, id));
                                    break;
                                }
                                tmp = tmp.Take(len).ToArray();
                                var e2 = new ServerReceiveEventArgs(tmp, len, e.ClientInfo);
                                SocketReceive?.Invoke(this, e2);
                            }
                        });
                    }
                    catch (Exception)
                    {
                        isStart = false;
                        throw;
                    }
                }
            });
            return true;
        }
        public string[] GetClients()
        {
            if (!isStart)
            {
                throw new Exception("连接已关闭");
            }
            return clients.Keys.ToArray();
        }
        public bool Close()
        {
            foreach (var key in clients)
            {
                try
                {
                    key.Value.Close();
                }
                catch (Exception) { }
            }
            isStart = false;
            clients?.Clear();
            try
            {
                socket.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int Send(string id, byte[] data)
        {
            if (data == null)
            {
                return -1;
            }
            if (data.Length == 0)
            {
                return 0;
            }
            if (clients.TryGetValue(id, out Socket s))
            {
                try
                {
                    var r = s.Send(data);
                    return r;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 发送字符串信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public int Send(string id, string text)
        {
            if (text == null)
            {
                return -1;
            }
            if (text == string.Empty)
            {
                return 0;
            }
            return Send(id, Encoding.UTF8.GetBytes(text));
        }
    }

    public delegate void ServerAcceptEventHandler(object sender, ServerAcceptEventArgs e);
    public delegate void ServerReceiveEventHandler(object sender, ServerReceiveEventArgs e);
    public delegate void ServerCloseEventHandler(object sender, ServerCloseEventArgs e);

    public class ServerReceiveEventArgs : EventArgs
    {
        /// <summary>
        /// 接收的数据
        /// </summary>
        public byte[] Data { get; }
        /// <summary>
        /// 获取接收的字符串 编码为UTF-8
        /// </summary>
        public string Text { get => Encoding.UTF8.GetString(Data); }
        /// <summary>
        /// 接收的实际长度
        /// </summary>
        public int ActualLength { get; }
        /// <summary>
        /// 客户端信息
        /// </summary>
        public ClientInfo ClientInfo { get; set; }
        public ServerReceiveEventArgs(byte[] data, int actualLength, ClientInfo info)
        {
            Data = data;
            ActualLength = actualLength;
            ClientInfo = info;
        }
    }

    public class ServerCloseEventArgs : EventArgs
    {
        public ClientInfo ClientInfo { get; set; }
        public ServerCloseEventArgs(string address, int port, string id)
        {
            ClientInfo = new ClientInfo(address, port, id);
        }
    }
    public class ServerAcceptEventArgs : EventArgs
    {
        public ClientInfo ClientInfo { get; set; }
        public ServerAcceptEventArgs(string address, int port, string id)
        {
            ClientInfo = new ClientInfo(address, port, id);
        }
    }
    public class ClientInfo
    {
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string Address { get; }
        /// <summary>
        /// 客户端的端口
        /// </summary>
        public int Port { get; }
        /// <summary>
        /// 客户端ID<br/>格式为Address:Port<br/>
        /// </summary>
        public string ID { get; }
        public ClientInfo(string address, int port, string id)
        {
            Address = address;
            Port = port;
            ID = id;
        }
        public ClientInfo()
        {

        }
    }
}

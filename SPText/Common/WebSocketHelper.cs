using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebSockets;

namespace SPText.Common
{
    public class WebSocketHelper
    {
        private string userName = string.Empty;
        public void Show()
        {

        }

        /// <summary>
        /// 默认某一个群组里面有这么一些人
        /// </summary>
        public static List<SocketModel> socketlist = new List<SocketModel>() {
             new SocketModel(){ SocketGuid=string.Empty,UserName="User1",Socket=null },
             new SocketModel(){ SocketGuid=string.Empty,UserName="User2",Socket=null },
             new SocketModel(){ SocketGuid=string.Empty,UserName="User3",Socket=null },
             new SocketModel(){ SocketGuid=string.Empty,UserName="User4",Socket=null }
        };
        /// <summary>
        /// 群发
        /// </summary>
        /// <param name="socketContext"></param>
        /// <returns></returns>
        public async Task ProcessChat(AspNetWebSocketContext socketContext)
        {
            userName = "123";
            //  SuperSocket:Session
            // 表示客户端发起请求的一个链接
            System.Net.WebSockets.WebSocket socket = socketContext.WebSocket;

            CancellationToken token = new CancellationToken();

            string socketGuid = Guid.NewGuid().ToString();

            AddUser(socketGuid, userName, socket, token);

            await SengdMessageOld(token, userName, "进入聊天室");
            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, token);
                string userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count); // 来自于客户端发送过来的消息内容 

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    RemoveUser(socketGuid);
                    await SengdMessageOld(token, userName, "离开聊天室");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, token);
                }
                else
                {
                    await SengdMessageOld(token, userName, userMessage);
                }
            }
        }

        /// <summary>
        /// 单发
        /// </summary>
        /// <param name="socketContext"></param>
        /// <returns></returns>
        public async Task ProcessChat0(AspNetWebSocketContext socketContext)
        {
            userName = "123";
            //  SuperSocket:Session
            // 表示客户端发起请求的一个链接
            System.Net.WebSockets.WebSocket socket = socketContext.WebSocket;

            CancellationToken token = new CancellationToken();

            string socketGuid = Guid.NewGuid().ToString();

            AddUser(socketGuid, userName, socket);

            //await OldChatManager.SengdMessage(token, UserName, "进入聊天室");
            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, token);
                string userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count); // 来自于客户端发送过来的消息内容 

                await SengdMessage(token, userName, userMessage);//单聊
            }
        }


        // string: 要发谁   ArraySegment<byte>：要发送的消息
        public static Dictionary<string, List<ArraySegment<byte>>> chatList = new Dictionary<string, List<ArraySegment<byte>>>();


        public static void AddUser(string socketGuid, string userName, WebSocket socket, CancellationToken token)
        {
            socketlist.ForEach(item =>
            {
                if (userName == item.UserName)
                {
                    item.Socket = socket;
                    item.SocketGuid = socketGuid;
                }
                //else
                //{
                //    item.UserName = userName;
                //    item.Socket = socket;
                //    item.SocketGuid = socketGuid;
                //}
            });

            if (chatList.ContainsKey(userName) && chatList[userName].Count > 0)
            {
                foreach (var item in chatList[userName])
                {
                    socket.SendAsync(item, WebSocketMessageType.Text, true, token);
                }

                // 历史消息重新发送以后呢： 就应该删除掉  
                // 清除消息

            }

        }


        public static void RemoveUserOld(string socketGuid)
        {
            socketlist.ForEach(item =>
            {
                if (socketGuid == item.SocketGuid)
                {
                    item.Socket = null;
                    item.SocketGuid = null;
                }
            });
        }


        /// <summary>
        ///  群发消息 包括离线消息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task SengdMessageOld(CancellationToken token, string userName, string content)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
            buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes($"{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss:fff")}{userName}:{content}"));

            foreach (var socketInfo in socketlist)
            {
                if (socketInfo.Socket == null)
                {
                    if (chatList.ContainsKey(socketInfo.UserName))
                    {
                        chatList[socketInfo.UserName].Add(buffer);
                    }
                    else
                    {
                        chatList.Add(socketInfo.UserName, new List<ArraySegment<byte>>() { buffer });
                    }
                }
                else
                {
                    await socketInfo.Socket.SendAsync(buffer, WebSocketMessageType.Text, true, token);
                }
            }

        }


        public static async Task SendOne(string messge, CancellationToken cancellationToken)
        {
            //   user1;你好
            string[] messageArray = messge.Split(';');
            string toUser = messageArray[0];
            string toMessage = messageArray[1];
            var socketModel = socketlist.FirstOrDefault(a => toUser.Equals(a.UserName));
            if (socketModel != null)
            {
                WebSocket toSocket = socketModel.Socket;
                ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(toMessage));
                await toSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
            }
        }



        /// <summary>
        /// 添加一个用户（包含了这个用户对应的Socket）
        /// </summary>
        /// <param name="socketGuid"></param>
        /// <param name="userName"></param>
        /// <param name="socket"></param>
        public static void AddUser(string socketGuid, string userName, WebSocket socket)
        {
            socketlist.Add(new SocketModel()
            {
                SocketGuid = socketGuid,
                UserName = userName,
                Socket = socket
            });
        }

        /// <summary>
        /// 删除已经连接的用户
        /// </summary>
        /// <param name="socketGuid"></param>
        public static void RemoveUser(string socketGuid)
        {
            socketlist = socketlist.Where(a => a.SocketGuid != socketGuid).ToList();
        }

        /// <summary>
        ///  群发消息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task SengdMessage(CancellationToken token, string userName, string content)
        {
            ///WebSocket 消息发送的格式 消息内容的长度
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);

            buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes($"{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss:fff")}{userName}:{content}"));

            ///给每一个Socket （用户） 发送消息 （类似于一个广播的形式）
            foreach (var socketInfo in socketlist)
            {
                await socketInfo.Socket.SendAsync(buffer, WebSocketMessageType.Text, true, token);
            }

        }
    }
    public class SocketModel
    {
        /// <summary>
        /// 链接的唯一ID
        /// </summary>
        public string SocketGuid { get; set; }

        /// <summary>
        ///  用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 每一个用户链接进来以后 对应的这一个Socket实例
        /// </summary>
        public WebSocket Socket { get; set; }
    }
}

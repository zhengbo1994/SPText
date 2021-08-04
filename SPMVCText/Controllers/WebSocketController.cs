using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;

namespace SPMVCText.Controllers
{
    public class WebSocketController : Controller
    {
        // GET: WebSocket
        public ActionResult Index()
        {
            return View();
        }

        private string UserName = string.Empty;

        /// <summary>
        /// WebSocket建立链接的方法
        /// </summary>
        /// <param name="name"></param>
        public void MyWebSocket(string name)
        {
            if (HttpContext.IsWebSocketRequest)
            {
                this.UserName = name;
                HttpContext.AcceptWebSocketRequest(ProcessChat);
            }
            else
            {
                HttpContext.Response.Write("我不处理");
            }
        }

        /// <summary>
        /// 群发
        /// </summary>
        /// <param name="socketContext"></param>
        /// <returns></returns>
        public async Task ProcessChat(AspNetWebSocketContext socketContext)
        {
            //  SuperSocket:Session
            // 表示客户端发起请求的一个链接
            System.Net.WebSockets.WebSocket socket = socketContext.WebSocket;

            CancellationToken token = new CancellationToken();

            string socketGuid = Guid.NewGuid().ToString();

            OldChatManager.AddUser(socketGuid, UserName, socket, token);

            await OldChatManager.SengdMessage(token, UserName, "进入聊天室");
            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, token);
                string userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count); // 来自于客户端发送过来的消息内容 

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    OldChatManager.RemoveUser(socketGuid);
                    await OldChatManager.SengdMessage(token, UserName, "离开聊天室");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, token);
                }
                else
                {
                    await OldChatManager.SengdMessage(token, UserName, userMessage);
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
            //  SuperSocket:Session
            // 表示客户端发起请求的一个链接
            System.Net.WebSockets.WebSocket socket = socketContext.WebSocket;

            CancellationToken token = new CancellationToken();

            string socketGuid = Guid.NewGuid().ToString();

            ChatManager.AddUser(socketGuid, UserName, socket);

            //await OldChatManager.SengdMessage(token, UserName, "进入聊天室");
            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, token);
                string userMessage = Encoding.UTF8.GetString(buffer.Array, 0, result.Count); // 来自于客户端发送过来的消息内容 

                await ChatManager.SengdMessage(token, UserName, userMessage);//单聊
            }
        }
    }
}
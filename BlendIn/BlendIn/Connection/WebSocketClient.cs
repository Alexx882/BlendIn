using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlendIn.Connection
{
    public class WebSocketClient
    {
        ClientWebSocket client = new ClientWebSocket();
        CancellationTokenSource cts = new CancellationTokenSource();

        private static WebSocketClient _instance = null;
        public static WebSocketClient Instance => _instance ?? new WebSocketClient();

        private WebSocketClient()
        {
        }

        public async void ConnectToServerAsync(string ip = "192.168.8.155")
        {
            await client.ConnectAsync(new Uri($"ws://{ip}:8080"), cts.Token);
            Console.WriteLine($"Websocket state {client.State}");

            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await HandleIncomingMessages();
                }
            }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private async Task HandleIncomingMessages()
        {
            WebSocketReceiveResult result;
            var buffer = new ArraySegment<byte>(new byte[4096]);
            var message = "";
            do
            {
                result = await client.ReceiveAsync(buffer, cts.Token);
                if (result.MessageType != WebSocketMessageType.Text)
                    break;
                var messageBytes = buffer.Skip(buffer.Offset).Take(result.Count).ToArray();
                string receivedMessage = Encoding.UTF8.GetString(messageBytes);
                message += receivedMessage;
            } while (!result.EndOfMessage);

            var baseMsg = JsonConvert.DeserializeObject<BaseMessage>(message);

            if (baseMsg.Event == "connected")
            {
                Debug.WriteLine("connected");
                SendMessageAsync(new BaseMessage(){Event = "thanks"});
            }
        }

        public async void SendMessageAsync(BaseMessage message)
        {
            var byteMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var segment = new ArraySegment<byte>(byteMessage);

            await client.SendAsync(segment, WebSocketMessageType.Text, true, cts.Token);
        }
    }
}
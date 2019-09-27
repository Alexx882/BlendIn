using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlendIn.Connection.Messages;
using BlendIn.Connection.Responses;
using Newtonsoft.Json;

namespace BlendIn.Connection
{
    public class WebSocketClient
    {
        ClientWebSocket client = new ClientWebSocket();
        CancellationTokenSource cts = new CancellationTokenSource();

        private static WebSocketClient _instance = null;

        public static WebSocketClient Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new WebSocketClient();
                return _instance;
            }
        }

        private WebSocketClient()
        {
        }

        public async Task ConnectToServerAsync(string ip = "192.168.8.155")
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

            Debug.WriteLine(message);

            if (baseMsg.@event == "connected")
            {
                Debug.WriteLine("### Connected");
                SendMessageAsync(new BaseMessage(){ @event = "thanks"});
            }
            else if (baseMsg.@event == "login")
            {
                var loginMsg = JsonConvert.DeserializeObject<LoginResponse>(message);
                if(_registeredCallbacks.ContainsKey(typeof(LoginResponse)))
                    _registeredCallbacks[typeof(LoginResponse)]?.Invoke(loginMsg);
            }
            else if (baseMsg.@event == "join")
            {
                var loginMsg = JsonConvert.DeserializeObject<PlayerJoinedResponse>(message);
                if (_registeredCallbacks.ContainsKey(typeof(PlayerJoinedResponse)))
                    _registeredCallbacks[typeof(PlayerJoinedResponse)]?.Invoke(loginMsg);
            }
            else if (baseMsg.@event == "start")
            {
                var loginMsg = JsonConvert.DeserializeObject<TimerResponse>(message);
                if (_registeredCallbacks.ContainsKey(typeof(TimerResponse)))
                    _registeredCallbacks[typeof(TimerResponse)]?.Invoke(loginMsg);
            }
        }

        private Dictionary<Type, Action<object>> _registeredCallbacks = new Dictionary<Type, Action<object>>();
        public void RegisterForMessage<TMessage>(Action<object> test)
        {
            _registeredCallbacks.Add(typeof(TMessage), test);
        }

        public async Task SendMessageAsync(BaseMessage message)
        {
            var byteMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var segment = new ArraySegment<byte>(byteMessage);

            await client.SendAsync(segment, WebSocketMessageType.Text, true, cts.Token);
        }

    }
}
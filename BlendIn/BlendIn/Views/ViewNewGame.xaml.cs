using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlendIn.Connection;
using BlendIn.Connection.Messages;
using BlendIn.Connection.Responses;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlendIn.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewNewGame : ContentPage
    {
        private string _lobby_name;
        public ViewNewGame()
        {
            // TODO Sound and Light optional settings
            InitializeComponent();
        }

        private async void ConnectAsHost(string name)
        {
            await WebSocketClient.Instance.ConnectToServerAsync();
            WebSocketClient.Instance.RegisterForMessage<LoginResponse>(HandleLoginResponse);
            WebSocketClient.Instance.RegisterForMessage<PlayerJoinedResponse>(HandlePlayerJoined);
            WebSocketClient.Instance.RegisterForMessage<TimerResponse>(HandleTimerResponse);

            await WebSocketClient.Instance.SendMessageAsync(new LoginMessage(){ @event = "login", username = name});
        }

        private void HandleTimerResponse(object obj)
        {
            var response = obj as TimerResponse;
        }

        private void HandlePlayerJoined(object obj)
        {
            var response = obj as PlayerJoinedResponse;
            Device.BeginInvokeOnMainThread(() => {
                LabelConnectedPlayers.Text += " ; " + response.user;
            });
        }

        private void HandleLoginResponse(object message)
        {
            var response = message as LoginResponse;
            _lobby_name = response.lobby_name;
            Device.BeginInvokeOnMainThread(() => {
                LabelLobby.Text = response.lobby_name;
            });
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var name = EntryUsername.Text;
            ConnectAsHost(name);
        }

        private void ButtonStart_OnClicked(object sender, EventArgs e)
        {
            WebSocketClient.Instance.SendMessageAsync(new StartGameMessage(){@event = "start", lobby = _lobby_name});
        }
    }
}
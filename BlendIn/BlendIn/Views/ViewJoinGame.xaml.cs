using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlendIn.Connection;
using BlendIn.Connection.Messages;
using BlendIn.Connection.Responses;
using BlendIn.Game;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlendIn.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewJoinGame : ContentPage
    {
        public ViewJoinGame()
        {
            BackgroundImage = "bg.png";
            InitializeComponent();
        }

        private void Go_Clicked(object sender, EventArgs e)
        {
            ConnectToServer(EntryUsername.Text, EntryGameCode.Text);
        }

        private async void ConnectToServer(string username, string joinGameCode)
        {
            await WebSocketClient.Instance.ConnectToServerAsync();
            WebSocketClient.Instance.RegisterForMessage<LoginResponse>(HandleLoginResponse);
            WebSocketClient.Instance.RegisterForMessage<TimerResponse>(HandleTimerResponse);

            await WebSocketClient.Instance.SendMessageAsync(new LoginMessage()
                {@event = "login", username = username, lobby = joinGameCode});

            GameLogic.Instance.Self = new Player(){PlayerName = username};
            GameLogic.Instance.LobbyName = joinGameCode;
        }

        private void HandleTimerResponse(object obj)
        {
            var response = obj as TimerResponse;
            Device.BeginInvokeOnMainThread(() =>
            {
                var v = new TimerView(response);
                Navigation.PushAsync(v);
            });
        }

        private void HandleLoginResponse(object obj)
        {
            var response = obj as LoginResponse;
            Device.BeginInvokeOnMainThread(() =>
            {
                ButtonGo.IsEnabled = false;
                if (response.status != "success")
                    LabelOutput.Text = response.error;
                else
                    LabelOutput.Text = "waiting for others";
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlendIn.Connection;
using BlendIn.Connection.Responses;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlendIn.Game
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimerView : ContentPage
    {
        private string _hunter_name;
        private long _game_start_time;

        public TimerView(TimerResponse response)
        {
            InitializeComponent();
            _hunter_name = response.hunter_username;
            _game_start_time = response.starttime;

            new Thread(() => TimerFunction(10)).Start();
        }

        private void TimerFunction(int remainingSeconds)
        {
            while (remainingSeconds > 0)
            {
                Device.BeginInvokeOnMainThread(() => { LabelInfo.Text = "" + remainingSeconds; });
                Thread.Sleep(1000);
                remainingSeconds--;
            }

            if (WebSocketClient.Instance.UserName == _hunter_name)
            {
                Device.BeginInvokeOnMainThread(() => LabelInfo.Text = "You are the hunter");
                WebSocketClient.Instance.IsHunter = true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => LabelInfo.Text = "You are prey");
                WebSocketClient.Instance.IsHunter = false;
            }
        }
    }
}
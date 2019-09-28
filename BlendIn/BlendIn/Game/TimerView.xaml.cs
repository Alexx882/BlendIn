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
            BackgroundImage = "bg.png";
            InitializeComponent();
            _hunter_name = response.hunter_username;
            _game_start_time = response.starttime;

            new Thread(() => TimerFunction(0)).Start();
        }

        private void TimerFunction(int remainingSeconds)
        {
            while (remainingSeconds > 0)
            {
                Device.BeginInvokeOnMainThread(() => { LabelInfo.Text = "" + remainingSeconds; });
                Thread.Sleep(1000);
                remainingSeconds--;
            }

            GameLogic.HunterName = _hunter_name;
            if (GameLogic.Instance.SelfIsHunter)
            {
                Device.BeginInvokeOnMainThread(() => LabelInfo.Text = "You are the hunter");
                Thread.Sleep(1000);
                Device.BeginInvokeOnMainThread(() => Navigation.PushAsync(new HunterGameView()));
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => LabelInfo.Text = "You are prey");
                Thread.Sleep(1000);
                Device.BeginInvokeOnMainThread(() => Navigation.PushAsync(new PreyGameView()));
            }
        }
    }
}
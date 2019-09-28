using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlendIn.Connection;
using BlendIn.Connection.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace BlendIn.Game
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HunterGameView : ContentPage
    {
        public double exposeCDValue = 20;
        public double stunCDValue = 30;

        public double exposeCurrent = 0;
        public double stunCurrent = 0;


        public HunterGameView()
        {
            InitializeComponent();
            new Thread(() => HunterLoop()).Start();
        }

        public void Stun_Clicked(object sender, EventArgs e)
        {
            WebSocketClient.Instance.SendMessageAsync(new HunterAction()
                {@event = "stun", lobby = GameLogic.Instance.LobbyName, user = GameLogic.Instance.SelfUserName});
            stunCurrent = stunCDValue;
            ButtonStun.IsEnabled = false;
        }


        public void Expose_Clicked(object sender, EventArgs e)
        {
            WebSocketClient.Instance.SendMessageAsync(new HunterAction()
                {@event = "expose", lobby = GameLogic.Instance.LobbyName, user = GameLogic.Instance.SelfUserName});
            exposeCurrent = exposeCDValue;
            ButtonExpose.IsEnabled = false;
        }

        public void Catch_Clicked(object sender, EventArgs e)
        {
            // todo barcode
            var caught_user = "user";
            WebSocketClient.Instance.SendMessageAsync(new HunterAction()
            {
                @event = "catch", lobby = GameLogic.Instance.LobbyName, user = GameLogic.Instance.SelfUserName,
                caught = caught_user
            });
        }

        private void HunterLoop()
        {
            while (true)
            {
                GameLogic.Instance.SendLocation();
                if (exposeCurrent > 0)
                {
                    exposeCurrent--;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() => { ButtonExpose.IsEnabled = true; });
                }

                if (stunCurrent > 0)
                {
                    stunCurrent--;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() => { ButtonStun.IsEnabled = true; });
                }

                Device.BeginInvokeOnMainThread(() => { oct_null.Text = GetOctantString(7); });
                Device.BeginInvokeOnMainThread(() => { oct_eins.Text = GetOctantString(6); });
                Device.BeginInvokeOnMainThread(() => { oct_zwei.Text = GetOctantString(5); });
                Device.BeginInvokeOnMainThread(() => { oct_drei.Text = GetOctantString(4); });
                Device.BeginInvokeOnMainThread(() => { oct_vier.Text = GetOctantString(3); });
                Device.BeginInvokeOnMainThread(() => { oct_fuenf.Text =GetOctantString(2); });
                Device.BeginInvokeOnMainThread(() => { oct_sechs.Text =GetOctantString(1); });
                Device.BeginInvokeOnMainThread(() => { oct_sieben.Text =GetOctantString(0); });

                PrintLocations();

                Thread.Sleep(1000);
            }
        }

        private void PrintLocations()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                LabelDebug.Text = "";
                GameLogic.Instance.Players
                    .ForEach(p =>
                        LabelDebug.Text += $"({p.PlayerName}, {p.Location.Latitude}, {p.Location.Longitude}), ");
                LabelDebug.TextColor = Color.AntiqueWhite;
                LabelDebug.Text += " "+GameLogic.Instance.GetCompass();
            });
        }

        private string GetOctantString(int octant)
        {
            string s = "";
            //for (int i = 0; i < GameLogic.Instance.GetAmountOfPlayersInOctant(octant); i++)
            //{
            //    s += "*";
            //}
            foreach (Player player in GameLogic.Instance.GetListOfPlayersInOctant(octant))
            {
                double distance = Calculations.GetDistance(GameLogic.Instance.Self.Location, player.Location);
                if (distance < 3)
                {
                    s += "O";
                }
                else if (distance < 5)
                {
                    s += "o";
                }
                else if (distance < 8)
                {
                    s += "*";
                }
                else
                {
                    s += ".";
                }
            }
            if (s.Equals(""))
            {
                return "-";
            }
                return s;
        }
    }
}
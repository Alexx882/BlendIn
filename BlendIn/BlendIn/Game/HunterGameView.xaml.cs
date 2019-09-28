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
            BackgroundImage = "gamebg.png";
            InitializeComponent();
            new Thread(() => HunterLoop()).Start();
        }

        public void Stun_Clicked(object sender, EventArgs e)
        {
            WebSocketClient.Instance.SendMessageAsync(new HunterAction()
                {@event = "stun", lobby = GameLogic.Instance.LobbyName, username = GameLogic.Instance.SelfUserName});
            stunCurrent = stunCDValue;
            ButtonStun.IsEnabled = false;
        }


        public void Expose_Clicked(object sender, EventArgs e)
        {
            WebSocketClient.Instance.SendMessageAsync(new HunterAction()
                {@event = "expose", lobby = GameLogic.Instance.LobbyName, username = GameLogic.Instance.SelfUserName});
            exposeCurrent = exposeCDValue;
            ButtonExpose.IsEnabled = false;
        }

        public void Catch_Clicked(object sender, EventArgs e)
        {
            // todo barcode
            var caught_user = "username";
            WebSocketClient.Instance.SendMessageAsync(new HunterAction()
            {
                @event = "catch", lobby = GameLogic.Instance.LobbyName, username = GameLogic.Instance.SelfUserName,
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

                Device.BeginInvokeOnMainThread(() => { PushOctantString(oct_null, 7); });
                Device.BeginInvokeOnMainThread(() => { PushOctantString(oct_eins, 6); });
                Device.BeginInvokeOnMainThread(() => { PushOctantString(oct_zwei, 5); });
                Device.BeginInvokeOnMainThread(() => { PushOctantString(oct_drei, 4); });
                Device.BeginInvokeOnMainThread(() => { PushOctantString(oct_vier, 3); });
                Device.BeginInvokeOnMainThread(() => { PushOctantString(oct_fuenf, 2); });
                Device.BeginInvokeOnMainThread(() => { PushOctantString(oct_sechs, 1); });
                Device.BeginInvokeOnMainThread(() => { PushOctantString(oct_sieben, 0); });

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

        private void PushOctantString(Label label, int octant)
        {
            label.TextColor = Color.FromHex("#0BD904");
            List<Player> playerList = GameLogic.Instance.GetListOfPlayersInOctant(octant);
            int amount = playerList.Count;
            int intensity = 0;
            label.Text = "*";
            foreach (Player player in playerList)
            {
                double distance = Calculations.GetDistance(GameLogic.Instance.Self.Location, player.Location);
                if (distance < 10)
                {
                    label.Text = "X";
                    label.TextColor = Color.Red;
                    intensity = 50;
                }
                else if(distance < 20)
                {   
                    intensity += 25;
                }
                else
                {
                    intensity += 15;
                }
               
            }
            if (intensity > 55)
            {
                label.FontSize = 55;
            }
            else
            {
                label.FontSize = intensity;
            }

            if (amount<=0)
            {
                label.Text = "-";
                label.FontSize = 20;
            }
        }
    }
}
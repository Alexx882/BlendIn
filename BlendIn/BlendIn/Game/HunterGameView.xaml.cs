using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlendIn.Connection;
using BlendIn.Connection.Messages;
using BlendIn.Connection.Responses;
using BlendIn.QrCode;
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

            WebSocketClient.Instance.RegisterForMessage<GameFinishedResponse>(HandleGameFinished);

            new Thread(() => HunterLoop()).Start();
        }

        private void HandleGameFinished(object obj)
        {
            var response = obj as GameFinishedResponse;
            if (response.winner == "Hunter")
                Navigation.PushAsync(new GameWonView());
            else
                Navigation.PushAsync(new GameLostView());
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

        public async void Catch_Clicked(object sender, EventArgs e)
        {
            var caught_user = (await QrCodeHelper.ReadQrCode(this)).Text;
            await WebSocketClient.Instance.SendMessageAsync(new HunterAction()
            {
                @event = "catch", lobby = GameLogic.Instance.LobbyName, username = GameLogic.Instance.SelfUserName,
                caught = caught_user
            });
        }

        private bool _flashOn = false;
        public async void Flashlight_Clicked(object sender, EventArgs e)
        {
            if (_flashOn)
                await Hardware.TryTurnOffFlashlight();
            else
                await Hardware.TryTurnOnFlashlight();

            _flashOn = !_flashOn;
        }

        private void HunterLoop()
        {
            while (true)
            {
                GameLogic.Instance.SendLocation();
                GameLogic.Instance.GameLength--;
                Device.BeginInvokeOnMainThread(() => { TimeLeft.Text = GameLogic.Instance.GameLength + "s"; });
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

                Device.BeginInvokeOnMainThread(() => { activePreyNr.Text = "" + GameLogic.Instance.ActivePrey; });
                
          
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
                        LabelDebug.Text += $"({p.PlayerName}, {p.Location.Latitude}, {p.Location.Longitude},{Calculations.GetDistance(GameLogic.Instance.Self.Location,p.Location)}, ");
                LabelDebug.TextColor = Color.AntiqueWhite;
                LabelDebug.Text += " "+GameLogic.Instance.GetCompass();
            });
        }

        private void PushOctantString(Label label, int octant)
        {
            label.TextColor = Color.FromHex("#0BD904");
            List<Player> playerList = GameLogic.Instance.GetListOfPlayersInOctant(octant);
            int amount = playerList.Count;
            int intensity = 1;
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
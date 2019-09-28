using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlendIn.Game
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreyGameView : ContentPage
    {

        public double vanishCDValue = 20;
        public double diruptCDValue = 30;

        public double vanishCurrent = 0;
        public double disruptCurrent = 0;

        public PreyGameView()
        {
            BackgroundImage = "gamebg.png";
            InitializeComponent();
            new Thread(() => PreyLoop()).Start();

        }

        private void PreyLoop()
        {
            while (true)
            {
                GameLogic.Instance.SendLocation();
                if (vanishCurrent > 0)
                {
                    vanishCurrent--;
                }
                else
                {
                    //Device.BeginInvokeOnMainThread(() => { ButtonVanish.IsEnabled = true; });
                }

                if (disruptCurrent > 0)
                {
                    disruptCurrent--;
                }
                else
                {
                    //Device.BeginInvokeOnMainThread(() => { ButtonDisrupt.IsEnabled = true; });

                }

                //Device.BeginInvokeOnMainThread(() => { oct_null.Text = GetOctantString(0); });
                //Device.BeginInvokeOnMainThread(() => { oct_eins.Text = GetOctantString(1); });
                //Device.BeginInvokeOnMainThread(() => { oct_zwei.Text = GetOctantString(2); });
                //Device.BeginInvokeOnMainThread(() => { oct_drei.Text = GetOctantString(3); });
                //Device.BeginInvokeOnMainThread(() => { oct_vier.Text = GetOctantString(4); });
                //Device.BeginInvokeOnMainThread(() => { oct_fuenf.Text = GetOctantString(5); });
                //Device.BeginInvokeOnMainThread(() => { oct_sechs.Text = GetOctantString(6); });
                //Device.BeginInvokeOnMainThread(() => { oct_sieben.Text = GetOctantString(7); });

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
            });
        }

        public void Disrupt_Clicked(object sender, EventArgs e)
        {
            /**
            WebSocketClient.Instance.SendMessageAsync(new HunterAction()
            { @event = "stun", lobby = GameLogic.Instance.LobbyName, user = GameLogic.Instance.SelfUserName });
            stunCurrent = stunCDValue;
            ButtonStun.IsEnabled = false;
    */
        }


        public void Vanish_Clicked(object sender, EventArgs e)
        {
            /**
            WebSocketClient.Instance.SendMessageAsync(new HunterAction()
            { @event = "expose", lobby = GameLogic.Instance.LobbyName, user = GameLogic.Instance.SelfUserName });
            exposeCurrent = exposeCDValue;
            ButtonExpose.IsEnabled = false;
    */
        }

        public void Revive_Clicked(object sender, EventArgs e)
        {
            /**
            // todo barcode
            var caught_user = "user";
            WebSocketClient.Instance.SendMessageAsync(new HunterAction()
            {
                @event = "catch",
                lobby = GameLogic.Instance.LobbyName,
                user = GameLogic.Instance.SelfUserName,
                caught = caught_user
            });
    */
        }
    }
}
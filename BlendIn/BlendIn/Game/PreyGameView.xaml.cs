using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using BlendIn.Connection;
using BlendIn.Connection.Responses;
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
            InitializeComponent();

            WebSocketClient.Instance.RegisterForMessage<HunterActionResponse>(HandleHunterAction);

            new Thread(() => PreyLoop()).Start();
        }

        private void HandleHunterAction(object obj)
        {
            var response = obj as HunterActionResponse;
            if (response.@event == "stun")
            {
                Stun();
            }
            else if (response.@event == "expose")
            {
                new Thread(async () =>
                {
                    Device.BeginInvokeOnMainThread(async () => await Hardware.TryTurnOnFlashlight());
                    Thread.Sleep((response.duration ?? 0) * 1000);
                    Device.BeginInvokeOnMainThread(async () => await Hardware.TryTurnOffFlashlight());
                }).Start();
            }
        }

        private void Stun(int duration = 5)
        {
            new Thread(() => ToggleFlashlight(duration)).Start();
            new Thread(() => PlaySound(duration)).Start();
        }

        private async Task ToggleFlashlight(int duration)
        {
            while (duration > 0)
            {
                Device.BeginInvokeOnMainThread(async () => await Hardware.TryTurnOnFlashlight());
                Thread.Sleep(500);
                Device.BeginInvokeOnMainThread(async () => await Hardware.TryTurnOffFlashlight());
                Thread.Sleep(500);
                duration--;
            }
        }

        private async Task PlaySound(int duration)
        {
            SoundController sc = new SoundController();
            sc.audio.Loop = true;
            Device.BeginInvokeOnMainThread(() => sc.audio.Play());
            Thread.Sleep(duration*1000);
            Device.BeginInvokeOnMainThread(() => sc.audio.Stop());
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
            { @event = "stun", lobby = GameLogic.Instance.LobbyName, username = GameLogic.Instance.SelfUserName });
            stunCurrent = stunCDValue;
            ButtonStun.IsEnabled = false;
    */
        }


        public void Vanish_Clicked(object sender, EventArgs e)
        {
            /**
            WebSocketClient.Instance.SendMessageAsync(new HunterAction()
            { @event = "expose", lobby = GameLogic.Instance.LobbyName, username = GameLogic.Instance.SelfUserName });
            exposeCurrent = exposeCDValue;
            ButtonExpose.IsEnabled = false;
    */
        }

        public void Revive_Clicked(object sender, EventArgs e)
        {
            /**
            // todo barcode
            var caught_user = "username";
            WebSocketClient.Instance.SendMessageAsync(new HunterAction()
            {
                @event = "catch",
                lobby = GameLogic.Instance.LobbyName,
                username = GameLogic.Instance.SelfUserName,
                caught = caught_user
            });
    */
        }
    }
}
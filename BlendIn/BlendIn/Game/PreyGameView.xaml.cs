﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using BlendIn.Connection;
using BlendIn.Connection.Responses;
using BlendIn.QrCode;
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

        private void PushOctantString(Label label, int octant)
        {
            label.TextColor = Color.FromHex("#0BD904");
            List<Player> playerList = GameLogic.Instance.GetListOfHuntersInOctant(octant);
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
                else if (distance < 20)
                {
                    intensity += 25;
                }
                else
                {
                    intensity += 15;
                }
                label.Text = GameLogic.Instance.Self.Location.Latitude +" "+ GameLogic.Instance.Self.Location.Longitude +" | "+ player.Location.Latitude + " "+ player.Location.Longitude + " |"+distance + "m";
            }
            if (intensity > 55)
            {
                label.FontSize = 55;
            }
            else
            {
                label.FontSize = intensity;
            }

            if (amount <= 0)
            {
                label.Text = "-";
                label.FontSize = 20;
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

        private void Caught_Clicked(object sender, EventArgs e)
        {
            ImageQrCode.IsVisible = !ImageQrCode.IsVisible;
            ImageQrCode.Source = QrCodeHelper.img;
        }
    }
}
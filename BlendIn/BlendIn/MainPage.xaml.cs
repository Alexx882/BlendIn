using BlendIn.Game;
using BlendIn.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlendIn.Tests;
using Xamarin.Forms;

namespace BlendIn
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Hardware.registerCompass(AdjustCompassFunction);
            Hardware.ToggleCompass();
        }

        public double AdjustCompassFunction(double d)
        {
            Debuglabel.Text = $"Reading: {d} degrees";
            return d;
        }

        private void ButtonStartGame_Clicked(object sender, EventArgs e)
        {
            GameView v = new GameView();
            ((App) App.Current).NavigationPage.PushAsync(v);
        }

        private void ButtonFlashlight_Clicked(object sender, EventArgs e)
        {
            var v = new FlashlightTestView();
            ((App) App.Current).NavigationPage.PushAsync(v);
        }

        private void ButtonGps_Clicked(object sender, EventArgs e)
        {
            var v = new GpsView();
            ((App)App.Current).NavigationPage.PushAsync(v);
        }

        private void ButtonQr_Clicked(object sender, EventArgs e)
        {
            var v = new QrTestView();
            ((App)App.Current).NavigationPage.PushAsync(v);
        }

        private void ButtonWebSocket_Clicked(object sender, EventArgs e)
        {
            var v = new WebSocketTestView();
            Navigation.PushAsync(v);
        }

        private void JoinGame_Clicked(object sender, EventArgs e)
        {
            //string joinGameCode = gameCode.Text;
            View1hunter v = new View1hunter();
            ((App)App.Current).NavigationPage.PushAsync(v);
        }

        private void NewGame_Clicked(object sender, EventArgs e)
        {
            View1prey v = new View1prey();
            Navigation.PushAsync(v);
        }
    }
}

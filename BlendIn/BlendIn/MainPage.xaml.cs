using BlendIn.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlendIn.Tests;
using Xamarin.Forms;
using Xamarin.Essentials;

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
            GameLogic.personalCompassDegrees = d;
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

        private void Location_Test_Clicked(object sender, EventArgs e)
        {
            Location boston = new Location(42.358056, -71.063611);
            Location sanFrancisco = new Location(37.783333, -122.416667);
            double miles = Location.CalculateDistance(boston, sanFrancisco, DistanceUnits.Miles);
            //LocationDebugLabel.Text = "DI: " + miles +" DS: " + Calculations.GetDistance(boston,sanFrancisco)+ " Octant: " + Calculations.GetOctantBetweenTwoPoints(boston, sanFrancisco)+ " B: "+Calculations.getFinalBearing(boston,sanFrancisco);
            LocationDebugLabel.Text = "Distance "+ Calculations.GetDistance(boston, sanFrancisco) + "m in Octant: " + Calculations.GetOctantBetweenTwoPoints(boston,sanFrancisco,GameLogic.personalCompassDegrees);
        }
    }
}

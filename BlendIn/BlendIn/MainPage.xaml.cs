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
using Xamarin.Essentials;

namespace BlendIn
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private GameLogic _gameLogic;
     
        public MainPage()
        {
            BackgroundImage = "bg.png";
            InitializeComponent();
            _gameLogic = GameLogic.Instance;
        }
        /*
        private void ButtonStartGame_Clicked(object sender, EventArgs e)
        {
            HunterGameView v = new HunterGameView();
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


            Location a = new Location(46.933502, 13.872430);
            Location b = new Location(46.933942, 13.872419);
            Location c = new Location(46.933649, 13.871893);

            //double miles = Location.CalculateDistance(boston, sanFrancisco, DistanceUnits.Miles);
            //LocationDebugLabel.Text = "DI: " + miles +" DS: " + Calculations.GetDistance(boston,sanFrancisco)+ " Octant: " + Calculations.GetOctantBetweenTwoPoints(boston, sanFrancisco)+ " B: "+Calculations.getFinalBearing(boston,sanFrancisco);

            LocationDebugLabel1.Text = "AB Distance "+ Calculations.GetDistance(a, b) + "m in Octant: " + Calculations.GetOctantBetweenTwoPoints(a,b,180);
            LocationDebugLabel2.Text = "BC Distance " + Calculations.GetDistance(b, c) + "m in Octant: " + Calculations.GetOctantBetweenTwoPoints(b, c,180);
            LocationDebugLabel3.Text = "CA Distance " + Calculations.GetDistance(a, c) + "m in Octant: " + Calculations.GetOctantBetweenTwoPoints(c, a, 180);


            //SoundController sound = new SoundController();

            // LocationDebugLabel.Text = "Distance "+ Calculations.GetDistance(boston, sanFrancisco) + "m in Octant: " + Calculations.GetOctantBetweenTwoPoints(boston,sanFrancisco, _gameLogic.personalCompassDegrees);
            //SoundController sound = new SoundController();
            //sound.audio.Loop = true;
            //sound.audio.Play();
            //HunterGameView v = new HunterGameView();
            //Navigation.PushAsync(v);
        }
        */

        private void JoinGame_Clicked(object sender, EventArgs e)
        {
            ViewJoinGame v = new ViewJoinGame();
            Navigation.PushAsync(v);
        }

        private void NewGame_Clicked(object sender, EventArgs e)
        {
            ViewNewGame v = new ViewNewGame();
            Navigation.PushAsync(v);
        }
    }
}

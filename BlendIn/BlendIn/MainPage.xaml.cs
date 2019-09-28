﻿using BlendIn.Game;
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
            InitializeComponent();
            _gameLogic = GameLogic.Instance;
        }

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
            Location boston = new Location(42.358056, -71.063611);
            Location sanFrancisco = new Location(37.783333, -122.416667);
            double miles = Location.CalculateDistance(boston, sanFrancisco, DistanceUnits.Miles);
            //LocationDebugLabel.Text = "DI: " + miles +" DS: " + Calculations.GetDistance(boston,sanFrancisco)+ " Octant: " + Calculations.GetOctantBetweenTwoPoints(boston, sanFrancisco)+ " B: "+Calculations.getFinalBearing(boston,sanFrancisco);
            // LocationDebugLabel.Text = "Distance "+ Calculations.GetDistance(boston, sanFrancisco) + "m in Octant: " + Calculations.GetOctantBetweenTwoPoints(boston,sanFrancisco, _gameLogic.personalCompassDegrees);
            SoundController sound = new SoundController();
            //sound.audio.Loop = true;
            sound.audio.Play();
            HunterGameView v = new HunterGameView();
            Navigation.PushAsync(v);
        }

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

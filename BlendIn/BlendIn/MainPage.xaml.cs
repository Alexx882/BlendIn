using BlendIn.Game;
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
    }
}

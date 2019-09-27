using BlendIn.Game;
using BlendIn.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void NewGame_Clicked(object sender, EventArgs e)
        {
            View1prey v = new View1prey();
            ((App)App.Current).NavigationPage.PushAsync(v);

            
        }

        private void JoinGame_Clicked(object sender, EventArgs e)
        {
            //string joinGameCode = gameCode.Text;
            View1hunter v = new View1hunter();
            ((App)App.Current).NavigationPage.PushAsync(v);


        }
    }
}

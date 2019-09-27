using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlendIn.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View1prey : ContentPage
    {
        public View1prey()
        {
            InitializeComponent();
        }

        private void Go_Clicked(object sender, EventArgs e)
        {
            //string joinGameCode = gameCode.Text;
            View1hunter v = new View1hunter();
            ((App)App.Current).NavigationPage.PushAsync(v);


        }
    }
}
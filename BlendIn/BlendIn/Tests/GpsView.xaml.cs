using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Essentials;

namespace BlendIn.Tests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GpsView : ContentPage
    {
        public GpsView()
        {
            InitializeComponent();
        }

        private async void ButtonGetLocation_OnClicked(object sender, EventArgs e)
        {
            var location = await Hardware.GetLocation();
            LocationLabel.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
        }
    }
}
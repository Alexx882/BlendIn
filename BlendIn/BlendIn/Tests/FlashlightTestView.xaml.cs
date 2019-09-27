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
    public partial class FlashlightTestView : ContentPage
    {
        public FlashlightTestView()
        {
            InitializeComponent();
        }

        private async void ButtonOn_OnClicked(object sender, EventArgs e)
        {
            await Hardware.TryTurnOnFlashlight();
        }

        private async void ButtonOff_OnClicked(object sender, EventArgs e)
        {
            await Hardware.TryTurnOffFlashlight();
        }
    }
}
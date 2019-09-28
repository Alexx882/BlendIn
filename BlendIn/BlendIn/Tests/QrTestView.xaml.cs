using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlendIn.QrCode;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace BlendIn.Tests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrTestView : ContentPage
    {
        public QrTestView()
        {
            InitializeComponent();
        }

        private async void btnScan_Clicked(object sender, EventArgs e)
        {
            var res = await QrCodeHelper.ReadQrCode(this);
            LabelResult.Text = res.Text;
        }

        private async void btnImg_Clicked(object sender, EventArgs e)
        {
            await QrCodeHelper.CraeteQrCode("test alex");
            ImageQr.Source = QrCodeHelper.img;
        }
    }
}
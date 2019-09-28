using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlendIn.Connection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlendIn.Tests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebSocketTestView : ContentPage
    {

        public WebSocketTestView()
        {
            InitializeComponent();
        }
        private void Button_OnClicked(object sender, EventArgs e)
        {
            WebSocketClient.Instance.ConnectToServerAsync();
        }

        private void ButtonIp_OnClicked(object sender, EventArgs e)
        {
            WebSocketClient.ip = "104.248.134.91";
        }
    }
}
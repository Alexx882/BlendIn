using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlendIn.Connection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlendIn.Game
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameWonView : ContentPage
    {
        public GameWonView()
        {
            InitializeComponent();
            WebSocketClient.Instance.Disconnect();
        }
    }
}
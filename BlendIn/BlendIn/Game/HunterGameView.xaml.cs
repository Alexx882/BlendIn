using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlendIn.Game
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HunterGameView : ContentPage
    {
        public HunterGameView()
        {
            InitializeComponent();
            new Thread(() => HunterLoop()).Start();

        }

        private void HunterLoop()
        {
            while (true)
            {

                Device.BeginInvokeOnMainThread(() => { oct_null.Text = GetOctantString(0); });
                Device.BeginInvokeOnMainThread(() => { oct_eins.Text = GetOctantString(1); });
                Device.BeginInvokeOnMainThread(() => { oct_zwei.Text = GetOctantString(2); });
                Device.BeginInvokeOnMainThread(() => { oct_drei.Text = GetOctantString(3); });
                Device.BeginInvokeOnMainThread(() => { oct_vier.Text = GetOctantString(4); });
                Device.BeginInvokeOnMainThread(() => { oct_fuenf.Text = GetOctantString(5); });
                Device.BeginInvokeOnMainThread(() => { oct_sechs.Text = GetOctantString(6); });
                Device.BeginInvokeOnMainThread(() => { oct_sieben.Text = GetOctantString(7); });


                Thread.Sleep(1000);
            }
        }

        private string GetOctantString(int octant)
        {
            string s = "";
            for (int i = 0; i < GameLogic.GetAmountOfPlayersInOctant(octant); i++)
            {
                s += "*";
            }
            return s;
        }
    }

}
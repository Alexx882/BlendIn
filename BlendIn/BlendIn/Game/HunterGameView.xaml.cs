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
        public double exposeCDValue = 20;
        public double stunCDValue = 30;

        public double exposeCurrent = 0;
        public double stunCurrent = 0;


        public HunterGameView()
        {
            InitializeComponent();
            new Thread(() => HunterLoop()).Start();

        }

        public void Stun_Clicked(object sender, EventArgs e)
        {
            //TODO Do Something Usefull aka actually stun here
            stunCurrent = stunCDValue;
            ButtonStun.IsEnabled = false;

        }


        public void Expose_Clicked(object sender, EventArgs e)
        {
            //TODO Do Something Usefull aka actually expose here
            exposeCurrent = exposeCDValue;
            ButtonExpose.IsEnabled = false;

        }

        public void Catch_Clicked(object sender, EventArgs e)
        {
            //TODO Do Something Usefull aka actually catch here

        }

        private void HunterLoop()
        {
            while (true)
            {
                if (exposeCurrent > 0)
                {
                    exposeCurrent--;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() => { ButtonExpose.IsEnabled = true; });
                }

                if (stunCurrent > 0)
                {
                    stunCurrent--;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() => { ButtonStun.IsEnabled = true; });

                }

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
            for (int i = 0; i < GameLogic.Instance.GetAmountOfPlayersInOctant(octant); i++)
            {
                s += "*";
            }
            return s;
        }
    }

}
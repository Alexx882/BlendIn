using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlendIn.Connection.Responses;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlendIn.Game
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimerView : ContentPage
    {
        public TimerView(TimerResponse response)
        {
            InitializeComponent();
            LabelInfo.Text = response.hunter_username;
        }
    }
}
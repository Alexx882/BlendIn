using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Collections.Generic;
using BlendIn.Connection;

namespace BlendIn
{
    public class GameLogic
    {

        public static List<Player> players = new List<Player>();
        public static Player mySelf;
        public static double personalCompassDegrees;
        public bool iAmHunter => WebSocketClient.Instance.IsHunter;

        public static double GetAmountOfPlayersInOctant(int octant)
        {
            int amountOfPlayers = 0;
            foreach (Player player in players)
            {
                if(Calculations.GetOctantBetweenTwoPoints(mySelf.location, player.location, personalCompassDegrees) == octant)
                {
                    amountOfPlayers += 1;
                }
            }
            return amountOfPlayers;
        }

    }
}

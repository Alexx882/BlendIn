using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Collections.Generic;
using BlendIn.Connection;
using BlendIn.Connection.Messages;
using BlendIn.Connection.Responses;

namespace BlendIn.Game
{
    public class GameLogic
    {
        public List<Player> Players { get; set; }
        public Player Self { get; set; }

        public double personalCompassDegrees;
        public bool SelfIsHunter => Self.IsHunter ?? false;
        public string SelfUserName => Self.PlayerName; 

        public string LobbyName { get; set; } 

        private static GameLogic _instance;

        public static GameLogic Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new GameLogic();
                return _instance;
            }
        }

        private GameLogic()
        {
            WebSocketClient.Instance.RegisterForMessage<TickResponse>(HandleTick);
            Players = new List<Player>();
        }

        private void HandleTick(object obj)
        {
            var response = obj as TickResponse;
            Players = response.userlist;
        }
        public async void SendLocation()
        {
            var location = await Hardware.GetLocation();
            await WebSocketClient.Instance.SendMessageAsync(new LocationMessage()
            {
                @event = "location", lobby = LobbyName, username = SelfUserName, latitude = location?.Latitude,
                longitude = location?.Longitude
            });
        }

        public double GetAmountOfPlayersInOctant(int octant)
        {
            int amountOfPlayers = 0;
            foreach (Player player in Players)
            {
                if(Calculations.GetOctantBetweenTwoPoints(Self.Location, player.Location, personalCompassDegrees) == octant)
                {
                    amountOfPlayers += 1;
                }
            }
            return amountOfPlayers;
        }
        
    }
}

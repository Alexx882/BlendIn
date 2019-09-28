using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Collections.Generic;
using System.Linq;
using BlendIn.Connection;
using BlendIn.Connection.Messages;
using BlendIn.Connection.Responses;

namespace BlendIn.Game
{
    public class GameLogic
    {
        /// <summary>
        /// Used for a lot of dirty stuff, eg. evaluating <see cref="Player.IsHunter"/>.
        /// </summary>
        public static string HunterName { get; set; }

        public List<Player> Players { get; set; }
        public Player Self { get; set; }

        private double _personalCompassDegrees;
        public bool SelfIsHunter => Self.IsHunter ?? false;
        public string SelfUserName => Self.PlayerName;
        public int ActivePrey => Players.Count() - 1;

        public string LobbyName { get; set; }

        private static GameLogic _instance;

        public static GameLogic Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameLogic();
                return _instance;
            }
        }

        private GameLogic()
        {
            WebSocketClient.Instance.RegisterForMessage<TickResponse>(HandleTick);
            Players = new List<Player>();

            Hardware.registerCompass(AdjustCompassFunction);
            Hardware.ToggleCompass();
        }

        public double AdjustCompassFunction(double d)
        {
            _personalCompassDegrees = d;
            return d;
        }

        private void HandleTick(object obj)
        {
            var response = obj as TickResponse;
            Players = response.userlist;
        }

        public async void SendLocation()
        {
            var location = await Hardware.GetLocation();
            GameLogic.Instance.Self.Location = location;
            await WebSocketClient.Instance.SendMessageAsync(new LocationMessage()
            {
                @event = "location", lobby = LobbyName, username = SelfUserName, latitude = location?.Latitude,
                longitude = location?.Longitude
            });
        }

        public double GetAmountOfPlayersInOctant(int octant)
        {
            int amountOfPlayers = 0;
            foreach (Player player in Players.Where(p => p.PlayerName != SelfUserName))
            {
                if (Calculations.GetOctantBetweenTwoPoints(Self.Location, player.Location, _personalCompassDegrees) ==
                    octant)
                {
                    amountOfPlayers += 1;
                }
            }

            return amountOfPlayers;
        }

        public List<Player> GetListOfPlayersInOctant(int octant)
        {
            List<Player> playerList = new List<Player>();
            foreach (Player player in Players.Where(p => p.PlayerName != SelfUserName))
            {
                if (Calculations.GetOctantBetweenTwoPoints(Self.Location, player.Location, _personalCompassDegrees) ==
                    octant)
                {
                    playerList.Add(player);
                }
            }

            return playerList;
        }

        public List<Player> GetListOfHuntersInOctant(int octant)
        {
            List<Player> playerList = new List<Player>();
            foreach (Player player in Players.Where(p => p.IsHunter ?? false))
            {
                if (Calculations.GetOctantBetweenTwoPoints(Self.Location, player.Location, _personalCompassDegrees) ==
                    octant)
                {
                    playerList.Add(player);
                }
            }

            return playerList;
        }

        public double GetCompass()
        {
            return this._personalCompassDegrees;
        }
    }
}
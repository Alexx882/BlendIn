using System;
using System.Runtime.Serialization;
using BlendIn.Connection;
using BlendIn.Connection.Responses;
using Xamarin.Essentials;

namespace BlendIn.Game
{
    public class Player
    {
        private string _name = null;

        public string name
        {
            get => _name;
            set => _name = value;
        }

        public string PlayerName
        {
            get => _name;
            set => _name = value;
        }

        private Location _location = new Location();

        public double? lat
        {
            get => _location.Latitude;
            set => _location.Latitude = value ?? 0;
        }

        public double? @long
        {
            get => _location.Longitude;
            set => _location.Longitude = value ?? 0;
        }

        public Location Location
        {
            get => _location;
            set => _location = value;
        }

        public bool? IsHunter { get; set; }

        public Player()
        {
        }
    }
}
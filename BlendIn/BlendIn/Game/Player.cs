

using BlendIn.Connection;
using Xamarin.Essentials;

namespace BlendIn
{
    public class Player
    {

        public Location location;
        public string playerName;
        public bool isHunter;

        Player(string playerName, Location location)
        {
            this.playerName = playerName;
            this.location = location;
        }

    }
}

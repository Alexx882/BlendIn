

using Xamarin.Essentials;

namespace BlendIn
{
    public class Player
    {

        public Location location;
        public string playerName;

        Player(string playerName, Location location)
        {
            this.playerName = playerName;
            this.location = location;
        }

    }
}

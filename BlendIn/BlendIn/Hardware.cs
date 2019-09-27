using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BlendIn
{
    class Hardware
    {
        public static async Task<bool> TryTurnOnFlashlight()
        {
            try
            {
                await Flashlight.TurnOnAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> TryTurnOffFlashlight()
        {
            try
            {
                await Flashlight.TurnOffAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns null if something didnt work.
        /// </summary>
        /// <returns></returns>
        public static async Task<Location> GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                Location location = await Geolocation.GetLocationAsync(request);

                return location;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

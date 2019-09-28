using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BlendIn
{
    public class Hardware
    {
        public static SensorSpeed speed = SensorSpeed.UI; //Describes how often the Sensor is checked
        public static Func<double,double> adjustCompass;

        public static void registerCompass(Func<double,double> AdjustCompassFunction)
        {
            // Register for reading changes, be sure to unsubscribe when finished
            Compass.ReadingChanged += Compass_ReadingChanged;
            adjustCompass = AdjustCompassFunction;
        }

        internal static void registerCompass(Action<double> p)
        {
            throw new NotImplementedException();
        }

        public static void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            var data = e.Reading;
            adjustCompass(data.HeadingMagneticNorth);
        }

        public static void ToggleCompass()
        {
            try
            {
                if (Compass.IsMonitoring)
                    Compass.Stop();
                else
                    Compass.Start(speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Some other exception has occurred
            }
        }

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

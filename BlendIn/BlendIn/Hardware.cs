using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

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

        /// <summary>
        /// Opens a view for scanning the QR code.
        /// </summary>
        /// <param name="view">The calling Page to push the scanner over.</param>
        /// <returns></returns>
        public static Task<ZXing.Result> ReadQrCode(Page view)
        {
            var scannerPage = new ZXingScannerPage();

            var task = new TaskCompletionSource<ZXing.Result>();

            scannerPage.OnScanResult += (result) => {
                scannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() => view.Navigation.PopAsync());

                task.SetResult(result);
            };

            view.Navigation.PushAsync(scannerPage);
            return task.Task;
        }
    }
}

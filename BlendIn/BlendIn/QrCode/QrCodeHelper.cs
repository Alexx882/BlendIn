using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace BlendIn.QrCode
{
    public class QrCodeHelper
    {
        public static ImageSource img = null;

        public static async Task<ImageSource> CreateQrCode(string username, int res = 300)
        {
            HttpClient _client = new HttpClient();

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"https://api.qrserver.com/v1/create-qr-code/?size={res}x{res}&data={username}"),
                Method = HttpMethod.Get,
            };
            var response = await _client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBytes = await response.Content.ReadAsByteArrayAsync();
                img = ImageSource.FromStream(() => new MemoryStream(responseBytes));
                return img;
            }

            return null;
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

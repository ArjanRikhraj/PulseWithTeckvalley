using System;
using Foundation;
using Pulse.iOS;
using UIKit;
using Xamarin.Forms;
using ZXing;

[assembly: Dependency(typeof(QRCodeService))]
namespace Pulse.iOS
{
	public class QRCodeService : IQRCode
	{

		public async void Share(string qrData)
		{
			try
			{
				var barcodeWriter = new ZXing.Mobile.BarcodeWriter
				{
					Format = BarcodeFormat.QR_CODE,
					Options = new ZXing.Common.EncodingOptions
					{
						Width = 300,
						Height = 300,
						Margin = 1
					}
				};
				var item = barcodeWriter.Write(qrData);
				var activityItems = new[] { item };
				var activityController = new UIActivityViewController(activityItems, null);

				var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

				while (topController.PresentedViewController != null)
				{
					topController = topController.PresentedViewController;
				}

				topController.PresentViewController(activityController, true, () => { });
			}
			catch (Exception)
			{
				await App.Instance.Alert("Problem in sharing QR code in this device", Constant.AlertTitle, Constant.Ok);
			}
		}
	}
}

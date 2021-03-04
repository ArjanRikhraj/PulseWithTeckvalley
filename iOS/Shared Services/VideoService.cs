using System;
using System.Drawing;
using System.IO;
using AVFoundation;
using Foundation;
using UIKit;
using Pulse.iOS;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(VideoService))]
namespace Pulse.iOS
{
	public class VideoService:IVideoService
	{

		public void GetThumbnail(Stream stream, string filpath)
		{
			var asset = AVAsset.FromUrl(NSUrl.FromFilename(filpath));
			var imageGenerator = AVAssetImageGenerator.FromAsset(asset);
			imageGenerator.AppliesPreferredTrackTransform = true;
			var actualTime = asset.Duration;
			CoreMedia.CMTime cmTime = new CoreMedia.CMTime(1, 60);
			NSError error;
			var imageRef = imageGenerator.CopyCGImageAtTime(cmTime, out actualTime, out error);
			if (imageRef == null)
				return ;
			var image = UIImage.FromImage(imageRef);
			App.iOSThumbnail = GetImageSourceFromUIImage(image);
			App.iOSImageThumbnail = image.AsPNG().ToArray();
		}

		public void GetImageThumbnail(byte[] imageData, double width, double height,System.IO.Stream stream,string filePath)
		{
			// Original image properties
			UIImage originalImage = ImageFromByteArray(imageData);
			var sourceSize = originalImage.Size;

			// Resized image max width and height
			float maxWidth = (float)width;
			float maxHeight = (float)height;

			// Calculate ratio
			var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
			if (maxResizeFactor > 1)
			{
				App.iOSImageThumbnail= originalImage.AsPNG().ToArray();
			}
			else
			{
				// Resized image dimensions
				var newImgWidth = (float)maxResizeFactor * (float)sourceSize.Width;
				var newImgHeight = (float)maxResizeFactor * (float)sourceSize.Height;

				// Resized image
				UIGraphics.BeginImageContext(new SizeF(newImgWidth, newImgHeight));
				originalImage.Draw(new RectangleF(0, 0, newImgWidth, newImgHeight));
				var resultImage = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();

				if (resultImage != null)
				{
					App.iOSImageThumbnail = resultImage.AsPNG().ToArray();
				}

			}
		}

		public static void rotateBitmap(Stream fileStream, double width, double height)
		{
	
		}

		public static UIKit.UIImage ImageFromByteArray(byte[] data)
		{
			if (data == null)
			{
				return null;
			}

			UIKit.UIImage image;
			try
			{
				image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
			}
			catch (Exception)
			{
				return null;
			}
			return image;
		}

		internal static ImageSource GetImageSourceFromUIImage(UIImage uiImage)
		{
			return uiImage == null ? null : ImageSource.FromStream(() => uiImage.AsJPEG(0.75f).AsStream());
		}
	}
}


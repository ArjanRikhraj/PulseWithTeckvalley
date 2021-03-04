using System;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.IO;
using CoreGraphics;
using System.Drawing;
using Pulse.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(IOSImageRotationService))]
namespace Pulse.iOS
{
	public class IOSImageRotationService : IiOSImageRotationService
	{
		#region IiOSImageRotationService implementation
		public async Task<Stream> GetCorrectImageRotation(ImageSource imageSource)
		{
			Stream returnVal = null;
			UIImage image = null;
			using (image = await GetImageFromImageSource(imageSource))
			{
				if (image != null)
				{
					image = ScaleAndRotateImage(image, image.Orientation);
					if (image != null)
					{
						returnVal = image.AsPNG().AsStream();
					}
				}
			}
			return returnVal;
		}
		#endregion
		private Task<UIImage> GetImageFromImageSource(ImageSource imageSource)
		{
			IImageSourceHandler handler = null;

			if (imageSource is FileImageSource)
			{
				handler = new FileImageSourceHandler();
			}
			else if (imageSource is StreamImageSource)
			{
				handler = new StreamImagesourceHandler(); // sic
			}
			else if (imageSource is UriImageSource)
			{
				handler = new ImageLoaderSourceHandler(); // sic
			}
			else
			{
				throw new NotImplementedException();
			}

			return handler.LoadImageAsync(imageSource);
		}
		private UIImage ScaleAndRotateImage(UIImage imageIn, UIImageOrientation orIn)
		{
			var oldImageSize = imageIn.AsPNG().ToArray();
			if (oldImageSize.Length > (1024 * 1024))
			{
				var resizedImage = ResizeImage(imageIn); ;
				imageIn = resizedImage; ;
			}
			return imageIn;
			//int kMaxResolution = 2048;

			//CGImage imgRef = imageIn.CGImage;
			//float width = imgRef.Width;
			//float height = imgRef.Height;
			//CGAffineTransform transform = CGAffineTransform.MakeIdentity();
			//RectangleF bounds = new RectangleF(0, 0, width, height);

			//if (width > kMaxResolution || height > kMaxResolution)
			//{
			//	float ratio = width / height;

			//	if (ratio > 1)
			//	{
			//		bounds.Width = kMaxResolution;
			//		bounds.Height = bounds.Width / ratio;
			//	}
			//	else
			//	{
			//		bounds.Height = kMaxResolution;
			//		bounds.Width = bounds.Height * ratio;
			//	}
			//}

			//float scaleRatio = bounds.Width / width;
			//SizeF imageSize = new SizeF(width, height);
			//UIImageOrientation orient = orIn;
			//float boundHeight;

			//switch (orient)
			//{
			//	case UIImageOrientation.Up:                                        //EXIF = 1
			//		transform = CGAffineTransform.MakeIdentity();
			//		break;

			//	case UIImageOrientation.UpMirrored:                                //EXIF = 2
			//		transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0f);
			//		transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
			//		break;

			//	case UIImageOrientation.Down:                                      //EXIF = 3
			//		transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
			//		transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
			//		break;

			//	case UIImageOrientation.DownMirrored:                              //EXIF = 4
			//		transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
			//		transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
			//		break;

			//	case UIImageOrientation.LeftMirrored:                              //EXIF = 5
			//		boundHeight = bounds.Height;
			//		bounds.Height = bounds.Width;
			//		bounds.Width = boundHeight;
			//		transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
			//		transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
			//		transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
			//		break;

			//	case UIImageOrientation.Left:                                      //EXIF = 6
			//		boundHeight = bounds.Height;
			//		bounds.Height = bounds.Width;
			//		bounds.Width = boundHeight;
			//		transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
			//		transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
			//		break;

			//	case UIImageOrientation.RightMirrored:                             //EXIF = 7
			//		boundHeight = bounds.Height;
			//		bounds.Height = bounds.Width;
			//		bounds.Width = boundHeight;
			//		transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
			//		transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
			//		break;

			//	case UIImageOrientation.Right:                                     //EXIF = 8
			//		boundHeight = bounds.Height;
			//		bounds.Height = bounds.Width;
			//		bounds.Width = boundHeight;
			//		transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
			//		transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
			//		break;

			//	default:
			//		throw new Exception("Invalid image orientation");
			//		break;
			//}

			//UIGraphics.BeginImageContext(bounds.Size);

			//CGContext context = UIGraphics.GetCurrentContext();

			//if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
			//{
			//	context.ScaleCTM(-scaleRatio, scaleRatio);
			//	context.TranslateCTM(-height, 0);
			//}
			//else
			//{
			//	context.ScaleCTM(scaleRatio, -scaleRatio);
			//	context.TranslateCTM(0, -height);
			//}

			//context.ConcatCTM(transform);
			//context.DrawImage(new RectangleF(0, 0, width, height), imgRef);

			//UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
			//UIGraphics.EndImageContext();

			//return imageCopy;
		}

		public UIImage ResizeImage(UIImage originalImage)
		{
			var originalBytes = originalImage.AsPNG().ToArray();
			// Original image properties
			var sourceSize = originalImage.Size;

			// Resized image max width and height
			float maxWidth = 1242;//12422
			float maxHeight = 2208;//22088
			float newWidth = (float)sourceSize.Width;
			float newHeight = (float)sourceSize.Height;

			if (sourceSize.Width > maxWidth)
			{
				newWidth = maxWidth;
				newHeight = (newWidth * (float)sourceSize.Height) / (float)sourceSize.Width; ;
			}
			else if (sourceSize.Height > maxHeight)
			{
				newHeight = maxHeight;
				newWidth = (newHeight * (float)sourceSize.Width) / (float)sourceSize.Height;
			}

			// Resized image
			UIGraphics.BeginImageContext(new SizeF(newWidth, newHeight));
			originalImage.Draw(new RectangleF(0, 0, newWidth, newHeight));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			var reducedBytes = resultImage.AsPNG().ToArray();
			// Return resized image
			return resultImage;
		}
	}
}


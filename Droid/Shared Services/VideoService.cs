using System.IO;
using Android.Graphics;
using Android.Media;
using Android.Provider;
using Pulse.Droid;


[assembly: Xamarin.Forms.Dependency(typeof(VideoService))]

namespace Pulse.Droid
{
	public class VideoService : IVideoService
	{
		readonly Bitmap.CompressFormat outputFormat = Bitmap.CompressFormat.Jpeg;
		readonly Bitmap.CompressFormat outFormat = Bitmap.CompressFormat.Jpeg;
		public void GetThumbnail(System.IO.Stream stream, string filepath)
		{
			if (stream != null)
			{
				Bitmap bmThumbnail = ThumbnailUtils.CreateVideoThumbnail(filepath, ThumbnailKind.MiniKind);
				using (MemoryStream ms = new MemoryStream())
				{

					if (bmThumbnail != null)
					{
						bmThumbnail.Compress(outputFormat, 75, ms);
						App.DroidThumbnail = ms.GetBuffer();
					}
				}
			}
		}


		public void GetImageThumbnail(byte[] imageData, double width, double height, System.IO.Stream imageStream,string filepath)
		{
			Bitmap resized;

			using (resized = ThumbnailUtils.ExtractThumbnail(BitmapFactory.DecodeStream(imageStream), (int)width, (int)height, ThumnailExtractOptions.RecycleInput))
			{
				resized = rotateBitmap(filepath, resized,  width, height);
				using (MemoryStream ms = new MemoryStream())
				{
					if (resized != null)
					{
						resized.Compress(outFormat, 100, ms);
						App.DroidImageThumbnail = ms.GetBuffer();
					}
				}
			}
		}

		public Bitmap rotateBitmap(string filepath, Bitmap bitmap, double width, double height)
		{
			ExifInterface exifData = new ExifInterface(filepath);

			int orientation = exifData.GetAttributeInt(ExifInterface.TagOrientation, 1);
			switch (orientation)
			{
				case 6:
					bitmap = CreateRotateBitmap(filepath, bitmap, (int)width, (int)height, 90);
					break;
				case 3:
					bitmap = CreateRotateBitmap(filepath, bitmap, (int)width, (int)height, 180);
					break;
				case 8:
					bitmap = CreateRotateBitmap(filepath, bitmap, (int)width, (int)height, 270);
					break;
				default:
					break;
			}
			return bitmap;
		}

		public Bitmap CreateRotateBitmap(string filepath, Bitmap bitmap, double width, double height, int angle)
		{
			Matrix matrix = new Matrix();
			matrix.PostRotate(angle);
			return bitmap = Bitmap.CreateBitmap(bitmap, 0, 0, (int)width, (int)height, matrix, true);
		}
	}
}

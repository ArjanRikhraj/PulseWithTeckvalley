
using System.IO;

namespace Pulse
{

	public interface IVideoService
	{
		void GetThumbnail(Stream stream, string filepath);
		void GetImageThumbnail(byte[] imageData, double width, double height,System.IO.Stream imageStream,string filepath);
	}
}



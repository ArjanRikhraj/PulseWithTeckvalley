using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pulse
{
    public interface IiOSImageRotationService
    {
        Task<Stream> GetCorrectImageRotation(ImageSource imageSource);

    }
}

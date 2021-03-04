using System;
using System.Threading.Tasks;

namespace Pulse
{
    public interface IVideoPicker
    {
        Task<string> GetVideoFileAsync();
    }
}

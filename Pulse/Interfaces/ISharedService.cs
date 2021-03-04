namespace Pulse
{
    public interface ISharedService
    {
        string GetVersionCode();
		void UpdateApp();
        void CloseApp();
    }
}

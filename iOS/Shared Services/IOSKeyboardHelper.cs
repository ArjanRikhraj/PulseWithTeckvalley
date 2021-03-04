using Pulse.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSKeyboardHelper))]
namespace Pulse.iOS
{
	public class iOSKeyboardHelper : IKeyboardHelper
	{
		public void HideKeyboard()
		{
			UIApplication.SharedApplication.KeyWindow.EndEditing(true);
		}
	}

}

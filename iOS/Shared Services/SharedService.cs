using System;
using System.Collections.Generic;
using System.Text;
using Pulse;
using UIKit;
using CoreGraphics;
using System.Drawing;
using Foundation;
using Xamarin.Forms;
using System.Threading;

[assembly: Xamarin.Forms.Dependency (typeof(Pulse.iOS.SharedService))]
namespace Pulse.iOS
{
    public class SharedService : ISharedService
    {

		public string GetVersionCode()
		{    
			return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();


				
		}
		public void UpdateApp()
		{
			//Device.OpenUri(new Uri(string.Format(Constant.iTuneURL)));
			Device.OpenUri(new Uri("http://itunes.apple.com/[country]/app/[App –Name]/id[App-ID]?mt=8"));
					
		}
		public void CloseApp()
		{
			Thread.CurrentThread.Abort();
		}
      
    }
}

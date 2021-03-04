
using Android.Content;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(Pulse.Droid.SharedService))]
namespace Pulse.Droid
{
    public class SharedService : ISharedService
    {
        public string GetVersionCode()
        {            
             return MainActivity.APPLICATION_CONTEXT.PackageManager.GetPackageInfo(MainActivity.APPLICATION_CONTEXT.PackageName, 0).VersionCode.ToString();
        }
        public void UpdateApp()
        {            
            string appPackageName = Android.App.Application.Context.PackageName;

            Intent shareIntent = new Intent(Intent.ActionSend);
            Forms.Context.StartActivity(Intent.CreateChooser(shareIntent, "Play Store"));
            try
            {
                Forms.Context.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + appPackageName)));
            }
            catch (ActivityNotFoundException anfe)
            {
                Forms.Context.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + appPackageName)));
            }
        }
   
        public void CloseApp()
        {            
            var activity = Forms.Context as Android.App.Activity;
            if (activity != null)
                activity.FinishAffinity();
            Java.Lang.JavaSystem.Exit(0);
        }        
    }
}
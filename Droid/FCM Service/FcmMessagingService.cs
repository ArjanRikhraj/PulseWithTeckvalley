using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Firebase.Messaging;

namespace Pulse.Droid
{
	[Service] 	[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
	public class FcmMessagingService : FirebaseMessagingService
	{
		string messageData;
        public static  String NOTIFICATION_CHANNEL_ID = "10001";

        public override void OnMessageReceived(RemoteMessage message) 		{
			string title = string.Empty;

			string id = string.Empty;
			string notificationMessage = string.Empty;
			if (message.Data != null && message.Data.Count > 0)
			{
				if (message.Data.ContainsKey("title"))
				{
					title = message.Data["title"];
					App.NotificationTitle = title;
				}
				if (message.Data.ContainsKey("id"))
				{
					id = message.Data["id"];
					App.NotificationID = id;
				}
				if (message.Data.ContainsKey("message"))
				{
					notificationMessage = message.Data["message"];
				}
                App.GetUnreadNotification();
				if (App.NotificationTitle == "Pulse Message")
				{
					if (!string.IsNullOrEmpty(App.NotificationID))
					{
					if (App.IsChatOpened && App.CurrentChatWindow == Convert.ToInt32(App.NotificationID))
					        
						{
							App.GetMessageNotification();
						}
					}
				}
				else
				{
					SendNotification(notificationMessage, title, id);
				}

			} 		} 		[Obsolete] 		void SendNotification(string messageBody, string Title, string data) 		{
			var intent = new Intent(this, typeof(MainActivity));
			intent.PutExtra("data", data);
			var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.UpdateCurrent); 			var notificationBuilder = new Notification.Builder(this)
				 .SetSmallIcon(Resource.Drawable.icon)
				.SetContentTitle(Title)
				.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
				.SetContentText(messageBody) 				.SetAutoCancel(true)
				.SetContentIntent(pendingIntent);
			var notificationManager = NotificationManager.FromContext(this);
            if (Android.OS.Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "NOTIFICATION_CHANNEL_NAME", NotificationImportance.High);
                notificationChannel.EnableLights(true);
                notificationChannel.EnableVibration(true);
                notificationChannel.LightColor = Color.Red;
                notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });
                notificationBuilder.SetChannelId(NOTIFICATION_CHANNEL_ID);
                notificationManager.CreateNotificationChannel(notificationChannel);
            }
			notificationManager.Notify(DateTime.Now.Millisecond, notificationBuilder.Build());

		} 
	}
}

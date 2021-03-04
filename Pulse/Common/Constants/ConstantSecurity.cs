using System.Net;
using Amazon;

namespace Pulse
{
	public static partial class Constant
	{

		/// <summary>
		/// Facebook
		/// </summary>

		//public const string FacebookAppID = "110876032924543";
		public const string FacebookAppID = "826805037527758";


		public const string FacebookAuthorizeUrl = "https://www.facebook.com/v2.9/dialog/oauth";
		public const string FacebookRedirectUrliOS = "fb" + FacebookAppID + "://authorize";
        //public const string FacebookAuthorizeUrl = "https://m.facebook.com/dialog/oauth/";
        public const string FacebookRedirectUrl = "https://www.facebook.com/connect/login_success.html";
		public const string FacebookScope = "";
		public const string FacebookAccessTokenParameter = "access_token";
		public const string FacebookProfileUrl = "https://graph.facebook.com/me?fields=email,name,gender,picture";

		public const string Authorization = "Authorization";
		public const string ContentTypeText = "Content-Type";
		public const string ContentType = "application/json";


		#region Google Places	
		public const string GoogleSearchType = "(cities)";
		public const string GoogleSearchLanguage = "en_US";
		public const string GooglePlacesURL = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input={0}&types={1}&language={2}&key={3}";
		public const string GoogleSetDestinationURL = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input={0}&language={1}&key={2}";
		public const string GooglePlaceDetailURL = "https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&key={1}";
		public const string GoogleNearbyPlaceDetailURL = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={0},{1}&name={2}&radius=500000&key={3}";
		public const string GoogleGeocodeRequestURL = "https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key={2}";
		public const string GoogleApiKeyDroid = "AIzaSyDkj-3l6s74EWJL7zbfFK3_--MD5o8Kh9E";
		public const string GoogleApiKeyIOS = "AIzaSyBI0uGBZEf7xnDWWMgMUMBNqldWPFBuajI";
		public const string GOOGLE_MAPS_ANDROID_API_KEY = "AIzaSyCpFScameK9TW71EMWWRbf_Zt-eNmPLUWE";
		public const string GOOGLE_MAPS_IOS_API_KEY = "AIzaSyDX8awoGUpR2EQeaPSNMr1pYX_mo7EbndI";
		#endregion

		#region AWS S3 bucket
		//public const string AWSAccessKey = "AKIAIBWAPLCZWQUPDMRQ";
		//public const string AWSSecretKey = "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";
		//public const string BUCKET_NAME = "pli-socialma";
		//public const string AWS_USER_Folder = "Users_Images/";
		//public const string AWS_EVENT_Folder = "Event_Images/";
		//public const string AWS_GROUPS_Folder = "Group_Images/";
		//public const string AWS_File_Ext = ".png";
		//public const string AWS_Video_Ext = ".mp4";
		//public const string CDNURI = "https://s3.ca-central-1.amazonaws.com/pli-socialma/";
		//public const long MaxResponseContentBufferSize = 256000;
		//public static readonly RegionEndpoint BucketREGION = RegionEndpoint.CACentral1;

		//public const HttpStatusCode NO_SUCH_BUCKET_STATUS_CODE = HttpStatusCode.NotFound;
		//public const HttpStatusCode BUCKET_ACCESS_FORBIDDEN_STATUS_CODE = HttpStatusCode.Forbidden;
		//public const HttpStatusCode BUCKET_REDIRECT_STATUS_CODE = HttpStatusCode.Redirect;

        public const string AWSAccessKey = "AKIAIBWAPLCZWQUPDMRQ";
        public const string AWSSecretKey = "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";
        public const string BUCKET_NAME = "pli-socialma-dev";
        public const string AWS_USER_Folder = "Users_Images/";
        public const string AWS_EVENT_Folder = "Event_Images/";
        public const string AWS_GROUPS_Folder = "Group_Images/";
        public const string AWS_File_Ext = ".png";
        public const string AWS_Video_Ext = ".mp4";
        public const string CDNURI = "https://s3.us-west-2.amazonaws.com/pli-socialma-dev/";
        public const long MaxResponseContentBufferSize = 256000;
        public static readonly RegionEndpoint BucketREGION = RegionEndpoint.USWest2;

        public const HttpStatusCode NO_SUCH_BUCKET_STATUS_CODE = HttpStatusCode.NotFound;
        public const HttpStatusCode BUCKET_ACCESS_FORBIDDEN_STATUS_CODE = HttpStatusCode.Forbidden;
        public const HttpStatusCode BUCKET_REDIRECT_STATUS_CODE = HttpStatusCode.Redirect;
     	#endregion

		#region paypal
		public const string PayPalSandboxBaseServiceUrl = "https://api.sandbox.paypal.com/v1/";
		public const string PayPalBaseServiceUrl = "https://api.paypal.com/v1/";
        public const string PayPalTokenServiceUrl = PayPalSandboxBaseServiceUrl + "oauth2/token/";
        public const string PayPalPaymentServiceUrl = PayPalSandboxBaseServiceUrl + "payments/payment/";
        public const string PayPalCreditCardsUrl = PayPalSandboxBaseServiceUrl + "vault/credit-cards/";
        public const string PayPalGetCreditCardsUrl = PayPalSandboxBaseServiceUrl + "vault/credit-cards?external_customer_id=";
		public const string PaypalClientId = "AXwUk19RF6YDkO6k7tFbPvrN1xtjVw8PvdQ0OGsrVn37O-AESarY7tz9U4RomoQ3oNZhVo4Fb4CvHBtg";
		public const string PayPalClientSecret = "ECHVMu0cHZ1y55v1FqF2t7ffKSe1h9yFQXcoMZqoYQ46ioMdcdNgHKhABXXXt2qDeG3p2xrBMh85Txm6";
		public const string SANDBOXPaypalClientId = "AeesYO7oqVoShp-GUUwSmBoClLKHeuUs96i88uJcy8cqW0vJKa0cXhceEYUEQadPadFQKvrYUSy9Au6y";
		public const string SANDBOXPayPalClientSecret = "EDx3LVql3-dVZp6rKn8o66e_LJYTSE-T8h_z-VY8ZLIV9HBCp3EyqvBOm39ix0K8YzF-Xnu760dBhPwc";
		public const string PayPalGrantType = "client_credentials";
		#endregion

	}
}

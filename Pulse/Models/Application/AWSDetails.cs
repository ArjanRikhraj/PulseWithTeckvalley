namespace Pulse
{
	public class AwsImagesPath
	{
		public string user_profile { get; set; }
		public string event_images { get; set; }
		public string event_videos { get; set; }
		public string event_videos_thumbnails { get; set; }
		public string event_transcoded_videos { get; set; }
        public string story_images { get; set; }
        public string story_videos { get; set; }
    }

	public class AwsDetailResponse
	{
		public string aws_access_key { get; set; }
		public string s3_region { get; set; }
		public string s3_bucket { get; set; }
		public string s3_host { get; set; }
		public string aws_secret_key { get; set; }
		public string andriod_app_version { get; set; }
		public string ios_app_version { get; set; }
		public AwsImagesPath images_path { get; set; }
		public int records_per_page { get; set; }
        public EnvKeys Env_keys { get; set; }
	}

	public class AwsDetail
	{
		public AwsDetailResponse response { get; set; }
		public int status { get; set; }
	}
    public class EnvKeys
    {
        public string Google_near_by_place_detail_url { get; set; }
        public string SANDBOXPayPalClientSecret { get; set; }
        public string Google_place_detail_url { get; set; }
        public string Google_search_type { get; set; }
        public bool Transcoding_enable { get; set; }
        public string GoogleApiKeyIOS { get; set; }
        public int BoostedEventPrice { get; set; }
        public string Aws_cdn_url { get; set; }
        public string Google_places_url { get; set; }
        public string Pay_pal_sabdbox_service_url { get; set; }
        public string Card_expiry_year_start { get; set; }
        public string MaxResponseContentBufferSize { get; set; }
        public string SANDBOXPaypalClientId { get; set; }
        public string Facebook_profile_url { get; set; }
        public string Card_expiry_year_range { get; set; }
        public string PayPalClientSecret { get; set; }
        public string FacebookAppID { get; set; }
        public string PaypalClientId { get; set; }
        public string GoogleApiKeyDroid { get; set; }
        public string Google_set_destination_url { get; set; }
        public string Facebook_authorize_url { get; set; }
        public string GOOGLE_MAPS_IOS_API_KEY { get; set; }
        public string Andriod_google_navigation { get; set; }
        public string GOOGLE_MAPS_ANDROID_API_KEY { get; set; }
        public string Google_search_lang { get; set; }
        public string Pay_pal_base_service_url { get; set; }
        public string Ios_google_navigation { get; set; }
    }

}

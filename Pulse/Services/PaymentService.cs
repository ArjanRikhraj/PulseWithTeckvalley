using System;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using ModernHttpClient;
using Xamarin.Forms;

namespace Pulse
{
	public class PaymentService
	{
		readonly HttpClient client;

		public PaymentService()
		{
			if (Device.RuntimePlatform == Device.iOS)
			{
				client = new HttpClient();
			}
			else if (Device.RuntimePlatform == Device.Android)
			{
				client = new HttpClient(new NativeMessageHandler());
			}
			client.MaxResponseContentBufferSize = Constant.MaxResponseContentBufferSize;
		}

		public async Task<PaypalToken> GetPaypalToken()
		{
			PaypalToken token = new PaypalToken();

            var uri = new Uri(string.Format("{0}", Constant.PayPalTokenServiceUrl));
			try
			{
                var authData = string.Format("{0}:{1}", Constant.SANDBOXPaypalClientId, Constant.SANDBOXPayPalClientSecret);
				//if (App.IsTestingEnvironment)
				//{
				//var authData = string.Format("{0}:{1}", Constant.SANDBOXPaypalClientId, Constant.SANDBOXPayPalClientSecret);
				//}
				var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeaderValue);

				Dictionary<string, string> pairs = new Dictionary<string, string>();
				pairs.Add("grant_type", Constant.PayPalGrantType);
				var formContent = new FormUrlEncodedContent(pairs);

				var response = await client.PostAsync(uri, formContent);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					token = JsonConvert.DeserializeObject<PaypalToken>(content);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}
			return token;
		}

		public async Task<PaymentResponse> MakePayment(PaymentDetail paymentDetail)
		{
            var uri = new Uri(string.Format("{0}", Constant.PayPalPaymentServiceUrl));
			PaymentResponse obj = new PaymentResponse();
			try
			{
				PaypalToken newToken = await GetPaypalToken();
				if (newToken.access_token == null)
					return null;

				var json = JsonConvert.SerializeObject(paymentDetail);
				var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");

				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newToken.access_token);

				var response = await client.PostAsync(uri, httpcontent);

				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					obj = JsonConvert.DeserializeObject<PaymentResponse>(content);
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"ERROR {0}", ex.Message);
			}

			return obj;
		}


	}
}


using System;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using ModernHttpClient;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net;

namespace Pulse
{
    public class MainServices
    {
        readonly HttpClient client;

        #region Public Methods
        public MainServices()
        {
            HttpClientHandler handler = new HttpClientHandler();

            //not sure about this one, but I think it should work to allow all certificates:
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chaun, ssPolicyError) =>
            {
                return true;
            };
            client = new HttpClient(handler);
            //client = Device.RuntimePlatform == Device.iOS ? new HttpClient() : new HttpClient(new NativeMessageHandler());
            client.MaxResponseContentBufferSize = Constant.MaxResponseContentBufferSize;
        }
        public async Task<Token> GetToken()
        {
            Token token = new Token();
            var uri = new Uri(string.Format("{0}", Constant.TokenServiceUrl));
            try
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>();
                pairs.Add(Constant.ParaKeyUsername, Constant.UserNameForToken);
                pairs.Add(Constant.ParaKeyPassword, Constant.PasswordForToken_localNtesting);
                pairs.Add(Constant.ParaKeyGrantType, Constant.PublicGrant_type);
                pairs.Add(Constant.ParaKeyClientId, Constant.PublicClientId_staging);
                pairs.Add(Constant.ParaKeyClientSecret, Constant.PublicClientSecret_staging);
                var formContent = new FormUrlEncodedContent(pairs);
                //HttpResponseMessage response = null;
               var response = await client.PostAsync(uri, formContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<Token>(content);
                    SessionManager.AccessToken = token.access_token;
                }
                else if (Convert.ToInt32(response.StatusCode) == Constant.Status401)
                {
                    SessionManager.AccessToken = "";
                    var content = await response.Content.ReadAsStringAsync();
                    var tokenError = JsonConvert.DeserializeObject<TokenError>(content);
                }
                else
                {
                    SessionManager.AccessToken = "";
                    await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
                //token.access_token = "9ZRUIrvipnpQIO2eWWnGgtIuiTSAQT";
                return token;
            }
            return token;
        }   
        public async Task<T> Post<T>(string methodName, object postData)
        {
            var uri = new Uri(string.Format("{0}{1}", Constant.BaseServiceUrl, methodName));
            var json = JsonConvert.SerializeObject(postData);
            var httpcontent = new StringContent(json, Encoding.UTF8, Constant.ContentType);
         //   client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Constant.TokenTypeBearer, "9ZRUIrvipnpQIO2eWWnGgtIuiTSAQT");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Constant.TokenTypeBearer, SessionManager.AccessToken);
            var response = await client.PostAsync(uri, httpcontent);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
           var r=  JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            return r;
        }
        public async Task<T> Put<T>(string methodName, object postData)
        {
            var uri = new Uri(string.Format("{0}{1}", Constant.BaseServiceUrl, methodName));
            var json = JsonConvert.SerializeObject(postData);
            var httpcontent = new StringContent(json, Encoding.UTF8, Constant.ContentType);
            
                // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Constant.TokenTypeBearer, "9ZRUIrvipnpQIO2eWWnGgtIuiTSAQT");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Constant.TokenTypeBearer, SessionManager.AccessToken);
            var response = await client.PutAsync(uri, httpcontent);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
        public async Task<T> Get<T>(string methodName)
        {
            try
            {
                var uri = new Uri(string.Format("{0}{1}", Constant.BaseServiceUrl, methodName));
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Constant.TokenTypeBearer, "9ZRUIrvipnpQIO2eWWnGgtIuiTSAQT");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Constant.TokenTypeBearer, SessionManager.AccessToken);
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                }
                return JsonConvert.DeserializeObject<T>(string.Empty);
            }
            catch (Exception e)
            {
                return JsonConvert.DeserializeObject<T>(string.Empty);
            }
        }
        #endregion
    }
}


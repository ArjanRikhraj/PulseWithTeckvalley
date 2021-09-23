using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pulse.ViewModels
{
   public class EventMediaViewModel:BaseViewModel
    {
        public int pageNoMedia=1;
        public int totalMediaPages=1;
        public int totalLiveMediaPages;
        public List<EventMedia> MediaList;
        MainServices mainService;
        public EventMediaViewModel()
        {
            mainService = new MainServices();
        }
        public async Task<bool> GetMediaList(bool isLive)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                    TapCount = 0;
                    return false;
                }
                else
                {
                    if (SessionManager.AccessToken != null && (pageNoMedia == 1 || pageNoMedia <= totalLiveMediaPages))
                    {
                        int TappedEventId = 0;
                        MediaList = new List<EventMedia>();
                        string url = isLive ? Constant.EventLiveMediaListUrl : Constant.EventMediaListUrl;
                        var response = await mainService.Get<ResultWrapper<EventMedia>>(url + TappedEventId + "/?page=" + pageNoMedia);
                        if (response != null && response.status == Constant.Status200 && response.response != null && response.response.Count > 0)
                        {
                            foreach (var item in response.response)
                            {
                                MediaList.Add(item);
                            }
                            totalLiveMediaPages = GetPageCount(response.response[response.response.Count - 1].total_media);
                            return true;

                        }
                        else if (response != null && response.status == Constant.Status401)
                        {
                            SignOut();
                            IsLoading = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

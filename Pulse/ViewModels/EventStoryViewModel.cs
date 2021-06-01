using Pulse.Models.Application.Events;
using Pulse.Pages.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pulse.ViewModels
{
   public class EventStoryViewModel : BaseViewModel
    {
        MainServices mainService;
        ObservableCollection<Story> eventStories { get; set; }
        public ObservableCollection<Story> EventStories
        {
            get
            {
                return eventStories;
            }
            set
            {
                eventStories = value;
                OnPropertyChanged("EventStories");
            }
        }
        int pos;
        public int Pos
        {
            get
            {
                return this.pos;
            }

            set
            {
                this.pos = value;
                OnPropertyChanged("Pos");
            }
        }
        bool isNoStoryVisible;
        public bool IsNoStoryVisible
        {
            get
            {
                return this.isNoStoryVisible;
            }

            set
            {
                this.isNoStoryVisible = value;
                OnPropertyChanged("IsNoStoryVisible");
            }
        }
        private ICommand storyMenuCommand { get; set; }
        public ICommand StoryMenuCommand
        {
            get
            {
                return storyMenuCommand ?? (storyMenuCommand = new Command<object>((currentObject) => StoryOptions(currentObject)));
            }
        }
        private ICommand cancelCommand { get; set; }
        public ICommand CancelCommand
        {
            get
            {
                return cancelCommand ?? (cancelCommand = new Command<object>((currentObject) => CancelStory(currentObject)));
            }
        }

        

        private ICommand saveStoryCommand { get; set; }
        public ICommand SaveStoryCommand
        {
            get
            {
                return saveStoryCommand ?? (saveStoryCommand = new Command<object>((currentObject) => SaveStory(currentObject)));
            }
        }
        private CancellationTokenSource cancellation;
        public EventStoryViewModel(int eventId)
        {
            mainService = new MainServices();
            GetAllStories(eventId);
            this.cancellation = new CancellationTokenSource();
        }
        private async void SaveStory(object sender)
        {
            try
            {
                var currentObject = (Story)sender;
                if (currentObject != null)
                {
                    SaveStoryRequest request = new SaveStoryRequest();
                    request.event_id = currentObject.event_id;
                    request.id = currentObject.id;
                    request.user_id = SessionManager.UserId;
                    var response = await mainService.Post<ResultWrapperSingle<Stories>>(Constant.SaveEventStories, request);
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        ShowToast(Constant.AlertTitle, "Story Successfully Saved");
                        currentObject.IsMenuOptionVisible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        private async void CancelStory(object sender)
        {
            try
            {
                var currentObject = (Story)sender;
                if (currentObject != null)
                {
                    currentObject.IsMenuOptionVisible = false;
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        private async void StoryOptions(object sender)
        {
            try
            {
                var currentObject = (Story)sender;
                if(currentObject!=null)
                {
                    currentObject.IsMenuOptionVisible = true;
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
         private async void GetAllStories(int eventId)
        {
            try
            {
                EventStoryRequest request = new EventStoryRequest();
                request.event_id = eventId;
                var response = await mainService.Post<ResultWrapperSingle<Stories>>(Constant.GetEventStories, request);
                if (response != null && response.status == Constant.Status200 && response.response != null)
                {
                    if (response.response.story.Count() > 0)
                    {
                        IsNoStoryVisible = false;
                        eventStories = new ObservableCollection<Story>();
                        var allStories = response.response.story;
                        var storyCount = response.response.StoryCount;
                        foreach (var item in allStories)
                        {
                            Story story = new Story();
                            story.id = item.id;
                            story.event_id = item.event_id;
                            story.file_url = item.file_url;
                            story.profile_url = item.profile_url;
                            story.user_id = item.user_id;
                            story.create_date = item.create_date;
                            story.create_time = item.create_time;
                            story.VideoHeight = Application.Current.MainPage.Height;
                            story.BtnBack = "back.png";
                            story.ProgressTime = 4000;
                            if (item.file_url.Contains("mp4"))
                            {
                                story.file_url = "https://s3.us-west-2.amazonaws.com/pli-socialma-prod/Users_Images/169a9ec.png";
                                story.videofile_url = VideoSource.FromUri(item.file_url);
                                story.IsVideoVisible = true;
                                story.IsImageVisible = true;
                            }
                            else
                            {
                                story.IsVideoVisible = false;
                                story.IsImageVisible = true;
                            }
                            eventStories.Add(story);
                        }
                        EventStories = eventStories;
                        if (EventStories.Count > 1)
                        {
                            int count = 0;
                            CancellationTokenSource cts = this.cancellation; // safe copy
                            Device.StartTimer(TimeSpan.FromSeconds(4), (Func<bool>)(() =>
                            {
                                if (count == storyCount - 1)
                                {
                                    return false;
                                }
                                Pos = (Pos + 1) % EventStories.Count;
                                count++;
                                return true;
                            }));
                        }
                    }
                    else
                        IsNoStoryVisible = true;
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
    }
}

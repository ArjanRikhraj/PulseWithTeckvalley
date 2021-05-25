using Pulse.Models.Application.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        private ICommand saveStoryCommand { get; set; }
        public ICommand SaveStoryCommand
        {
            get
            {
                return saveStoryCommand ?? (saveStoryCommand = new Command<object>((currentObject) => SaveStory(currentObject)));
            }
        }
        public EventStoryViewModel(int eventId)
        {
            mainService = new MainServices();
            GetAllStories(eventId);
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
                            story.ProgressTime = 3000;
                            if (item.file_url.Contains("mp4"))
                            {
                                story.videofile_url = VideoSource.FromUri(item.file_url);
                                story.IsVideoVisible = true;
                                story.IsImageVisible = false;
                            }
                            else
                            {
                                story.IsVideoVisible = false;
                                story.IsImageVisible = true;
                            }
                            eventStories.Add(story);
                        }
                        EventStories = eventStories;
                        //if (EventStories.Count > 1)
                        //{
                        //    int count=0;
                        //    Device.StartTimer(TimeSpan.FromSeconds(5), (Func<bool>)(() =>
                        //    {
                        //        if(count== storyCount-1)
                        //        {
                        //            Navigation.PopModalAsync();
                        //            return false;
                        //        }
                        //        Pos = (Pos + 1) % EventStories.Count;
                        //        count++;
                        //        return true;
                        //    }));
                        //}
                        //if (EventStories.Count == 1)
                        //{
                        //    Device.StartTimer(TimeSpan.FromSeconds(5), (Func<bool>)(() =>
                        //    {
                        //        Navigation.PopModalAsync();
                        //        return false;
                        //    }));
                        //}

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

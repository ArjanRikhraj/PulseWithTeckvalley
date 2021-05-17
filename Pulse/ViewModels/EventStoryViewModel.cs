using Pulse.Models.Application.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public EventStoryViewModel(int eventId)
        {
            mainService = new MainServices();
            GetAllStories(eventId);
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
                    eventStories = new ObservableCollection<Story>();
                    var allStories = response.response.story;
                    var storyCount = response.response.StoryCount;
                    foreach (var item in allStories)
                    {
                        Story story = new Story();
                        story.event_id = item.event_id;
                        story.file_url = item.file_url= "https://s3.us-west-2.amazonaws.com/pli-socialma-prod/Event_Images/6bab6284-5265-4970-a23a-cefb1e6823ff.jpg";
                        story.profile_url = item.profile_url;
                        story.user_id = item.user_id;
                        story.create_date = item.create_date;
                        story.create_time = item.create_time;
                        story.ProgressTime = 3000;
                        if(item.file_url.Contains("mp4"))
                        {
                            story.IsVideoVisible = true;
                            story.IsImageVisible = false;
                            Device.StartTimer(TimeSpan.FromSeconds(3), (Func<bool>)(() =>
                            {
                                Pos = (Pos + 1) % EventStories.Count;
                                if(Pos== storyCount)
                                {
                                    App.Navigation.PopAsync();
                                    return false;
                                }
                                return true;
                            }));
                        }
                        else
                        {
                            story.IsVideoVisible = false;
                            story.IsImageVisible = true;
                            Device.StartTimer(TimeSpan.FromSeconds(5), (Func<bool>)(() =>
                            {
                                Pos = (Pos + 1) % EventStories.Count;
                                if (Pos == storyCount-1)
                                {
                                   // Navigation.PopModalAsync();
                                    return false;
                                }
                                return true;
                            }));
                        }
                        eventStories.Add(story);
                    }
                    EventStories = eventStories;
                    if (EventStories.Count > 1)
                    {
                       
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
    }
}

using MediaManager.Forms;
using MediaManager.Playback;
using Pulse.Models.Application.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pulse.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StoriesPage : ContentPage
    {
        MainServices mainService;
        readonly EventViewModel eventViewModel;
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
        public StoriesPage()
        {
            mainService = new MainServices();
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            GetAllStories();
        }
        private async void GetAllStories()
        {
            try
            {
                EventStoryRequest request = new EventStoryRequest();
                request.event_id = eventViewModel.TappedEventId;
                var response = await mainService.Post<ResultWrapperSingle<Stories>>(Constant.GetEventStories, request);
                if (response != null && response.status == Constant.Status200 && response.response != null)
                {
                    if (response.response.story.Count() > 0)
                    {
                       // IsNoStoryVisible = false;
                        eventStories = new ObservableCollection<Story>();
                        var allStories = response.response.story;
                        var storyCount = response.response.StoryCount;
                        StackLayout stack = new StackLayout()
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Orientation = StackOrientation.Horizontal
                        };
                        foreach (var item in allStories)
                        {
                            if (!string.IsNullOrEmpty(item.file_url))
                            {

                                if (item.file_url.Contains("mp4"))
                                {
                                    VideoView videoView = new VideoView()
                                    {
                                        Source = item.file_url,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        VerticalOptions = LayoutOptions.FillAndExpand,
                                        HeightRequest = Application.Current.MainPage.Height,
                                        Repeat = RepeatMode.One,
                                        ShowControls = true
                                    };
                                    stack.Children.Add(videoView);
                                }
                                else
                                {
                                    Image storyImage = new Image()
                                    {
                                        Source = item.file_url,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        VerticalOptions = LayoutOptions.FillAndExpand,
                                        HeightRequest = Application.Current.MainPage.Height,
                                    };
                                    stack.Children.Add(storyImage);
                                }
                             
                            }
                        }
                        scrollMain.Content = stack;
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
using Pulse.Models.Application.Events;
using Pulse.Pages.Event;
using Pulse.Pages.User;
using Rg.Plugins.Popup.Services;
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
        readonly EventViewModel eventViewModel;
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
        bool isReportStoryVisible;
        public bool IsReportStoryVisible
        {
            get
            {
                return this.isReportStoryVisible;
            }

            set
            {
                this.isReportStoryVisible = value;
                OnPropertyChanged("IsReportStoryVisible");
            }
        }
        bool isDeleteStoryVisible;
        public bool IsDeleteStoryVisible
        {
            get
            {
                return this.isDeleteStoryVisible;
            }

            set
            {
                this.isDeleteStoryVisible = value;
                OnPropertyChanged("IsDeleteStoryVisible");
            }
        }
        bool isReportPopupVisible;
        public bool IsReportPopupVisible
        {
            get
            {
                return this.isReportPopupVisible;
            }

            set
            {
                this.isReportPopupVisible = value;
                OnPropertyChanged("IsReportPopupVisible");
            }
        }
        bool isAddStoryVisible;
        public bool IsAddStoryVisible
        {
            get
            {
                return this.isAddStoryVisible;
            }

            set
            {
                this.isAddStoryVisible = value;
                OnPropertyChanged("IsAddStoryVisible");
            }
        }
        
        bool isOverlayPopupVisible;
        public bool IsOverlayPopupVisible
        {
            get
            {
                return this.isOverlayPopupVisible;
            }

            set
            {
                this.isOverlayPopupVisible = value;
                OnPropertyChanged("IsOverlayPopupVisible");
            }
        }
        private string descriptionComment;
        public string DescriptionComment
        {
            get
            {
                return descriptionComment;
            }
            set
            {
                descriptionComment = value;
                OnPropertyChanged("DescriptionComment");
            }
        }
        private List<string> reportCommentList;
        public List<string> ReportCommentList
        {
            get
            {
                return reportCommentList;
            }
            set
            {
                reportCommentList = value;
                OnPropertyChanged("ReportCommentList");
            }
        }
        private string selectedReason;
        public string SelectedReason
        {
            get
            {
                return selectedReason;
            }
            set
            {
                selectedReason = value;
                OnPropertyChanged("SelectedReason");
                ReportStory(selectedReason);
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
        private ICommand reportStoryCommand { get; set; }
        public ICommand ReportStoryCommand
        {
            get
            {
                return reportStoryCommand ?? (reportStoryCommand = new Command<object>((currentObject) => OnReportStoryCommand(currentObject)));
            }
        }
        private ICommand deleteStoryCommand { get; set; }
        public ICommand DeleteStoryCommand
        {
            get
            {
                return deleteStoryCommand ?? (deleteStoryCommand = new Command<object>((currentObject) => OnDeleteStoryCommand(currentObject)));
            }
        }

       

        public Command CloseReportPopupCommand { private set; get; }
        private Story StoryDetails;
        private int EventId;
        private CancellationTokenSource cancellation;
        public EventStoryViewModel(int eventId)
        {
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            this.EventId = eventId;
            CloseReportPopupCommand = new Command(CloseReportPopup);
            mainService = new MainServices();
            GetAllStories();
            GetAllReportComments();
            this.cancellation = new CancellationTokenSource();
        }

        private void CloseReportPopup(object obj)
        {
            IsReportPopupVisible = false;
            IsOverlayPopupVisible = false;
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
                        currentObject.IsMenuOptionVisible = false;
                    }
                }
                IsAddStoryVisible = true;
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        private async void OnReportStoryCommand(object sender)
        {
            try
            {
                var currentObject = (Story)sender;
                if (currentObject != null)
                {
                    if(!IsReportPopupVisible)
                    {
                        IsReportPopupVisible = true;
                        IsOverlayPopupVisible = true;
                        currentObject.IsMenuOptionVisible = false;
                        StoryDetails = currentObject;
                    }
                    IsAddStoryVisible = true;
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        private async void OnDeleteStoryCommand(object sender)
        {
            try
            {
                var currentObject = (Story)sender;
                if (currentObject != null)
                {
                    ReportStoryRequest request = new ReportStoryRequest();
                    request.story_id = currentObject.id;
                    var response = await mainService.Put<ResultWrapperSingle<Stories>>(Constant.DeleteEventStories, request);
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        ShowToast(Constant.AlertTitle, "Successfully Deleted");
                        currentObject.IsMenuOptionVisible = false;
                        IsOverlayPopupVisible = false;
                        GetAllStories();
                        if (EventStories.Count == 0 || EventStories == null)
                            await Navigation.PopModalAsync();
                    }
                    IsAddStoryVisible = true;
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
        private async void GetAllReportComments()
        {
            try
            {
                reportCommentList = new List<string>();
                reportCommentList.Add("Bullying/Harassment");
                reportCommentList.Add("False information");
                reportCommentList.Add("Violence or dangerous organizations");
                reportCommentList.Add("Scam or fraud");
                reportCommentList.Add("Intellectual property violation");
                reportCommentList.Add("Sale of illegal or regulated goods");
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }

        private async void ReportStory(string reason)
        {
            try
            {
                if (StoryDetails != null && !string.IsNullOrEmpty(reason))
                {
                    if (string.IsNullOrEmpty(DescriptionComment))
                    {
                        await App.Instance.Alert(Constant.ReportDescriptionMessage, Constant.AlertTitle, Constant.Ok);
                        return;
                    }
                    ReportStoryRequest request = new ReportStoryRequest();
                    request.story_id = StoryDetails.id;
                    request.reason = reason;
                    request.description = DescriptionComment;
                    var response = await mainService.Post<ResultWrapperSingle<Stories>>(Constant.ReportEventStories, request);
                    if (response != null && response.status == Constant.Status200 && response.response != null)
                    {
                        await PopupNavigation.PushAsync(new ReportConfirmationPage("Story"));
                        IsReportPopupVisible = false;
                        IsOverlayPopupVisible = false;
                    }
                }
                reason = null;
            }
            catch (Exception ex)
            {
                reason = null;
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
                    IsAddStoryVisible = true;
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
                if (currentObject != null)
                {
                    IsAddStoryVisible = false;
                    currentObject.IsMenuOptionVisible = true;
                    IsDeleteStoryVisible = (eventViewModel.IsOwner || eventViewModel.IsAdmin || currentObject.user_id == SessionManager.UserId) ? true : false;
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
         private async void GetAllStories()
        {
            try
            {
                IsAddStoryVisible = eventViewModel.IsUserCheckedIn || eventViewModel.IsOwner ? true : false;
                EventStoryRequest request = new EventStoryRequest();
                request.event_id = EventId;
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
                    {
                        IsNoStoryVisible = true;
                        EventStories = null;
                    }
                    IsReportStoryVisible = eventViewModel.IsGoing||eventViewModel.IsUserCheckedIn ? true : false;
                   
                }
            }
            catch (Exception ex)
            {
                await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
            }
        }
    }
}

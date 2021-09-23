using Plugin.Connectivity;
using Pulse.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pulse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventMediaPage : ContentPage
    {
        readonly EventViewModel eventViewModel;
		ObservableCollection<EventGallery> tempMediaList = new ObservableCollection<EventGallery>();
		public EventMediaPage()
        {
            InitializeComponent();
            eventViewModel = ServiceContainer.Resolve<EventViewModel>();
            BindingContext = eventViewModel;
            SetInitialValues();
        }
        void SetInitialValues()
        {
            //listViewMedia.LoadMoreCommand = new Command(GetMedia);
            eventViewModel.pageNoMedia = 1;
            eventViewModel.totalMediaPages = 1;
            //tempMediaList.Clear();
            eventViewModel.totalLiveMediaPages = 1;
			lblEvent.Text = eventViewModel.EventTitle;
			lblDescription.Text = eventViewModel.Description;
			GetMedia();
        }
		async void GetMedia()
		{
			try
			{
				eventViewModel.IsLoading = true;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				}
				else
				{
					bool isList = await eventViewModel.GetMediaList(false);
					SetMediaList(isList, eventViewModel.MediaList);
				}
				eventViewModel.IsLoading = false;
			}

			catch (Exception)
			{
				eventViewModel.IsLoading = false;
				await App.Instance.Alert(Constant.ServerNotRunningMessage, Constant.AlertTitle, Constant.Ok);
			}
		}
		void SetMediaList(bool isList, List<EventMedia> list)
		{
			if (isList && eventViewModel.pageNoMedia < 2)
			{
				SetMedia(list);
			}
			else if (isList)
			{
				SetMedia(list);
			}
			else if (!isList && eventViewModel.pageNoMedia < 2)
			{
				eventViewModel.IsLoading = false;
			}
			else
			{
				mediaList.ItemsSource = tempMediaList;
				eventViewModel.IsLoading = false;
			}
		}

		void SetMedia(List<EventMedia> list)
		{
			foreach (var item in list)
			{
				tempMediaList.Add(new EventGallery
				{
					MediaId = item.id,
					ImageWidth = App.ScreenWidth,
					ImageHeight = App.ScreenHeight / 1.2,
					TotalMedia= item.total_media,
					FileType= item.file_type,
					FileName = item.file_type == 1 ? PageHelper.GetEventTranscodedVideo(PageHelper.GetEventTranscodedVideo(item.file_name)) : PageHelper.GetEventImage(item.file_name),
					VideoFileName = item.file_type == 1 ? PageHelper.GetEventTranscodedVideo(item.file_name) : "",
					IsPlayIconVisible = item.file_type == 1 ? true : false,
					IsImage = item.file_type == 1 ? false : true,
					IsVisibleUserName = false,
					VideoThumbnailFileName = item.file_type == 1 ? PageHelper.GetEventVideoThumbnail(item.file_thumbnail) : string.Empty

				});
			}
			eventViewModel.MediaList.Clear();
			mediaList.ItemsSource = tempMediaList;
			eventViewModel.pageNoMedia++;
			eventViewModel.IsLoading = false;
		}

        private async void Cross_Clicked(object sender, EventArgs e)
        {
			await Navigation.PopModalAsync();
		}
    }
}
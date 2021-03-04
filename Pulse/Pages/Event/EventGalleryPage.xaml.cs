using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class EventGalleryPage : BaseContentPage
	{
		readonly EventViewModel eventViewModel;
		int _tapCount = 0;
		ObservableCollection<EventGallery> tempMediaList = new ObservableCollection<EventGallery>();
		public EventGalleryPage()
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = eventViewModel;
			SetInitialValues();
		}
		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(10, 10, 10, 10);
			}
			listViewMedia.LoadMoreCommand = new Command(GetMedia);
			eventViewModel.pageNoMedia = 1;
			eventViewModel.totalMediaPages = 1;
			tempMediaList.Clear();
			eventViewModel.totalLiveMediaPages = 1;
			GetMedia();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<App>(this, "getPhotoEventGalleryMedia", (obj) => {
                eventViewModel.pageNoMedia = 1;
                eventViewModel.totalMediaPages = 1;
                tempMediaList.Clear();
                listViewMedia.IsVisible = false;
                GetMedia();
            });

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App>(this, "getPhotoEventGalleryMedia");
        }

       async void Cross_Clicked(object sender, System.EventArgs e)
		{
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
					await Navigation.PopModalAsync();
					eventViewModel.IsLoading = false;
					_tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				_tapCount = 0;
			}
		}

		async void GetMedia()
		{

			try
			{
				eventViewModel.IsLoading = true;
				if (!CrossConnectivity.Current.IsConnected)
				{
					await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
					_tapCount = 0;
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
				_tapCount = 0;
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
				listViewMedia.IsVisible = false;
				eventViewModel.IsLoading = false;
			}
			else
			{
				listViewMedia.ItemsSource = tempMediaList;
				eventViewModel.IsLoading = false;
			}
		}

		void SetMedia(List<EventMedia> list)
		{
			listViewMedia.IsVisible = true;
			foreach (var item in list)
			{
                tempMediaList.Add(new EventGallery
                {
                    ImageWidth = App.ScreenWidth,
                    ImageHeight = App.ScreenHeight / 1.2,
                    FileName = item.file_type == 1 ? PageHelper.GetEventTranscodedVideo(PageHelper.GetEventTranscodedVideo(item.file_name)) : PageHelper.GetEventImage(item.file_name),
                    VideoFileName = item.file_type == 1 ? PageHelper.GetEventTranscodedVideo(item.file_name) : "",
                    IsPlayIconVisible = item.file_type == 1 ? true : false,
                    IsImage = item.file_type == 1 ? false : true,
                    IsVisibleUserName = false,
                    VideoThumbnailFileName = item.file_type == 1 ? PageHelper.GetEventVideoThumbnail(item.file_thumbnail):string.Empty

				});
			}
			eventViewModel.MediaList.Clear();
			listViewMedia.ItemsSource = tempMediaList;
			eventViewModel.pageNoMedia++;
			eventViewModel.IsLoading = false;
		}

		void lstMediaTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
            if (listViewMedia.SelectedItem != null)
            {
                listViewMedia.SelectedItem = null;
            }
			var selected = (EventGallery)e.Item;
			if (selected.IsPlayIconVisible)
			{

				if (!string.IsNullOrEmpty(selected.VideoFileName))
				{
					if (Device.RuntimePlatform == Device.iOS)
					{
						stckVideo.IsVisible = true;
						CustomWebView video = new CustomWebView
						{
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.FillAndExpand,
						};
						video.Source = PageHelper.GetEventTranscodedVideo(PageHelper.GetEventTranscodedVideo(selected.VideoFileName));
                        grdVideo.Padding = new Thickness(10, 25, 10, 25);
                        grdVideo.Children.Add(video, 0, 1);
					}
					else if (Device.RuntimePlatform == Device.Android)
					{
						DependencyService.Get<IVideoPlayer>().Play(PageHelper.GetEventTranscodedVideo(selected.VideoFileName));
					}
				}
			}
		}
		void CrossVideo_Clicked(object sender, System.EventArgs e)
		{
			stckVideo.IsVisible = false;
		}

       
	}
}

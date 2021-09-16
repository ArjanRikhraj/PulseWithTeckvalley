using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class PhotoAlbumPage : BaseContentPage
	{
		readonly EventViewModel eventViewModel;
		int _tapCount = 0;
		ObservableCollection<EventGallery> tempMediaList = new ObservableCollection<EventGallery>();
		public PhotoAlbumPage()
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = eventViewModel;
			SetInitialValues();
			// listViewMedia.ItemAppearing += ListViewMedia_ItemAppearing;
			//listViewMedia.ItemDisappearing += ListViewMedia_ItemDisappearing;
		}
		void SetInitialValues()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				topStack.Margin = new Thickness(10, 10, 10, 10);
			}
			eventViewModel.pageNoMedia = 1;
			eventViewModel.totalMediaPages = 1;
			tempMediaList.Clear();
			listViewMedia.IsVisible = false;
			lblNoMedia.IsVisible = false;
			listViewMedia.LoadMoreCommand = new Command(async () => await GetMedia());
			 GetMedia();
		}
    
        void ListViewMedia_ItemDisappearing(object sender, ItemVisibilityEventArgs e)
        {
            if (e != null && sender != null)
            {
                var selectedItem = e.Item as EventGallery;
                var item = (ViewCell)sender;
                if (item != null && item.View != null)
                {
                    var itemView = (Grid)item.View;
                    if (itemView != null)
                    {
                        var item1 = (StackLayout)itemView.Children;
                        if (item1 != null)
                        {
                            var item2 = (Grid)item1.Children[1];
                            if (item2 != null)
                            {
                                var videoplayer = (VideoPlayer)item2.Children;
                                if (videoplayer != null && selectedItem.IsPlayIconVisible)
                                {
                                    videoplayer.Stop();
                                }
                            }
                        }
                    }
                }
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
			  MessagingCenter.Subscribe<object>(this, "getPhotoAlbumMedia", (obj) => {
                eventViewModel.pageNoMedia = 1;
                eventViewModel.totalMediaPages = 1;
                lblNoMedia.IsVisible = false;
                tempMediaList.Clear();
                listViewMedia.IsVisible = false;
                GetMedia();
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App>(this, "getPhotoAlbumMedia");
        }


		async void Cross_Clicked(object sender, System.EventArgs e)
		{
			 await Navigation.PopModalAsync();
			if (CrossConnectivity.Current.IsConnected)
			{
				if (_tapCount < 1)
				{
					_tapCount = 1;
					eventViewModel.IsLoading = true;
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

		async Task GetMedia()
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
					bool isList = await eventViewModel.GetMyMediaList();
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
				lblNoMedia.IsVisible = true;
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
			mediaCollectionView.IsVisible = true;
			//listViewMedia.IsVisible = true;
			lblNoMedia.IsVisible = false;
			tempMediaList.Clear();
			foreach (var item in list)
			{
				tempMediaList.Add(new EventGallery
				{
					FileUrl=item.file_name,
					MediaId = item.id,
					IsPrivate=item.is_private,
					UserId= item.user_id,
					EventId= item.event_id,
					ImageWidth = App.ScreenWidth,
					ImageHeight = App.ScreenHeight / 1.2,
					FileName = item.file_type == 1 ? PageHelper.GetEventVideoThumbnail(item.file_thumbnail) : PageHelper.GetEventImage(item.file_name),
					IsPlayIconVisible = item.file_type == 1 ? true : false,
					PinIcon = item.is_private == true ? "iconPin.png" : "iconPinned.png",
					EventName = item.event_name,
					VideoFileName = item.file_type == 1 ? PageHelper.GetEventTranscodedVideo(item.file_name) : "",
					MediaDate = SetEventDate(item.create_date),
					IsVisibleUserName = true,
					UserImage = !string.IsNullOrEmpty(item.profile_image) ? item.profile_image : string.Empty,
					UserName = !string.IsNullOrEmpty(item.user_name) ? item.user_name : string.Empty,
					IsImage = item.file_type == 1 ? false : true,
					VideoThumbnailFileName = item.file_type == 1 ? PageHelper.GetEventVideoThumbnail(item.file_thumbnail) : string.Empty
				}) ;
            }
			eventViewModel.MediaList.Clear();
			mediaCollectionView.ItemsSource = tempMediaList;
			//listViewMedia.ItemsSource = tempMediaList;
			eventViewModel.pageNoMedia++;
			eventViewModel.IsLoading = false;
		}
		string SetEventDate(string createDate)
		{
			var dateStart = DateTime.Parse(createDate);
			return dateStart.Date.ToString("ddd,dd MMM").ToUpperInvariant() + ", " + dateStart.ToString("h:mm tt").Trim().ToUpperInvariant();
		}
		void lstMediaTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			var selected = (EventGallery)e.Item;

			if (selected.IsPlayIconVisible)
			{

				if (!string.IsNullOrEmpty(selected.VideoFileName))
				{
					if (Device.RuntimePlatform == Device.iOS)
					{
						stckVideo.IsVisible = true;
						WebView video = new WebView
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

        async  void mediaCollectionView_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
			var items = ((CollectionView)sender).SelectedItem as EventGallery;
			if (items == null)
				return;
			await eventViewModel.ShowMedia(items);
			((CollectionView)sender).SelectedItem = null;
		}
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
			listViewMedia.LoadMoreCommand = new Command(GetMedia);
			GetMedia();
		}
        //void ListViewMedia_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    IEnumerable<PropertyInfo> pInfos = (listViewMedia as ItemsView<Cell>).GetType().;
        //    var templatedItems = pInfos.FirstOrDefault(info => info.Name == "TemplatedItems");
        //    if (templatedItems != null)
        //    {
        //        var cells = templatedItems.GetValue(listViewMedia);
        //        Xamarin.Forms.ITemplatedItemsList<Xamarin.Forms.Cell> listCell = cells as Xamarin.Forms.ITemplatedItemsList<Xamarin.Forms.Cell>;
        //        ViewCell currentCell = listCell[position] as ViewCell;
        //        currentCell.View.BackgroundColor = Color.White;

        //        foreach (ViewCell cell in cells as Xamarin.Forms.ITemplatedItemsList<Xamarin.Forms.Cell>)
        //        {
        //            if (cell.BindingContext != null && currentCell != cell)
        //            {
        //                cell.View. = Color.Transparent;
        //            }
        //        }
        //    }
        //}

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
            MessagingCenter.Subscribe<App>(this, "getPhotoAlbumMedia", (obj) => {
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
			listViewMedia.IsVisible = true;
			lblNoMedia.IsVisible = false;
			foreach (var item in list)
			{
				tempMediaList.Add(new EventGallery
				{
					ImageWidth = App.ScreenWidth,
                    ImageHeight = App.ScreenHeight / 1.2,
					FileName = item.file_type == 1 ? PageHelper.GetEventVideoThumbnail(item.file_thumbnail) : PageHelper.GetEventImage(item.file_name),
					IsPlayIconVisible = item.file_type == 1 ? true : false,
					EventName = item.event_name,
                    VideoFileName = item.file_type == 1 ? PageHelper.GetEventTranscodedVideo(item.file_name) : "",
					MediaDate = SetEventDate(item.create_date),
                    IsVisibleUserName = true,
                    UserImage =!string.IsNullOrEmpty(item.profile_image)? item.profile_image :string.Empty,
                    UserName = !string.IsNullOrEmpty(item.user_name)? item.user_name :string.Empty,
                    IsImage = item.file_type == 1 ? false : true,
                    VideoThumbnailFileName = item.file_type == 1? PageHelper.GetEventVideoThumbnail(item.file_thumbnail):string.Empty
				});

            }
			eventViewModel.MediaList.Clear();
			listViewMedia.ItemsSource = tempMediaList;
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class CommentListingPage : BaseContentPage
	{
		#region private variables
		int _tapCount = 0;
		readonly EventViewModel eventViewModel;
		ObservableCollection<CommentListViewSource> tempCommentList = new ObservableCollection<CommentListViewSource>();
		ObservableCollection<UserInfo> Taggedfriends = new ObservableCollection<UserInfo>();
		#endregion

		#region Constructor
		public CommentListingPage()
		{
			InitializeComponent();
			eventViewModel = ServiceContainer.Resolve<EventViewModel>();
			BindingContext = eventViewModel;
			SetScreenDesign();
		}


		#endregion

		#region Private Methods
		void SetScreenDesign()
		{
			eventViewModel.pageNoComment = 1;
			tempCommentList.Clear();
			GetCommentsList();
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
					await eventViewModel.FetchEventDetail(Convert.ToString(eventViewModel.TappedEventId), false);
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

        async void Edit_Icon_Tapped(object sender, System.EventArgs e)
        {
            stackPopUpForComment.IsVisible = true;
            grdOverlayDialog.IsVisible = true;
            eventViewModel.IsLoading = false;
        }

        async void Delete_Tapped(object sender, System.EventArgs e)
        {
            stackPopUpForComment.IsVisible = false;
            grdOverlayDialog.IsVisible = false;
            var selected = (sender) as StackLayout;
            eventViewModel.TappedCommentId = selected.ClassId;
            bool result = await App.Instance.ConfirmAlert(Constant.DeleteCommentText, Constant.AlertTitle, Constant.Ok, Constant.CancelButtonText);
            if (result)
            {
                bool isDeleted = await eventViewModel.DeleteComment();
                if (isDeleted)
                {
                    eventViewModel.pageNoComment = 1;
                    tempCommentList.Clear();
                    if (stckComment.Children != null && stckComment.Children.Count > 0)
                        stckComment.Children.Clear();
                    GetCommentsList();
                    await App.Instance.Alert(Constant.CommentDeleted, Constant.AlertTitle, Constant.Ok);
                }
            }
        }
		async void Report_Tapped(object sender, System.EventArgs e)
		{
            stackPopUpForComment.IsVisible = false;
            grdOverlayDialog.IsVisible = false;
            var selected = (sender) as StackLayout;
			eventViewModel.TappedCommentId = selected.ClassId;
            await DisplayActionSheet();
			
		}
        void Comment_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                if (editorReportComment.Text.Length > 200)
                {
                    editorReportComment.Text = e.OldTextValue;
                }
                else
                {
                    editorReportComment.Text = e.NewTextValue;
                }
            }
        }
        async System.Threading.Tasks.Task DisplayActionSheet()
        {
            var actionSheet = await DisplayActionSheet(Constant.ReportEventTitle, Constant.CancelText, null, Constant.SpamText, Constant.NudityText, Constant.ViolenceText, Constant.HarassTxt, Constant.OtherText);
            switch (actionSheet)
            {
                case Constant.SpamText:
                    eventViewModel.EvenetCommentReportedData = Constant.SpamText;
                    await eventViewModel.ReportEventComment();

                    break;

                case Constant.NudityText:
                    eventViewModel.EvenetCommentReportedData = Constant.NudityText;
                    await eventViewModel.ReportEventComment();

                    break;
                case Constant.ViolenceText:
                    eventViewModel.EvenetCommentReportedData = Constant.ViolenceText;
                    await eventViewModel.ReportEventComment();

                    break;
                case Constant.HarassTxt:
                    eventViewModel.EvenetCommentReportedData = Constant.HarassTxt;
                    await eventViewModel.ReportEventComment();

                    break;
                case Constant.OtherText:
                    stackReport.IsVisible = true;
                    grdOverlayDialog.IsVisible = true;
                    break;

            }
        }
        void ReportEditor_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (editorReportComment.Text.Length >= 150)
            {
                editorReportComment.Text = editorReportComment.Text.Remove(editorReportComment.Text.Length - 1);  // Remove Last character

            }
        }
        async void ReportEventCommentSubmit(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount = 1;
                    eventViewModel.IsLoading = true;
                    eventViewModel.EvenetCommentReportedData = editorReportComment.Text;
                    if (string.IsNullOrEmpty(eventViewModel.EvenetCommentReportedData))
                    {
                        App.Instance.Alert("Please write a comment", Constant.AlertTitle, Constant.Ok);
                        eventViewModel.IsLoading = false;
                        _tapCount = 0;
                        return;
                    }
                    await eventViewModel.ReportEventComment();
                    stackReport.IsVisible = false;
                    grdOverlayDialog.IsVisible = false;
                    eventViewModel.IsLoading = false;
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
        async void CancelComment_Clicked(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (_tapCount < 1)
                {
                    _tapCount++;
                    stackReport.IsVisible = false;
                    grdOverlayDialog.IsVisible = false;
                    _tapCount = 0;
                }
            }
            else
            {
                await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
                _tapCount = 0;
            }
        }
		async void GetCommentsList()
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
					bool isList = await eventViewModel.GetCommentList();
					SetCommentList(isList, eventViewModel.CommentList);
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

		void SetCommentList(bool isList, List<CommentResponse> commentList)
		{
			if (isList && eventViewModel.pageNoComment < 2)
			{
				SetComments(commentList);
			}
			else if (isList)
			{
				SetComments(commentList);
			}
			else if (!isList && eventViewModel.pageNoComment < 2)
			{
				stckComment.IsVisible = false;
				lblNoComment.IsVisible = true;
				eventViewModel.IsLoading = false;
			}
			else
			{
				eventViewModel.IsLoading = false;
			}
		}

		void SetComments(List<CommentResponse> commentList)
		{
			stckComment.IsVisible = true;
			lblNoComment.IsVisible = false;
			foreach (var item in commentList)
			{
				CreateViewCellContent(item);
			}
			eventViewModel.CommentList.Clear();
			eventViewModel.IsLoading = false;
		}
		void CreateGuestGrid(Grid gridSelectedGuests)
		{
			gridSelectedGuests.Children.Clear();
			int column = 0;
			int row = 0;
			if (Taggedfriends != null && Taggedfriends.Count > 0)
			{
				foreach (var item in Taggedfriends)
				{
					gridSelectedGuests.RowSpacing = 2;
					gridSelectedGuests.ColumnSpacing = 2;
					Frame myView = CreateGuestList(item);
					if (column < 1)
					{
						gridSelectedGuests.Children.Add(myView, column, row);
						column++;
					}
					else
					{
						gridSelectedGuests.Children.Add(myView, column, row);
						gridSelectedGuests.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
						row++;
						column = 0;
					}
				}
			}
			else
			{
				gridSelectedGuests.Children.Clear();
			}
		}
		void CreateViewCellContent(CommentResponse item)
		{
			StackLayout mainStack = new StackLayout();
			Grid grid = new Grid()
			{
				Margin = new Thickness(10, 10, 10, 0)
			};
			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30, GridUnitType.Auto) });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Auto) });

			RoundImage roundImage = new RoundImage()
			{
				HeightRequest = 33,
				WidthRequest = 33,
				BorderRadius = 16,
				VerticalOptions = LayoutOptions.Start,
				Source = !string.IsNullOrEmpty(item.user_info.profile_image) ? PageHelper.GetUserImage(item.user_info.profile_image) : Constant.UserDefaultSquareImage,

			};
			ExtendedLabel extendedLabel = new ExtendedLabel()
			{
				TextColor = Color.FromHex(Constant.GrayTextColor),
				FontFace = FontFace.PoppinsSemiBold,
				LineBreakMode = LineBreakMode.TailTruncation,
				FontSize = 13,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = item.user_info.fullname

			};
			ExtendedLabel extendedLabelComment = new ExtendedLabel()
			{
				TextColor = Color.FromHex(Constant.GrayTextColor),
				FontFace = FontFace.PoppinsLight,
				FontSize = 11,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = item.comment_text
			};
			ExtendedLabel extendedLabelCommentDate = new ExtendedLabel()
			{
				TextColor = Color.FromHex(Constant.GrayTextColor),
				FontFace = FontFace.PoppinsRegular,
				LineBreakMode = LineBreakMode.TailTruncation,
				FontSize = 11,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = DateTime.Parse(item.send_date).ToLocalTime().ToString("ddd,dd MMM, h:mm tt"),

			};
            eventViewModel.IsOwner = item.is_owner;
			Image editIcon = new Image()
			{
                Source = Constant.OptionsIcon,
                Aspect = Aspect.AspectFit,
				ClassId = item.id.ToString()

			};
            //Image reportIcon = new Image()
            //{
            //    Source = Constant.DeleteIcon,
            //    Aspect = Aspect.AspectFit,
            //    ClassId = item.id.ToString(),
            //    Margin = new Thickness(0, 0, 10, 0),

            //};
			Grid guestGrid;
			if (item.tagged_users != null && item.tagged_users.Count > 0)
			{
				guestGrid = new Grid();
				guestGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) });
				guestGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) });

				Taggedfriends = new ObservableCollection<UserInfo>();
				foreach (var i in item.tagged_users)
				{
					Taggedfriends.Add(i);
				}
				CreateGuestGrid(guestGrid);
			}
			else
			{
				guestGrid = new Grid();
			}
			var tapGestureRecognizer = new TapGestureRecognizer();
			//tapGestureRecognizer.Tapped += Delete_Tapped;
            tapGestureRecognizer.Tapped += Edit_Icon_Tapped;

            var tapGestureRecognizerReport = new TapGestureRecognizer();
            tapGestureRecognizerReport.Tapped += Report_Tapped;
			grid.Children.Add(roundImage, 0, 0);
			Grid.SetRowSpan(roundImage, 3);
			grid.Children.Add(extendedLabel, 1, 0);
			grid.Children.Add(extendedLabelComment, 1, 1);
			grid.Children.Add(extendedLabelCommentDate, 1, 2);
            //grid.Children.Add(reportIcon, 2, 0);
            //grid.Children.Add(deleteIcon, 3, 0);
            grid.Children.Add(editIcon, 2, 0);
			grid.Children.Add(guestGrid, 1, 3);
            editIcon.GestureRecognizers.Add(tapGestureRecognizer);
            //reportIcon.GestureRecognizers.Add(tapGestureRecognizerReport);
			BoxView box = new BoxView()
			{
				HeightRequest = 1,
				Margin = new Thickness(0, 10, 0, 0),
				BackgroundColor = Color.FromHex(Constant.BoxViewColor),
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
			mainStack.Children.Add(grid);
			mainStack.Children.Add(box);
			stckComment.Children.Add(mainStack);
		}
		Frame CreateGuestList(UserInfo friend)
		{
			Frame frame = new Frame
			{
				HasShadow = false,
				BackgroundColor = Color.FromHex(Constant.TagsbackgroundColor),
				CornerRadius = 18,
				Margin = 0,
				Padding = 0,
				ClassId = Convert.ToString(friend.id)
			};
			StackLayout stackLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Spacing = 0,
				Margin = 0,
				Padding = new Thickness(10, 5, 10, 5),
				BackgroundColor = Color.Transparent,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand

			};
			CircleImage userImage = new CircleImage
			{
				HeightRequest = 23,
				WidthRequest = 23,
				Margin = new Thickness(0, 0, 0, 0),
				Aspect = Aspect.Fill,
				Source = !string.IsNullOrEmpty(friend.profile_image) ? PageHelper.GetUserImage(friend.profile_image) : Constant.ProfileIcon
			};
			Label userName = new Label
			{
				TextColor = Color.FromHex(Constant.AddEventEntriesColor),
				FontSize = 12,
				Margin = new Thickness(0, 2, 0, 2),
				LineBreakMode = LineBreakMode.TailTruncation,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Text = friend.fullname
			};

			stackLayout.Children.Add(userImage);
			stackLayout.Children.Add(userName);
			frame.Content = stackLayout;
			return frame;
		}

		void CommentList_Scrolled(object sender, Xamarin.Forms.ScrolledEventArgs e)
		{
			var scrollView = (ScrollView)sender;
			var scHeight = scrollView.ContentSize.Height;
			var yHorz = scrollView.ScrollY;
			var scrollDiff = scHeight - yHorz;
			if (scrollDiff <= App.ScreenHeight-(gradientStack.Height-15) && eventViewModel.pageNoComment < eventViewModel.totalPagesComment)
			{
				eventViewModel.pageNoComment++;
				tempCommentList.Clear();
				GetCommentsList();
			}
		}
        void Cancel_Clicked_Commentpopup(object sender, System.EventArgs e)
        {
            stackPopUpForComment.IsVisible = false;
            stackReport.IsVisible = false;
            grdOverlayDialog.IsVisible = false;
            eventViewModel.IsLoading = false;
        }
		#endregion


	}
}

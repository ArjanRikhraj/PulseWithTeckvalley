using System;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Pulse
{
	public partial class AppNavigationHeader : ContentView
	{
		int tapCount;
		public AppNavigationHeader()
		{
			InitializeComponent();
			if (Device.RuntimePlatform == Device.Android)
			{
				btnBack.VerticalOptions = LayoutOptions.Center;
				backText.VerticalOptions = LayoutOptions.End;
			}
            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    this.Padding = new Thickness(0, 20, 0, 10);
            //}
		}
		#region Properties

		public String HeaderTitle
		{
			get { return backText.Text; }
			set { backText.Text = value; }
		}

		public Color HeaderTitleColor
		{
			get { return backText.TextColor; }
			set { backText.TextColor = value; }
		}
		public bool IsHeaderTitleVisible
		{
			get { return backText.IsVisible; }
			set { backText.IsVisible = value; }
		}
		public bool IsBackButtonVisible
		{
			set { btnBack.IsVisible = value; }
			get { return btnBack.IsVisible; }
		}

		public bool IslblDoneTabVisible
		{
			get
			{
				return lblDoneTab.IsVisible;

			}
			set
			{

				lblDoneTab.IsVisible = value;

			}
		}

		public string LblDoneTabTitle
		{
			get
			{
				return lblDoneTab.Text;

			}
			set
			{
				lblDoneTab.Text = value;
				OnPropertyChanged("LblDoneTabTitle");
			}
		}





		#endregion Properties

		async void Back_Tapped(object sender, System.EventArgs e)
		{

			if (CrossConnectivity.Current.IsConnected)
			{
				if (tapCount < 1)
				{
					tapCount = 1;
					await Navigation.PopModalAsync();
					tapCount = 0;
				}
			}
			else
			{
				await App.Instance.Alert(Constant.NetworkDisabled, Constant.AlertTitle, Constant.Ok);
				tapCount = 0;
			}
		}
	}
}

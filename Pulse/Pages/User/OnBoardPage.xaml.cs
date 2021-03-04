using System.Collections.Generic;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Pulse
{
	public partial class OnBoardPage : BaseContentPage
	{
		List<OnBoardListData> listSource;
		#region Constructor
		public OnBoardPage()
		{
			InitializeComponent();
			BindingContext = this;
			BindCarouselList();
			Settings.IsOnBoardingShown=true;
		}
		#endregion
        protected async override void OnAppearing()
		{
            base.OnAppearing(); 
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(false);
            App.GetPermission();

		}
		#region Methods
		void BindCarouselList()
		{
			listSource = new List<OnBoardListData>();
			listSource.Add(new OnBoardListData
			{
				OnBoardImgSrc = Constant.EventlistOnboardImage,
				OnBoardLabel1 = "Discover & Attend Events ",
				OnBoardLabel2 = "Happening Around You"
			});
			listSource.Add(new OnBoardListData
			{
				OnBoardImgSrc = Constant.CreateEventOnboardImage,
				OnBoardLabel1 = "Create & Manage Events,",
				OnBoardLabel2 = "Invite Friends & Create Buzz!"
			});
			listSource.Add(new OnBoardListData
			{
				OnBoardImgSrc = Constant.FriendsOnboardImage,
				OnBoardLabel1 = "Connect With Your Friends",
				OnBoardLabel2 = "& Grow Your Network!"
			});
			crslOnBoard.ItemsSource = listSource;
			crslOnBoard.PositionSelected += (sender, e) =>
			{
				btnSkip.Text = "Skip";
				if (crslOnBoard.Position == 2)
				{
					btnSkip.Text = Constant.SignUpSmallLetter;
				}
			};
		}

		void Skip_Clicked(object sender, System.EventArgs e)
		{
			if (!btnSkip.InputTransparent)
				Navigation.PushAsync(new SignUpPage(), true);
		}
	}
	#endregion

	public class OnBoardListData
	{
		public string OnBoardImgSrc { get; set; }
		public string OnBoardLabel1 { get; set; }
		public string OnBoardLabel2 { get; set; }
	}
}

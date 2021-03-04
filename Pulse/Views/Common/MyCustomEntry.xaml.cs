using Xamarin.Forms;
namespace Pulse
{
	public partial class MyCustomEntry : ContentView
	{

		#region constructor
		public MyCustomEntry()
		{
			InitializeComponent();
			entry.BindingContext = this;
			entry.SetBinding(Entry.TextProperty, TextProperty.PropertyName, BindingMode.TwoWay);
			if (Device.RuntimePlatform == Device.Android)
			{
				stackMain.Padding = new Thickness(0, 2, 0, 0);
				stackMain.Spacing = 0;
				stackMain.Margin = new Thickness(0, 0, 0, 0);
				lblSmall.Margin = new Thickness(0, 0, 0, 0);
				lblSmall.FontSize = 11;
				entry.FontSize = 14;
				entry.Margin = new Thickness(0, 0, 0, 0);
				box.Margin = new Thickness(0, 0, 0, 0);
			}
		}
		#endregion

		#region public properties
		public string LabelText
		{
			get { return lblSmall.Text; }
			set
			{
				lblSmall.Text = value;
			}
		}


		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(nameof(Text), typeof(string), typeof(MyCustomEntry), null, BindingMode.TwoWay);
		public string Text
		{
			get
			{
				return (string)GetValue(TextProperty);
			}
			set
			{
				SetValue(TextProperty, value);
			}
		}


		public string EntryPlaceholder
		{
			get { return entry.Placeholder; }
			set
			{
				entry.Placeholder = value;
			}
		}
		public Keyboard KeyBoard
		{
			get { return entry.Keyboard; }
			set
			{
				entry.Keyboard = value;
			}
		}
		public int EntryMaxLength
		{
			get { return entry.MaxLength; }
			set
			{
				entry.MaxLength = value;
			}
		}

		public bool IsPassword
		{
			get { return entry.IsPassword; }
			set
			{
				entry.IsPassword = value;
			}
		}

		public bool IsPasswordEyeImageVisible
		{
			get { return stackPasswordEyeIcon.IsVisible; }
			set
			{
				stackPasswordEyeIcon.IsVisible = value;
			}
		}
		public bool IsCheckIconVisible
		{
			get { return stackCheckIcon.IsVisible; }
			set
			{
				stackCheckIcon.IsVisible = value;
			}
		}
		#endregion

		#region Private methods
		void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.NewTextValue))
			{
				lblSmall.TextColor = Color.Transparent;
				if (IsPasswordEyeImageVisible)
				{
					passwordHideImage.IsVisible = true;
					passwordShowImage.IsVisible = false;
					entry.IsPassword = true;
				}
			}
			else
			{
				lblSmall.TextColor = Color.FromHex(Constant.GrayTextColor);
			}
		}

		void Eye_Stack_Tapped(object sender, System.EventArgs e)
		{
			entry.IsPassword = !entry.IsPassword;
			passwordHideImage.IsVisible = !passwordHideImage.IsVisible;
			passwordShowImage.IsVisible = !passwordShowImage.IsVisible;
		}

		#endregion


	}
}

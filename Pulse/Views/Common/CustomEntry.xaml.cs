using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Pulse
{
	public partial class CustomEntry : ContentView
	{
		#region constructor
		public CustomEntry()
		{
			InitializeComponent();
			entry.BindingContext = this;
			entry.SetBinding(Entry.TextProperty, TextProperty.PropertyName, BindingMode.TwoWay);
			if (Device.RuntimePlatform == Device.Android)
			{
				box.Margin = new Thickness(14, 6, 0, 0);
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
		public ImageSource SideImage
		{
			get { return sideImage.Source; }
			set
			{
				sideImage.Source = value;
			}
		}
		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomEntry), null, BindingMode.TwoWay);
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
		#endregion

		#region Private methods
		void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.NewTextValue))
			{
				lblSmall.TextColor = Color.Transparent;
			}
			else
			{
				lblSmall.TextColor = Color.FromHex(Constant.AddEventEntriesColor);
			}
		}
		#endregion
	}
}

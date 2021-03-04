using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Pulse
{
	public partial class DisabledCustomEntry : ContentView
	{
		public DisabledCustomEntry()
		{
			InitializeComponent();
			entry.BindingContext = this;
			entry.SetBinding(Entry.TextProperty, TextProperty.PropertyName, BindingMode.TwoWay);
			if (Device.RuntimePlatform == Device.Android)
			{
				box.Margin = new Thickness(14, 6, 0, 0);
			}
		}
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
		#endregion
	}
}

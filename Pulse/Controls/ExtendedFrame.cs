using System;
using Xamarin.Forms;

namespace Pulse
{
	public class ExtendedFrame : Frame
	{

		public static readonly BindableProperty BorderWidthProperty =
			BindableProperty.Create(Constant.BorderWidth, typeof(int), typeof(ExtendedFrame), 2);
		/// <summary>
		/// Gets or sets the BorderWidth
		/// </summary>
		public int BorderWidth
		{
			get { return (int)this.GetValue(BorderWidthProperty); }
			set { this.SetValue(BorderWidthProperty, value); }
		}

		public static readonly BindableProperty BorderRadiusProperty =
			BindableProperty.Create(Constant.BorderRadius, typeof(int), typeof(ExtendedFrame), 2);
		/// <summary>
		/// Gets or sets the BorderWidth
		/// </summary>
		public int BorderRadius
		{
			get { return (int)this.GetValue(BorderRadiusProperty); }
			set { this.SetValue(BorderRadiusProperty, value); }
		}


	}
}

using System;
using Xamarin.Forms;

namespace Pulse
{
	public delegate void PickerDoneClickEventHandler(object sender, EventArgs e);
	public class ExtendedPicker : Picker
	{
		public event PickerDoneClickEventHandler DoneClicked;
		public virtual void PickerDoneClicked(EventArgs e)
		{
			if (DoneClicked != null)
			{
				DoneClicked(this, e);//Raise the event
			}
		}
		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create("TextColor", typeof(Color), typeof(Pulse.ExtendedPicker), Color.Default);


		public Color TextColor
		{
			get
			{
				return (Color)GetValue(TextColorProperty);
			}
			set
			{
				SetValue(TextColorProperty, value);
			}
		}
		public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create("PlaceHolder", typeof(string), typeof(ExtendedPicker), "");

		public string PlaceHolder
		{
			get
			{
				return (string)GetValue(PlaceHolderProperty);
			}
			set
			{
				SetValue(PlaceHolderProperty, value);
			}
		}

		public static readonly BindableProperty HasBorderProperty =
			BindableProperty.Create("HasBorder", typeof(bool), typeof(Pulse.ExtendedPicker), true);

		public bool HasBorder
		{
			get
			{
				return (bool)GetValue(HasBorderProperty);
			}
			set
			{
				SetValue(HasBorderProperty, value);
			}
		}

		public static readonly BindableProperty TextSizeProperty =
			BindableProperty.Create("TextSize", typeof(float), typeof(Pulse.ExtendedPicker), 10f);

		public float TextSize
		{
			get
			{
				return (float)GetValue(TextSizeProperty);
			}
			set
			{
				SetValue(TextSizeProperty, value);
			}
		}

		public static readonly BindableProperty TextFontFamilyProperty =
			BindableProperty.Create("TextFontFamily", typeof(string), typeof(ExtendedPicker), "Calibri");

		public string TextFontFamily
		{
			get
			{
				return (string)GetValue(TextFontFamilyProperty);
			}
			set
			{
				SetValue(TextSizeProperty, value);
			}
		}

		/// <summary>
		/// The BackGround property
		/// </summary>
		public static readonly BindableProperty BackGroundProperty =
			BindableProperty.Create("BackGround", typeof(bool), typeof(ExtendedPicker), true);

		/// <summary>
		/// Sets underline 
		/// </summary>
		public bool BackGround
		{
			get { return (bool)GetValue(BackGroundProperty); }
			set { SetValue(BackGroundProperty, value); }
		}

		/// <summary>
		/// The PlaceholderTextColor property
		/// </summary>
		public static readonly BindableProperty PlaceholderTextColorProperty =
			BindableProperty.Create("PlaceholderTextColor", typeof(Color), typeof(ExtendedPicker), Color.Default);

		public Color PlaceholderTextColor
		{
			get { return (Color)GetValue(PlaceholderTextColorProperty); }
			set { SetValue(PlaceholderTextColorProperty, value); }
		}
	}
}

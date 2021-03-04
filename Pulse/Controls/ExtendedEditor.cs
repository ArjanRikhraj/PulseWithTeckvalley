using System;
using Xamarin.Forms;

namespace Pulse
{
	public class ExtendedEditor : Editor
	{
		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create<ExtendedEditor, string>(view => view.Placeholder, String.Empty);
		public ExtendedEditor()
		{

		}
		public string Placeholder
		{
			get
			{
				return (string)GetValue(PlaceholderProperty);
			}

			set
			{
				SetValue(PlaceholderProperty, value);

			}
		}
	}
}
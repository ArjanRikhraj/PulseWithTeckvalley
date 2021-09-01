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
        public new event EventHandler Completed;

        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(nameof(ReturnType),
            typeof(ReturnType), typeof(ExtendedEditor),
            ReturnType.Done, BindingMode.OneWay);

        public ReturnType ReturnType
        {
            get { return (ReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }

        public void InvokeCompleted()
        {
            if (this.Completed != null)
                this.Completed.Invoke(this, null);
        }
    }
    public enum ReturnType
    {
        Go,
        Next,
        Done,
        Send,
        Search
    }
}

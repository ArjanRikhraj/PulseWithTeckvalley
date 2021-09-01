using Android.Views.InputMethods;
using Android.Widget;
using Pulse;
using Pulse.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]

namespace Pulse.Droid
{
	public class ExtendedEditorRenderer : EditorRenderer
	{
		#region Private control
		#endregion

		/// <summary>
		/// On the element changed.
		/// </summary>
		/// <returns>The element changed.</returns>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);
			ExtendedEditor editor = (ExtendedEditor)this.Element;
			if (Control != null)
			{
				var element = Element as ExtendedEditor;
				Control.Background = null;
				Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
				Control.VerticalScrollBarEnabled = true;
				Control.Hint = element.Placeholder;
				Control.SetHintTextColor(Android.Graphics.Color.Rgb(86, 88, 92));
				Control.Typeface = FontHelper.GetFontFace(FontFace.PoppinsRegular);
				if (editor != null)
				{
					SetReturnType(editor);

                    // Editor Action is called when the return button is pressed
					Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
					{
						if (editor.ReturnType != ReturnType.Next)
							editor.Unfocus();

							// Call all the methods attached to custom_entry event handler Completed  
							editor.InvokeCompleted();
					};
				}
			}
		}
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (Control != null)
			{
				Control.Background = null;
				Control.Typeface = FontHelper.GetFontFace(FontFace.PoppinsRegular);
			}

		}
		private void SetReturnType(ExtendedEditor entry)
		{
			ReturnType type = entry.ReturnType;

			switch (type)
			{
				case ReturnType.Done:
					Control.ImeOptions = ImeAction.Done;
					Control.SetImeActionLabel("Done", ImeAction.Done);
					break;
				case ReturnType.Next:
					Control.ImeOptions = ImeAction.Next;
					Control.SetImeActionLabel("Next", ImeAction.Next);
					break;
				case ReturnType.Send:
					Control.ImeOptions = ImeAction.Send;
					Control.SetImeActionLabel("Send", ImeAction.Send);
					break;
				case ReturnType.Search:
					Control.ImeOptions = ImeAction.Search;
					Control.SetImeActionLabel("Search", ImeAction.Search);
					break;
				default:
					Control.ImeOptions = ImeAction.Done;
					Control.SetImeActionLabel("Done", ImeAction.Done);
					break;
			}
		}
	}
}

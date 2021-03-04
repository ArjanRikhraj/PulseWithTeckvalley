using System;
using Foundation;
using Pulse;
using Pulse.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]

namespace Pulse.iOS
{

	public class ExtendedLabelRenderer : LabelRenderer
	{
		/// <summary>
		/// Ons the element changed.
		/// </summary>
		/// <returns>The element changed.</returns>
		/// <param name="e">E.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			try
			{
				if (e.NewElement != null)
				{
					var view = e.NewElement as ExtendedLabel;
                    if (view != null && Control != null)
					{
						SetFont(view);
					}
                    //if (view.LineSpacing > 0.0)
                    //{
                    //    var labelString = new NSMutableAttributedString(view.Text);
                    //    var paragraphStyle = new NSMutableParagraphStyle { LineSpacing = (nfloat)view.LineSpacing };
                    //    var style = UIStringAttributeKey.;
                    //    var range = new NSRange(0, labelString.Length);

                    //    labelString.AddAttribute(style, paragraphStyle, range);
                    //    Control.AttributedText = labelString;
                    //}
				}
			}
			catch (System.Exception)
			{

			}

		}

		private void SetFont(ExtendedLabel view)
		{
			Control.Font = FontHelper.GetFont(view.FontFace, view.FontSize);
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			var view = this.Element as ExtendedLabel;
			if (view != null && Control != null)
			{
				SetFont(view);
			}
		}
	}

}

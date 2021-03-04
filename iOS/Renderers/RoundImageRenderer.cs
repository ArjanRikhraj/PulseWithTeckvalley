using Pulse;
using Pulse.iOS;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using System.Diagnostics;
using Foundation;
using CoreAnimation;
using CoreGraphics;

[assembly: ExportRenderer(typeof(RoundImage), typeof(ImageCircleRendererCustom))]
namespace Pulse.iOS
{
	/// <summary>
	/// ImageCircle Implementation
	/// </summary>
	[Preserve(AllMembers = true)]
	public class ImageCircleRendererCustom : ImageRenderer
	{
		/// <summary>
		/// Used for registration with dependency service
		/// </summary>
		public async static void Init()
		{
			var temp = DateTime.Now;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);
			if (Element == null)
				return;
			CreateCircle();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			CreateCircle();
		}

		private void CreateCircle()
		{
			try
			{
				var min = Math.Min(Element.Width, Element.Height);
				Control.Layer.CornerRadius = (nfloat)(min / ((RoundImage)Element).BorderRadius);
				Control.Layer.MasksToBounds = false;
				Control.BackgroundColor = ((RoundImage)Element).FillColor.ToUIColor();
				Control.ClipsToBounds = true;
				var externalBorder = new CALayer();
				externalBorder.CornerRadius = Control.Layer.CornerRadius;
				externalBorder.Frame = new CGRect(-.5, -.5, min + 1, min + 1);
				externalBorder.BorderColor = ((RoundImage)Element).BorderColor.ToCGColor();
				externalBorder.BorderWidth = ((RoundImage)Element).BorderThickness;

				Control.Layer.AddSublayer(externalBorder);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to create circle image: " + ex);
			}
		}
	}
}
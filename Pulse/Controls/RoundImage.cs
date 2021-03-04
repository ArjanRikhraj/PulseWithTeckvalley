using Xamarin.Forms;

namespace Pulse
{
	/// <summary>
	/// ImageCircle Interface
	/// </summary>
	public class RoundImage : Image
	{
		/// <summary>
		/// Thickness property of border
		/// </summary>
		public static readonly BindableProperty BorderThicknessProperty =
		  BindableProperty.Create(propertyName: nameof(BorderThickness),
			  returnType: typeof(int),
			  declaringType: typeof(RoundImage),
			  defaultValue: 0);

		/// <summary>
		/// Border thickness of circle image
		/// </summary>
		public int BorderThickness
		{
			get { return (int)GetValue(BorderThicknessProperty); }
			set { SetValue(BorderThicknessProperty, value); }
		}

		/// <summary>
		/// Thickness property of border
		/// </summary>
		public static readonly BindableProperty BorderRadiusProperty =
		  BindableProperty.Create(propertyName: nameof(BorderRadius),
			  returnType: typeof(int),
			  declaringType: typeof(RoundImage),
			  defaultValue: 0);

		/// <summary>
		/// Border thickness of circle image
		/// </summary>
		public int BorderRadius
		{
			get { return (int)GetValue(BorderRadiusProperty); }
			set { SetValue(BorderRadiusProperty, value); }
		}

		/// <summary>
		/// Color property of border
		/// </summary>
		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create(propertyName: nameof(BorderColor),
			  returnType: typeof(Color),
			  declaringType: typeof(RoundImage),
			  defaultValue: Color.White);


		/// <summary>
		/// Border Color of circle image
		/// </summary>
		public Color BorderColor
		{
			get { return (Color)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}

		/// <summary>
		/// Color property of fill
		/// </summary>
		public static readonly BindableProperty FillColorProperty =
			BindableProperty.Create(propertyName: nameof(FillColor),
			  returnType: typeof(Color),
			  declaringType: typeof(RoundImage),
			  defaultValue: Color.Transparent);

		/// <summary>
		/// Fill color of circle image
		/// </summary>
		public Color FillColor
		{
			get { return (Color)GetValue(FillColorProperty); }
			set { SetValue(FillColorProperty, value); }
		}

	}
}
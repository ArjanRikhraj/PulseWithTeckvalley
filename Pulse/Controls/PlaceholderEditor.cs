using System;
using Xamarin.Forms;
namespace Pulse
{
    public class PlaceholderEditor : Editor 
    {
        public PlaceholderEditor()
        {
            this.FontFace = FontFace.PoppinsRegular;
        } 
        private static BindableProperty PlaceholderProperty 
        = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(PlaceholderEditor)); 

        private static BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(PlaceholderEditor), Color.Gray);
        public string Placeholder { get { return (string)GetValue(PlaceholderProperty); } set { SetValue(PlaceholderProperty, value); } }
        public Color PlaceholderColor { get { return (Color)GetValue(PlaceholderColorProperty); } set { SetValue(PlaceholderColorProperty, value); } } 
        public FontFace FontFace { get; set; } }
}

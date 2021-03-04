using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using Pulse;
using Pulse.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Pulse.Droid
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        List<CustomPin> customPins;
        public bool IsboostEvent;
        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (e.OldElement != null)
                {
                    NativeMap.InfoWindowClick -= OnInfoWindowClick;
                }

                if (e.NewElement != null)
                {
                    var formsMap = (CustomMap)e.NewElement;
                    customPins = formsMap.CustomPins;
                    Control.GetMapAsync(this);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);
            NativeMap.UiSettings.ZoomControlsEnabled = false;
            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.MarkerClick += NativeMap_MarkerClick;
            NativeMap.SetInfoWindowAdapter(this);
        }

        async void NativeMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin != null)
            {
                App myApp = App.Current as App;
                if (null != myApp)
                {
                    if (customPin.IsMoreThanOneLocation)
                    {
                        App.Lognitude = customPin.Lognitude;
                        App.Latitude = customPin.Latitude;
                        await myApp.GetMapEvents();
                    }
                    else
                    {
                        e.Marker.ShowInfoWindow();
                    }
                }

            }
        }

        protected override MarkerOptions CreateMarker(Pin pin1)
        {
            var pin = (CustomPin)pin1;
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            
            if (pin.IsCurrentLocation)
            {
                //marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.icUserCurrentLocation));
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.icUserCurrentLocation));

            }
            else if (pin.IsBoostEvent)
            {
                if (Convert.ToInt32(pin.SameLocationPinCount) > 0)
                {
                    marker.SetIcon(BitmapDescriptorFactory.FromBitmap(createCustomMarker(MainActivity.APPLICATION_CONTEXT, Resource.Drawable.icBoostEventLocation, pin.SameLocationPinCount)));

                }
                else
                {
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.icBoostEventLocation));
                }

            }
            else
            {
                if (Convert.ToInt32(pin.SameLocationPinCount) > 0)
                {
                    marker.SetIcon(BitmapDescriptorFactory.FromBitmap(createCustomMarker(MainActivity.APPLICATION_CONTEXT, Resource.Drawable.map_pin, pin.SameLocationPinCount)));

                }
                else
                {
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.map_pin));
                }
            }
            return marker;
        }
        public static Bitmap createCustomMarker(Context context, int resource, String _name)
        {

            Android.Views.View marker = ((LayoutInflater)context.GetSystemService(Context.LayoutInflaterService)).Inflate(Resource.Layout.PinCountLayout, null);
            ImageView image = (ImageView)marker.FindViewById(Resource.Id.imageView1);
            image.SetImageResource(resource);
            TextView txt_name = (TextView)marker.FindViewById(Resource.Id.textView1);
            txt_name.Text = _name;
            DisplayMetrics displayMetrics = new DisplayMetrics();
            ((MainActivity)(context)).WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            marker.LayoutParameters = new ViewGroup.LayoutParams(52, ViewGroup.LayoutParams.WrapContent);
            marker.Measure(displayMetrics.WidthPixels, displayMetrics.HeightPixels);
            marker.Layout(0, 0, displayMetrics.WidthPixels, displayMetrics.HeightPixels);
            marker.BuildDrawingCache();
            Bitmap bitmap = Bitmap.CreateBitmap(marker.MeasuredWidth, marker.MeasuredHeight, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);
            marker.Draw(canvas);
            return bitmap;
        }


        async void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin != null)
            {
                App myApp = App.Current as App;
                if (null != myApp)
                {
                    if (customPin.Id == "Xamarin")
                    {
                    }
                    else if (customPin.IsMoreThanOneLocation)
                    {

                    }
                    else
                    {
                        await myApp.FetchEventDetailMethod(customPin.Id);
                    }
                }

            }
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }
                if (customPin.IsBoostEvent)
                {
                    view = inflater.Inflate(Resource.Layout.BoostedEventWindow, null);
                    var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                    var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
                    if (infoTitle != null)
                    {
                        infoTitle.Text = marker.Title;
                        infoTitle.Typeface = FontHelper.GetFontFace(FontFace.PoppinsSemiBold);
                    }
                    if (infoSubtitle != null)
                    {
                        infoSubtitle.Text = marker.Snippet;
                        infoSubtitle.Typeface = FontHelper.GetFontFace(FontFace.PoppinsRegular);
                        infoSubtitle.TextSize = 8;
                    }
                }
                else if (customPin.IsCurrentLocation)
                {
                    view = inflater.Inflate(Resource.Layout.CurrentLocationWindow, null);

                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.XamarinMapInfoWindow, null);
                    var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                    var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
                    if (infoTitle != null)
                    {
                        infoTitle.Text = marker.Title;
                        infoTitle.Typeface = FontHelper.GetFontFace(FontFace.PoppinsSemiBold);
                    }
                    if (infoSubtitle != null)
                    {
                        infoSubtitle.Text = marker.Snippet;
                        infoSubtitle.Typeface = FontHelper.GetFontFace(FontFace.PoppinsRegular);
                        infoSubtitle.TextSize = 8;
                    }
                }
                return view;
            }
            return null;
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.IsBoostEvent)
                {
                    IsboostEvent = true;
                }
                else
                {
                    IsboostEvent = false;
                }
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}


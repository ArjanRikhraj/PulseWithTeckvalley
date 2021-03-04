using System;
using System.Collections.Generic;
using CoreGraphics;
using Pulse;
using Pulse.iOS;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;
using System.Drawing;
using Foundation;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Pulse.iOS
{
    public class CustomMapRenderer : MapRenderer
    {
        List<CustomPin> customPins;
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                customPins = formsMap.CustomPins;
                nativeMap.Delegate = new CustomMapViewDelegate(formsMap.CustomPins as List<CustomPin>);

            }
        }



        void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        {
            var customView = e.View as CustomMKPinAnnotationView;
            if (customView != null)
            {
                App myApp = App.Current as App;
                if (null != myApp)
                {
                    if (customView.Id == "Xamarin")
                    {
                    }
                    else if (customView.IsMoreThanOneLocation)
                    {

                    }
                    else
                    {
                        myApp.FetchEventDetailMethod(customView.Id);
                    }
                }
            }
        }
        MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            MKPointAnnotation anno = null;

            if (annotation is MKUserLocation)
            {
                return null;
            }
            else
            {
                anno = annotation as MKPointAnnotation;
            }

            var customPin = GetCustomPin(annotation as MKPointAnnotation);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }


            annotationView = mapView.DequeueReusableAnnotation(customPin.Id);
            if (annotationView == null)
            {
                annotationView = new MKAnnotationView(annotation, customPin.Id);
                if (customPin.IsCurrentLocation)
                {
                    annotationView.Image = UIImage.FromFile("icUserCurrentLocation.png");

                }
                else if (customPin.IsBoostEvent)
                {
                    annotationView.Image = UIImage.FromFile("icBoostEventLocation.png");
                }
                else
                {
                    annotationView.Image = UIImage.FromFile("map_pin.png");
                }

            }


            annotationView.CanShowCallout = false;

            return annotationView;
        }
        CustomPin GetCustomPin(MKPointAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}
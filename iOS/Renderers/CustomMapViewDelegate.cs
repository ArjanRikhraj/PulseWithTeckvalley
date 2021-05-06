using System;
using System.Collections.Generic;
using System.Drawing;
using CoreGraphics;
using CoreLocation;
using MapKit;
using MetalKit;
using UIKit;
using Xamarin.Forms.Maps;

namespace Pulse.iOS
{
    public class CustomMapViewDelegate : MKMapViewDelegate
    {
        UIView customPinView;
        List<CustomPin> customPins;
        XamarinPinView customBoostView;
        XamarinNormalPinView normalPinView;
        CurrentLocationPinview currentLocationPinview;
        public CustomMapViewDelegate(List<CustomPin> pins)
        {
            customPins = pins;
        }

        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
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
            //if (customPin == null)
            //{
            //    //throw new Exception("Custom pin not found");
            //}
            if (customPin != null)
            {
                annotationView = mapView.DequeueReusableAnnotation(customPin.Id);
                if (annotationView == null)
                {
                    annotationView = new CustomMKPinAnnotationView(annotation, customPin.Id);
                    UILabel label = new UILabel(new CGRect(2, -20, 25, 25))
                    {
                        Text = customPin.SameLocationPinCount,
                        TextAlignment = UITextAlignment.Center,
                        TextColor = UIColor.White
                    };
                    label.Layer.CornerRadius = 12;
                    label.Layer.MasksToBounds = true;
                    if (customPin.IsCurrentLocation)
                    {
                        annotationView.Image = UIImage.FromFile("icUserCurrentLocation.png");
                        label.BackgroundColor = UIColor.FromRGB(255, 149, 105);

                    }
                    else if (customPin.IsBoostEvent)
                    {
                        annotationView.Image = UIImage.FromFile("icBoostEvent.png");
                        label.BackgroundColor = UIColor.FromRGB(255, 149, 105);
                    }
                    else
                    {
                        annotationView.Image = UIImage.FromFile("map_pin.png");
                        label.BackgroundColor = UIColor.FromRGB(255, 149, 105);
                    }

                    if (Convert.ToInt32(customPin.SameLocationPinCount) > 0)
                    {
                        annotationView.AddSubview(label);
                    }

                }
            //This removes the bubble that pops up with the title and everything
            ((CustomMKPinAnnotationView)annotationView).FormsIdentifier = customPin.Id;
                ((CustomMKPinAnnotationView)annotationView).IsBoostEvent = customPin.IsBoostEvent;
                ((CustomMKPinAnnotationView)annotationView).IsCurrentLocation = customPin.IsCurrentLocation;
                ((CustomMKPinAnnotationView)annotationView).EventName = customPin.EventName;
                ((CustomMKPinAnnotationView)annotationView).EventDate = customPin.EventDateTime;
                ((CustomMKPinAnnotationView)annotationView).Id = customPin.Id;
                ((CustomMKPinAnnotationView)annotationView).IsMoreThanOneLocation = customPin.IsMoreThanOneLocation;
                ((CustomMKPinAnnotationView)annotationView).Latitude = customPin.Latitude;
                ((CustomMKPinAnnotationView)annotationView).Lognitude = customPin.Lognitude;
                if (customPin.IsCurrentLocation)
                {
                    annotationView.Image = UIImage.FromFile("icUserCurrentLocation.png");
                }
                else if (customPin.IsBoostEvent)
                {
                    annotationView.Image = UIImage.FromFile("icBoostEvent.png");
                }
                else
                {
                    annotationView.Image = UIImage.FromFile("map_pin.png");
                }
                if (customPin.IsBoostEvent)
                {
                    annotationView.CanShowCallout = false;
                }
                else
                {
                    if (!customPin.IsMoreThanOneLocation)
                    {
                        annotationView.CanShowCallout = false;
                    }
                }
            }
            return annotationView;
        }


        public override void CalloutAccessoryControlTapped(MKMapView mapView, MKAnnotationView view, UIControl control)
        {
            base.CalloutAccessoryControlTapped(mapView, view, control);
            var customView = view as CustomMKPinAnnotationView;
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
        public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView view)
        {
            var customView = view as CustomMKPinAnnotationView;
            customPinView = new UIView();
            App myApp = App.Current as App;
            if (customView.IsBoostEvent)
            {
                if (customView.IsMoreThanOneLocation)
                {
                    App.Lognitude = customView.Lognitude;
                    App.Latitude = customView.Latitude;
                    myApp.GetMapEvents();
                    if (!customView.Selected)
                    {
                        customView.RemoveFromSuperview();
                    }
                }
                else
                {
                    customBoostView = new XamarinPinView(customView.EventName, customView.EventDate, customView.Id);
                    customBoostView.Center = new CGPoint(5, -(view.Frame.Height + 10));
                    view.AddSubview(customBoostView);
                }
            }
            else if (customView.IsCurrentLocation)
            {
                currentLocationPinview = new CurrentLocationPinview();
                currentLocationPinview.Center = new CGPoint(5, -(view.Frame.Height + 10));
                view.AddSubview(currentLocationPinview);
            }
            else
            {
                if (customView.IsMoreThanOneLocation)
                {
                    App.Lognitude = customView.Lognitude;
                    App.Latitude = customView.Latitude;
                    myApp.GetMapEvents();
                    if (!customView.Selected)
                    {
                        customView.RemoveFromSuperview();
                    }
                }
                else
                {
                    normalPinView = new XamarinNormalPinView(customView.EventName, customView.EventDate, customView.Id);
                    normalPinView.Center = new CGPoint(5, -(view.Frame.Height + 10));
                    view.AddSubview(normalPinView);
                }
            }
        }


        public override void DidDeselectAnnotationView(MKMapView mapView, MKAnnotationView view)
        {
            var customView = view as CustomMKPinAnnotationView;
            if (!view.Selected)
            {
                customPinView.RemoveFromSuperview();
            }
            if (customView.IsBoostEvent)
            {
                if (!view.Selected)
                {
                    if (normalPinView != null)
                    {
                        normalPinView.RemoveFromSuperview();
                    }
                    if (customBoostView != null)
                    {
                        customBoostView.RemoveFromSuperview();
                    }
                    if (currentLocationPinview != null)
                    {
                        currentLocationPinview.RemoveFromSuperview();
                    }

                }
            }
            else if (customView.IsCurrentLocation)
            {
                if (!view.Selected)
                {
                    if (normalPinView != null)
                    {
                        normalPinView.RemoveFromSuperview();
                    }
                    if (customBoostView != null)
                    {
                        customBoostView.RemoveFromSuperview();
                    }
                    if (currentLocationPinview != null)
                    {
                        currentLocationPinview.RemoveFromSuperview();
                    }

                }
            }
            else
            {
                if (!view.Selected)
                {
                    if (normalPinView != null)
                    {
                        normalPinView.RemoveFromSuperview();
                    }
                    if (customBoostView != null)
                    {
                        customBoostView.RemoveFromSuperview();
                    }
                    if (currentLocationPinview != null)
                    {
                        currentLocationPinview.RemoveFromSuperview();
                    }
                }
            }

        }

        void CustomBoostView_HitTest(CGPoint arg1, UIEvent arg2)
        {

        }



        CustomPin GetCustomPin(MKPointAnnotation annotation)
        {
            if(annotation!=null)
            {
                var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
                foreach (var pin in customPins)
                {
                    if (pin.Position == position)
                    {
                        return pin;
                    }
                }
            }
            return null;
        }
    }
}
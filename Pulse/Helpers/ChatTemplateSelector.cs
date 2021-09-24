using Pulse.Pages.Event.EventMedia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Pulse.Helpers
{
    class ChatTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate imageDataTemplate;
        private readonly DataTemplate videoDataTemplate;

        public ChatTemplateSelector()
        {
            this.imageDataTemplate = new DataTemplate(typeof(ImageViewCell));
            this.videoDataTemplate = new DataTemplate(typeof(VideoViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
         {
            var messageVm = item as EventGallery;
            if (messageVm == null)
                return null;

            return (messageVm.Count % 2 == 0) ? videoDataTemplate : imageDataTemplate;
        }
    }
}
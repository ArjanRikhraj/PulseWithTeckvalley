using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse.Models.Application.Events
{
   public class Stories
    {
        public int StoryCount { get; set; }
        public Story[] story { get; set; }
    }
    public partial class Story:BaseViewModel
    {
        public long id { get; set; }
        public long event_id { get; set; }
        public long user_id { get; set; }
        public string file_url { get; set; }
        public VideoSource videofile_url { get; set; }
        
        public string create_time { get; set; }
        public string create_date { get; set; }
        public string profile_url { get; set; }
        public decimal progressTime { get; set; }
        public decimal ProgressTime
        {
            get
            {
                return progressTime;
            }
            set
            {
                progressTime = value;
                OnPropertyChanged("ProgressTime");
            }
        }
        public bool isVideoVisible { get; set; }
        public bool IsVideoVisible
        {
            get
            {
                return isVideoVisible;
            }
            set
            {
                isVideoVisible = value;
                OnPropertyChanged("IsVideoVisible");
            }
        }
        public bool isImageVisible { get; set; }
        public bool IsImageVisible
        {
            get
            {
                return isImageVisible;
            }
            set
            {
                isImageVisible = value;
                OnPropertyChanged("IsImageVisible");
            }
        }
        public string btnBack { get; set; }
        public string BtnBack
        {
            get
            {
                return btnBack;
            }
            set
            {
                btnBack = value;
                OnPropertyChanged("BtnBack");
            }
        }
        public bool isMenuOptionVisible { get; set; }
        public bool IsMenuOptionVisible
        {
            get
            {
                return isMenuOptionVisible;
            }
            set
            {
                isMenuOptionVisible = value;
                OnPropertyChanged("IsMenuOptionVisible");
            }
        }
        public double videoHeight { get; set; }
        public double VideoHeight
        {
            get
            {
                return videoHeight;
            }
            set
            {
                videoHeight = value;
                OnPropertyChanged("VideoHeight");
            }
        }
    }
    public partial class EventStoryRequest
    {
         public long event_id { get; set; }
    }
    public partial class SaveStoryRequest
    {
        public long event_id { get; set; }
        public long id { get; set; }
        public long user_id { get; set; }
    }
}

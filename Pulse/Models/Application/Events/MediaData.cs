using Xamarin.Forms;
using Plugin.Media.Abstractions;
using System.IO;

namespace Pulse
{
	public class MediaData
	{
		private Stream mediaImageStream = null;
		private byte[] mediaVideoBytes = null;
		public int FileType { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public int id { get; set; }
		public MediaFile MediaFile
		{
			get;
			set;
		}
		public ImageSource imageSource
		{
			get; set;
		}
		public Stream MediaImageStream
		{
			get { return mediaImageStream; }
			set { mediaImageStream = value; }
		}
		public byte[] MediaVideoBytes
		{
			get { return mediaVideoBytes; }
			set { mediaVideoBytes = value; }
		}
		public bool IsAccessed
		{
			get;
			set;
		}
		public bool ProcessedUpload
		{
			get;
			set;
		}
		public int PlaceCheckinId
		{
			get;
			set;
		}
		public MediaData(int fileType, int fileId, string fileName, string filePath, MediaFile mediaFile = null, Stream mediaImageStream = null, ImageSource imageSrc = null)
		{
			this.FileType = fileType;
			this.FileName = fileName;
			this.FilePath = filePath;
			this.MediaFile = mediaFile;
			this.id = fileId;
			this.MediaImageStream = mediaImageStream;
			this.imageSource = imageSrc;
		}
		public MediaData(int fileType, int fileId, string fileName, string filePath, MediaFile mediaFile = null, Stream mediaImageStream = null, byte[] mediaVideoBytes = null, ImageSource imageSrc = null)
		{
			this.FileType = fileType;
			this.FileName = fileName;
			this.FilePath = filePath;
			this.MediaFile = mediaFile;
			this.id = fileId;
			this.MediaImageStream = mediaImageStream;
			this.MediaVideoBytes = mediaVideoBytes;
			this.imageSource = imageSrc;
		}
	}
	public class EventGallery : BaseViewModel
	{
        bool isPlayIconVisible;
        bool isImage;
        bool isVisibleUserName;
		double imageHeight;
		double imageWidth;
		bool isDeleteIconVisible;
		bool isReportIconVisible;
		bool isPrivate;
		string pinIcon;
		public int MediaId { get; set; }
		public int Count { get; set; }
		public int TotalMedia { get; set; }
		public int FileType { get; set; }
		public int EventId { get; set; }
		public int UserId { get; set; }
		public bool IsPlayIconVisible
		{
			get { return isPlayIconVisible; }
			set
			{
				isPlayIconVisible = value;
				OnPropertyChanged("IsPlayIconVisible");
			}
		}

        public bool IsImage
        {
            get { return isImage; }
            set
            {
                isImage = value;
                OnPropertyChanged("IsImage");
            }
        }
        public bool IsVisibleUserName
        {
            get { return isVisibleUserName; }
            set
            {
                isVisibleUserName = value;
                OnPropertyChanged("IsVisibleUserName");
            }
        }
		public bool IsDeleteIconVisible
		{
			get { return isDeleteIconVisible; }
			set
			{
				isDeleteIconVisible = value;
				OnPropertyChanged("IsDeleteIconVisible");
			}
		}
		public bool IsReportIconVisible
		{
			get { return isReportIconVisible; }
			set
			{
				isReportIconVisible = value;
				OnPropertyChanged("IsReportIconVisible");
			}
		}
		public bool IsPrivate
		{
			get { return isPrivate; }
			set
			{
				isPrivate = value;
				OnPropertyChanged("IsPrivate");
			}
		}
		public string PinIcon
		{
			get { return pinIcon; }
			set
			{
				pinIcon = value;
				OnPropertyChanged("PinIcon");
			}
		}
		public string VideoFileName
		{
			get; set;
		}
		public string EventName
		{
			get; set;
		}
		public string FileName
		{
			get; set;
		}
		public string FileUrl
		{
			get; set;
		}
		public string UserName
		{
			get; set;
		}
		public string UserImage
		{
			get; set;
		}
		public string MediaDate
		{
			get; set;
		}
        public string VideoThumbnailFileName
        {
            get;set;
        }
		public double ImageHeight
		{
			get { return imageHeight; }
			set
			{
				imageHeight = value;
				OnPropertyChanged("ImageHeight");
			}
		}
		public double ImageWidth
		{
			get { return imageWidth; }
			set
			{
				imageWidth = value;
				OnPropertyChanged("ImageWidth");
			}
		}
	}
}


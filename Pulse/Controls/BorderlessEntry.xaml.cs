using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pulse
{
	public delegate void SearchEventHandler(object sender);

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BorderlessEntry : Entry
	{
		public event SearchEventHandler OnSearch;
		public BorderlessEntry()
		{
			InitializeComponent();
			this.FontFace = FontFace.PoppinsRegular;
		}

		#region Properties 
		public FontFace FontFace
		{
			get;
			set;
		}

		public int MaxLength
		{
			get;
			set;
		}

		public bool IsSearchControl
		{
			get;
			set;
		}
		public bool HasSmallerHint
		{
			get;
			set;
		}
		#endregion

		public virtual void OnSearched()
		{
			if (OnSearch != null && IsSearchControl)
			{
				OnSearch(this);//Raise the event
			}
		}

	}
}

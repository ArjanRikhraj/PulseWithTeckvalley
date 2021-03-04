using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pulse
{
	public class ExtendedListView : ListView
	{
		public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create<ExtendedListView, ICommand>(bp => bp.LoadMoreCommand, default(ICommand));

		#region Properties

		public ICommand LoadMoreCommand
		{
			get { return (ICommand)GetValue(LoadMoreCommandProperty); }
			set { SetValue(LoadMoreCommandProperty, value); }
		}

		#endregion Properties

        public ExtendedListView() : base(Device.RuntimePlatform == Device.Android ? ListViewCachingStrategy.RecycleElement : ListViewCachingStrategy.RetainElement)
		{
			ItemAppearing += ListViewItemAppearing;
		}

		#region Events
		void ListViewItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			var items = ItemsSource as IList;

			if (items != null && e.Item == items[items.Count - 1] && LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
			{
				LoadMoreCommand.Execute(null);
			}
		}
		#endregion
	}
}


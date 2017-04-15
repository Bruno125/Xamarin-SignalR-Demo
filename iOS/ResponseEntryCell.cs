using System;

using Foundation;
using UIKit;

namespace XamarinSignalR.iOS
{
	public partial class ResponseEntryCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("ResponseEntryCell");
		public static readonly UINib Nib;

		static ResponseEntryCell()
		{
			Nib = UINib.FromName("ResponseEntryCell", NSBundle.MainBundle);
		}

		protected ResponseEntryCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}


		public static ResponseEntryCell Create()
		{
			return (ResponseEntryCell)Nib.Instantiate(null, null)[0];
		}

		public void Update(Response response)
		{
			ValueLabel.Text = response.Message;
			SizeLabel.Text = response.Size;
			DelayLabel.Text = response.Delay;
		}
	}
}

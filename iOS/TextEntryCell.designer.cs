// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace XamarinSignalR.iOS
{
	[Register ("TextEntryCell")]
	partial class TextEntryCell
	{
		[Outlet]
		UIKit.UILabel DelayLabel { get; set; }

		[Outlet]
		UIKit.UILabel SizeLabel { get; set; }

		[Outlet]
		UIKit.UILabel ValueLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ValueLabel != null) {
				ValueLabel.Dispose ();
				ValueLabel = null;
			}

			if (SizeLabel != null) {
				SizeLabel.Dispose ();
				SizeLabel = null;
			}

			if (DelayLabel != null) {
				DelayLabel.Dispose ();
				DelayLabel = null;
			}
		}
	}
}

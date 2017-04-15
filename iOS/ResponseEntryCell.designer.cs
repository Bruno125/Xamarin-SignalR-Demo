// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace XamarinSignalR.iOS
{
    [Register ("ResponseEntryCell")]
    partial class ResponseEntryCell
    {
        [Outlet]
        UIKit.UILabel DelayLabel { get; set; }


        [Outlet]
        UIKit.UILabel SizeLabel { get; set; }


        [Outlet]
        UIKit.UILabel ValueLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DelayLabel != null) {
                DelayLabel.Dispose ();
                DelayLabel = null;
            }

            if (SizeLabel != null) {
                SizeLabel.Dispose ();
                SizeLabel = null;
            }

            if (ValueLabel != null) {
                ValueLabel.Dispose ();
                ValueLabel = null;
            }
        }
    }
}
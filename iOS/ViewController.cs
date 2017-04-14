using System;

using UIKit;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;
using XamarinSignalR;

namespace XamarinSignalR.iOS
{
	public partial class ViewController : UIViewController
	{
		private SignalConnector Connector;

		private MessagesDataSource source = new MessagesDataSource();

		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Connector = new SignalConnector("iOS",(name, message) => InvokeOnMainThread ( () => {
				// manipulate UI controls
				source.Add( string.Format("{0}: {1}",name, message));
				TableView.ReloadData();
			}));
			Connector.JoinChat();

			TableView.Source = source;


			SendButton.TouchDown += (sender, e) =>
			{
				if (string.IsNullOrEmpty(MessageTextField.Text))
					return;

                Connector.Send(MessageTextField.Text);
				MessageTextField.Text = "";
			};

		}

	}
}

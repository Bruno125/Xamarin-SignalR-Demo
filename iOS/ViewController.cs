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

		private MessagesDataSource source;

		public ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			source = new MessagesDataSource(TableView);

			Connector = new SignalConnector((Response response) => InvokeOnMainThread(() =>
			{
				source.Add( response);
				TableView.ReloadData();
			}));

			Connector.JoinChat();


			SendButton.TouchDown += (sender, e) =>
			{
				if (string.IsNullOrEmpty(MessageTextField.Text))
					return;

                Connector.Send(MessageTextField.Text);
				MessageTextField.Text = "";
				MessageTextField.ResignFirstResponder();
			};

			MessageTextField.Delegate = new TextDelegate();
		}

		class TextDelegate : UITextFieldDelegate
		{
			public override bool ShouldReturn(UITextField textField)
			{
				textField.ResignFirstResponder();
				return true;
			}

		}

	}
}

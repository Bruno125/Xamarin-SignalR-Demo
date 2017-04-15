using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;

namespace XamarinSignalR.Droid
{
	[Activity(Label = "SignalRWithXamarin", MainLauncher = true)]
	public class MainActivity : Activity
	{
		private SignalConnector Connector;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);

			var input = FindViewById<EditText>(Resource.Id.ChatInput);
			var messages = FindViewById<ListView>(Resource.Id.ChatMessages);

			var adapter = new ResponseAdapter(this);
			messages.Adapter = adapter;

			Connector = new SignalConnector((Response response) => RunOnUiThread(() =>
			{
				adapter.Add(response);
			}));

			Connector.JoinChat();

			Button button = FindViewById<Button>(Resource.Id.ChatButton);
			button.Click += delegate
			{
				if (string.IsNullOrEmpty(input.Text))
					return;

				Connector.Send(input.Text);
				input.Text = "";
			};
		}

	}
}

using System;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;

namespace XamarinSignalR
{
	public class SignalConnector
	{
		private string Sender;
		private event Action<string,string> OnMessageReceived;

		HubConnection chatConnection;
		IHubProxy SignalRChatHub;

		public SignalConnector(string name, Action<string,string> listener)
		{
			Sender = name;
			OnMessageReceived = listener;

			chatConnection = new HubConnection("http://chatsignalrtst.apphb.com/",true);
			SignalRChatHub = chatConnection.CreateHubProxy("ChatHub");

			SignalRChatHub.On<string, string>("addNewMessageToPage", (Sender,message) =>
			{
				if (OnMessageReceived != null)
					OnMessageReceived(name, message);
			});

		}

		public async virtual void JoinChat()
		{
			try
			{
				await chatConnection.Start();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				// Do some error handling.
			}
		}


		public async void Send(string message)
		{
			if (chatConnection.State == ConnectionState.Connected)
				await SignalRChatHub.Invoke("Send", Sender, message);
		}

	}
}

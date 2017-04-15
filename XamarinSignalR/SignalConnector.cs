using System;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;

namespace XamarinSignalR
{
	public class Response
	{
		public string Size { get; set; }
		public string Delay { get; set; }
		public string Message { get; set; }
	}

	public class SignalConnector
	{
		private event Action<Response> OnMessageReceived;

		HubConnection chatConnection;
		IHubProxy SignalRChatHub;

		public SignalConnector(Action<Response> listener)
		{
			OnMessageReceived = listener;

			chatConnection = new HubConnection("http://chatsignalrtst.apphb.com/",true);
			SignalRChatHub = chatConnection.CreateHubProxy("ChatHub");

			SignalRChatHub.On<string, string>("addNewMessageToPage", (timestamp,message) =>
			{
				if (OnMessageReceived != null)
				{
					var response = new Response() { Size = MeasureSize(message), Delay = MeasureDelay(timestamp), Message = message };
					listener(response);
				}
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
			{
				var timestamp = CreateTimestamp();
				await SignalRChatHub.Invoke("Send", timestamp, message);
			}
		}

		private static readonly string DateFormat = "yyyy-MM-dd hh:mm:ss.ffffff";

		private string CreateTimestamp()
		{
			return DateTime.Now.ToString(DateFormat);

		}

		private string MeasureSize(string value)
		{
			return System.Text.ASCIIEncoding.Unicode.GetByteCount(value).ToString() + " bytes";
		}

		private string MeasureDelay(string timestamp)
		{
			try
			{
				DateTime date = DateTime.ParseExact(timestamp, DateFormat, System.Globalization.CultureInfo.InvariantCulture);
				int delay = (DateTime.Now - date).Milliseconds;
				double delaySeconds = delay * 1.0 / 1000;
				double rounded = Math.Truncate(delaySeconds * 1000) / 1000.0;
				return rounded.ToString() + "segs";
			}
			catch (Exception e)
			{
				return "No se pudo calcular";
			}
		}

		private Response CreateResponse(string delay, string size, string message)
		{
			Response response = new Response();
			response.Delay = delay;
			response.Size = size;
			response.Message = message;
			return response;
		}

	}
}

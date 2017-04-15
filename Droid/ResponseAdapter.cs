using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Views;
using Android.Widget;
using XamarinSignalR;

namespace XamarinSignalR.Droid
{
	public class ResponseAdapter : BaseAdapter
	{
		List<Response> responseList;
		Activity _activity;

		public ResponseAdapter(Activity activity)
		{
			responseList = new List<Response>();
			_activity = activity;
		}

		public override int Count
		{
			get { return responseList.Count; }
		}

		public void Add(Response response)
		{
			responseList.Insert(0, response);
			NotifyDataSetChanged();
		}

		public override Java.Lang.Object GetItem(int position)
		{
		// could wrap a Contact in a Java.Lang.Object
		// to return it here if needed
		return null;
		}

		public override long GetItemId(int position)
		{
		return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate(
				Resource.Layout.ResponseItem, parent, false);

			var response = responseList[position];

			view.FindViewById<TextView>(Resource.Id.TextValue).Text = response.Message;
			view.FindViewById<TextView>(Resource.Id.TextSize).Text = response.Size;
			view.FindViewById<TextView>(Resource.Id.TextDelay).Text = response.Delay;

			return view;
		}

	}


}

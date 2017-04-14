using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

namespace XamarinSignalR.iOS
{
	public class MessagesDataSource : UITableViewSource
	{

		List<string> Messages;
		string CellIdentifier = "TableCell";

		public MessagesDataSource()
		{
			Messages = new List<string>();
		}

		public void Add(string message)
		{
			Messages.Add(message);
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Messages.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
			string item = Messages[indexPath.Row];

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{ 
				cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); 
			}

			cell.TextLabel.Text = item;

			return cell;
		}
		
	}
}

using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using XamarinSignalR;

namespace XamarinSignalR.iOS
{

	public class MessagesDataSource : UITableViewSource
	{

		List<Response> Messages;
		string CellIdentifier = ResponseEntryCell.Key;

		public MessagesDataSource(UITableView tableView)
		{
			Messages = new List<Response>();
			tableView.RegisterNibForCellReuse(ResponseEntryCell.Nib, CellIdentifier);
			tableView.Source = this;
		}

		public void Add(Response message)
		{
			Messages.Insert(0, message);
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Messages.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			ResponseEntryCell cell = (ResponseEntryCell) tableView.DequeueReusableCell(CellIdentifier);
			Response item = Messages[indexPath.Row];

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{
				cell = ResponseEntryCell.Create();
			}

			cell.Update(item);
			return cell;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return UITableView.AutomaticDimension;
		}

		public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
		{
			return 80;
		}

	}
}

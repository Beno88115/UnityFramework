using UnityEngine;
using System.Collections;

namespace Tacticsoft
{
    /// <summary>
    /// An interface for a data source to a TableView
    /// </summary>
    public interface ITableViewCellSource
    {
        void RefreshTableView(TableViewCell cell, int row);
    }
}


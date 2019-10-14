using Tacticsoft;
using LuaInterface;

public class LuaTableView : TableView, ITableViewDataSource
{
    private LuaFunction m_GetNumberOfRowsForTableView;
    private LuaFunction m_GetHeightForRowInTableView;
    private LuaFunction m_GetCellForRowInTableView;

    public void Reload()
    {
        if (this.dataSource == null) {
            this.dataSource = this;
        }
        else {
            this.ReloadData();
        }
    }

    /// <summary>
    /// Get the number of rows that a certain table should display
    /// </summary>
    public int GetNumberOfRowsForTableView(TableView tableView)
    {
        if (m_GetNumberOfRowsForTableView != null) {
            return m_GetNumberOfRowsForTableView.Invoke<TableView, int>(tableView);
        }
        return 0;
    }
    
    /// <summary>
    /// Get the height of a row of a certain cell in the table view
    /// </summary>
    public float GetHeightForRowInTableView(TableView tableView, int row)
    {
        if (m_GetHeightForRowInTableView != null) {
            return m_GetHeightForRowInTableView.Invoke<TableView, int, float>(tableView, row + 1);
        }
        return 0f;
    }

    /// <summary>
    /// Create a cell for a certain row in a table view.
    /// Callers should use tableView.GetReusableCell to cache objects
    /// </summary>
    public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
    {
        if (m_GetCellForRowInTableView != null) {
            return m_GetCellForRowInTableView.Invoke<TableView, int, TableViewCell>(tableView, row + 1);
        }
        return null;
    }

    public void AddEventHandler(string eventName, LuaFunction function)
    {
        if (string.IsNullOrEmpty(eventName)) {
            return;
        }

        if (function == null) {
            return;
        }

        if (eventName.Equals("LTV_GET_NUMBERS_OF_ROW")) {
            if (m_GetNumberOfRowsForTableView != null) {
                m_GetNumberOfRowsForTableView.Dispose();
                m_GetNumberOfRowsForTableView = null;
            }
            m_GetNumberOfRowsForTableView = function;
        }
        else if (eventName.Equals("LTV_GET_HEIGHT_FOR_ROW")) {
            if (m_GetHeightForRowInTableView != null) {
                m_GetHeightForRowInTableView.Dispose();
                m_GetHeightForRowInTableView = null;
            }
            m_GetHeightForRowInTableView = function;
        }
        else if (eventName.Equals("LTV_GET_CELL_FOR_ROWS")) {
            if (m_GetCellForRowInTableView != null) {
                m_GetCellForRowInTableView.Dispose();
                m_GetCellForRowInTableView = null;
            }
            m_GetCellForRowInTableView = function;
        }
    }

    void OnDestroy()
    {
        if (m_GetNumberOfRowsForTableView != null) {
            m_GetNumberOfRowsForTableView.Dispose();
            m_GetNumberOfRowsForTableView = null;
        }
        if (m_GetHeightForRowInTableView != null) {
            m_GetHeightForRowInTableView.Dispose();
            m_GetHeightForRowInTableView = null;
        }
        if (m_GetCellForRowInTableView != null) {
            m_GetCellForRowInTableView.Dispose();
            m_GetCellForRowInTableView = null;
        }
    }
}

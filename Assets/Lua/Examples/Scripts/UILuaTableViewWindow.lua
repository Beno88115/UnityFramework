local UILuaTableViewWindow = Component("UILuaTableViewWindow")

function UILuaTableViewWindow:Awake()
	self.btnClose:AddClick(Helper.Handler(self, UILuaTableViewWindow.OnCloseButtonClicked))

	self.tableView:AddEventHandler("LTV_GET_NUMBERS_OF_ROW", Helper.Handler(self, UILuaTableViewWindow.GetNumberOfRowsForTableView))
	self.tableView:AddEventHandler("LTV_GET_HEIGHT_FOR_ROW", Helper.Handler(self, UILuaTableViewWindow.GetHeightForRowInTableView))
	self.tableView:AddEventHandler("LTV_GET_CELL_FOR_ROWS", Helper.Handler(self, UILuaTableViewWindow.GetCellForRowInTableView))
end

function UILuaTableViewWindow:Start()
	self.datas = {}
	table.insert(self.datas, "xbb")
	table.insert(self.datas, "xbb1")
	table.insert(self.datas, "xbb2")
	table.insert(self.datas, "xbb3")
	table.insert(self.datas, "xbb4")
	table.insert(self.datas, "xbb5")
	table.insert(self.datas, "xbb6")
	table.insert(self.datas, "xbb7")
	table.insert(self.datas, "xbb8")

	self.tableView:Reload()
end

function UILuaTableViewWindow:OnCloseButtonClicked()
	UIManager.Instance:PopWindow(self.SerialId)
end

function UILuaTableViewWindow:GetNumberOfRowsForTableView(tableView)
	return #self.datas
end

function UILuaTableViewWindow:GetHeightForRowInTableView(tableView, row)
	return 80
end

function UILuaTableViewWindow:GetCellForRowInTableView(tableView, row)
	local cell = self.tableView:GetReusableCell(self.tableViewCell.reuseIdentifier)
	if not cell then
		local go = UnityEngine.GameObject.Instantiate(self.tableViewCell)
		cell = go:GetComponent("LuaTableViewCell")
		cell.onOKClick = Helper.Handler(self, UILuaTableViewWindow.OnTableViewCellItemClicked)
	end

	local data = self.datas[row]
	cell:SetID(row)
	cell:SetName(data)

	return cell
end

function UILuaTableViewWindow:OnTableViewCellItemClicked(id)
	print("================item id: " .. id)
end

return UILuaTableViewWindow
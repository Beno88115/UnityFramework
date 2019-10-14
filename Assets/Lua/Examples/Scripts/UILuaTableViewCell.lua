local UITestLuaTableViewCell = Component("UITestLuaTableViewCell")

function UITestLuaTableViewCell:Awake()
	self.onOKClick = nil
	self.btnOK:AddClick(Helper.Handler(self, UITestLuaTableViewCell.OnOKButtonClicked))
end

function UITestLuaTableViewCell:Start()
	print("===============id:" .. self.id)
end

function UITestLuaTableViewCell:SetID(id)
	self.id = id
	self.txtID.text = tostring(self.id)
end

function UITestLuaTableViewCell:SetName(name)
	self.txtName.text = name
end

function UITestLuaTableViewCell:OnOKButtonClicked()
	if self.onOKClick then
		self.onOKClick(self.id)
    end
end

return UITestLuaTableViewCell
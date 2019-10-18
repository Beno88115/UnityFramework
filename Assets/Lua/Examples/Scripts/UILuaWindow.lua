local UILuaWindow = Component("UILuaWindow")

function UILuaWindow:Awake()
	self.btnClose:AddClick(Helper.Handler(self, UILuaWindow.OnCloseButtonClicked))
	self.btnLocalization:AddClick(Helper.Handler(self, UILuaWindow.OnLocalizationButtonClicked))
	self.btnTableView:AddClick(Helper.Handler(self, UILuaWindow.OnTableViewButtonClicked))
	self.btnSetting:AddClick(Helper.Handler(self, UILuaWindow.OnSettingButtonClicked))
end

function UILuaWindow:Start()
	print("==============start: " .. self.name)
	print("==============go:" .. tostring(self.gameObject))
end

function UILuaWindow:OnEnable()
	print("==============onenable")
end

function UILuaWindow:OnDisable()
	print("==============ondisable")
end

function UILuaWindow:OnDestroy()
	print("=========destroy:" .. self.name)
end

function UILuaWindow:OnInit(isNewInstance, userData)
	print("==============oninit")
end

function UILuaWindow:OnOpen(userData)
	print("==============onopen")
end

function UILuaWindow:OnClose(userData)
	print("==============onclose")
end

function UILuaWindow:OnCloseButtonClicked()
	UIManager.Instance:PopWindow(self.SerialId)
end

function UILuaWindow:OnLocalizationButtonClicked()
	local userData = {}
	userData.tip = "TIP22222"
	userData.tip2 = "XBB3333"
	UIManager.Instance:PushWindow("UILuaLocalization", userData)
end

function UILuaWindow:OnTableViewButtonClicked()
	UIManager.Instance:PushWindow("UILuaTableView")
end

function UILuaWindow:OnSettingButtonClicked()
	local str = SettingManager.Instance:GetString("key111", "defaultValue")
	print("===============setting str: " .. str)
	SettingManager.Instance:SetString("key111", "lalala" .. tostring(os.time()))
end

return UILuaWindow
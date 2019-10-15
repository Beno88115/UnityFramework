local UILuaWindow = Component("UILuaWindow")

function UILuaWindow:Awake()
	self.btnClose:AddClick(Helper.Handler(self, UILuaWindow.OnCloseButtonClicked))
	self.btnLocalization:AddClick(Helper.Handler(self, UILuaWindow.OnLocalizationButtonClicked))
	self.btnTableView:AddClick(Helper.Handler(self, UILuaWindow.OnTableViewButtonClicked))
	self.btnNormal:AddClick(Helper.Handler(self, UILuaWindow.OnNormalButtonClicked))
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

function UILuaWindow:OnNormalButtonClicked()
	print("=====================normal")
end

return UILuaWindow
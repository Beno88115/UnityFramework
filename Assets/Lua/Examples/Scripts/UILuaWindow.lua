local LuaComponent = require "LuaComponent"
UILuaWindow = Component("UILuaWindow", LuaComponent)

function UILuaWindow.Extend(cmpt)
	Helper.Extend(cmpt, UILuaWindow)
	Helper.AddEventHandler(cmpt)
end

function UILuaWindow:Awake(ctrls)
	ctrls.btnClose:AddClick(Helper.Handler(self, UILuaWindow.OnCloseButtonClicked))
	ctrls.btnSend:AddClick(Helper.Handler(self, UILuaWindow.OnSendButtonClicked))
	ctrls.btnLua2:AddClick(Helper.Handler(self, UILuaWindow.OnLua2ButtonClicked))
end

function UILuaWindow:Start()
	print("==============start: " .. self.name)
	print("==============go:" .. tostring(self.gameObject))
	self.gameObject:GetLuaComponent("xxxx")
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

function UILuaWindow:OnCloseButtonClicked()
	UIManager.Instance:PopWindow(self.SerialId)
end

function UILuaWindow:OnSendButtonClicked()
	print("=====================send")
end

function UILuaWindow:OnLua2ButtonClicked()
	local userData = {}
	userData.tip = "TIP22222"
	userData.tip2 = "XBB3333"
	UIManager.Instance:PushWindow("UILua2", userData)

	local xxx = GameObject.Instance(prefab)
	-- local c = xxx.GetComponent("LuaBehaviour")
	-- local lc = c.GetLuaCompnent('')
	local lc2 = xxx.GetLuaComponent("")
end
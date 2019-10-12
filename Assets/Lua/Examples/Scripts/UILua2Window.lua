local LuaComponent = require "LuaComponent"
local UILua2Window = Component("UILua2Window", LuaComponent)

function UILua2Window:Awake()
	self.btnClose:AddClick(Helper.Handler(self, UILua2Window.OnCloseButtonClicked))
end

function UILua2Window:Start()
	print("==============start: " .. self.name)
end

function UILua2Window:OnEnable()
	print("==============onenable")
end

function UILua2Window:OnDisable()
	print("==============ondisable")
end

function UILua2Window:OnDestroy()
	print("=========destroy:" .. self.name)
end

function UILua2Window:OnInit(isNewInstance, userData)
	print("==============oninit")
end

function UILua2Window:OnOpen(userData)
	self.txtTip.text = userData.tip
	self.txtTip2.text = userData.tip2
end

function UILua2Window:OnCloseButtonClicked()
	UIManager.Instance:PopWindow(self.SerialId)
end

function UILua2Window:OnSendButtonClicked()
	print("=====================send")
end

return UILua2Window
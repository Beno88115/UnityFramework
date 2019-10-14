local LuaComponent = require "LuaComponent"
local UILua2Window = Component("UILua2Window", LuaComponent)

function UILua2Window:Awake()
	self.btnClose:AddClick(Helper.Handler(self, UILua2Window.OnCloseButtonClicked))
	self.btnShow:AddClick(Helper.Handler(self, UILua2Window.OnShowButtonClicked))
	self.btnLua3:AddClick(Helper.Handler(self, UILua2Window.OnLua3ButtonClicked))
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
	self.imgIcon.sprite = nil
end

function UILua2Window:OnCloseButtonClicked()
	UIManager.Instance:PopWindow(self.SerialId)
end

function UILua2Window:OnShowButtonClicked()
	self.imgIcon:SetSprite("Chess")
end

function UILua2Window:OnLua3ButtonClicked()
	UIManager.Instance:PushWindow("UILuaTableView")
end

return UILua2Window
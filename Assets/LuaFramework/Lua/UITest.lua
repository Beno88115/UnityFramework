local behaviour = require "LuaBehaviour1"
local inspect = require "inspect"

UITest = class("UITest", behaviour)

function UITest.Extend(cmpt)
	extend(cmpt, UITest)
	registerEventHandler(cmpt)
end

function UITest:Awake(widgets)
	widgets.btnTitle:SetText("Title")
	widgets.btnClose:AddClick(handler(self, UITest.OnCloseButtonClicked))
end

-- function UITest:Start()
-- 	self:Sub(10, 30)
-- 	self:Add(10, 300)
-- 	self:OnXXX()
-- end

function UITest:OnEnable()
	print("============onenable")

	self:Sub(10, 30)
	self:Add(10, 3000)
end

function UITest:OnDisable()
	print("==============ondisable")
end

function UITest:OnDestroy()
	print("=========destroy:" .. self.name)
end

function UITest:OnCloseButtonClicked()
	print("=========close: " .. self.name)
end

function UITest:Sub(a, b)
	print("========sub:")
end
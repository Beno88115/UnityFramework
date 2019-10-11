local behaviour1 = require "LuaBehaviour1"
local behaviour4 = require "LuaBehaviour4"
local inspect = require "Core/Inspect"

UITest = Component("UITest", behaviour1)
-- UITest = component("UITest", behaviour1)
-- UITest = component("UITest", behaviour1, behaviour4)

function UITest.Extend(cmpt)
	Helper.Extend(cmpt, UITest)
	Helper.AddEventHandler(cmpt)
end

function UITest:Awake(widgets)
	widgets.btnTitle:SetText("Title")
	widgets.btnClose:AddClick(Helper.Handler(self, UITest.OnCloseButtonClicked))
end

-- function UITest:Start()
-- 	self:Sub(10, 30)
-- 	self:Add(10, 300)
-- 	self:OnXXX()
-- end

function UITest:OnEnable()
	print("============onenable")

	self:Sub(10, 30)
	self:OnXYZ()
	self:OnBattle()
	-- self:OnXXZZZ()
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
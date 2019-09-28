local LuaBehaviour = require "LuaBehaviour"
-- print("===============: " .. LuaBehaviour)

-- local mt = {}
-- mt.__index = LuaBehaviour

UITest = {}
UITest.__index = LuaBehaviour
-- setmetatable(UITest, LuaBehaviour)

-- function UITest.Attach(component, ...)
-- 	local mt = {}
-- 	mt.__index = component
-- 	setmetatable(UITest, mt)
-- 	UITest.Awake(component, component.gameObject, ...)
-- end

function UITest:Awake(gameObject, binders)
	print("=======awake:" .. self.name)

	-- local component = gameObject.transform:GetComponent("LuaBehaviour")
	-- component:AddClick(btn, this.OnCloseButtonClicked)

	-- print("==========binder: " .. binders.button)

	-- binders.button:AddClick(UITest.OnCloseButtonClicked)
	binders.button:AddClick(handler(self, UITest.OnCloseButtonClicked))
end

function UITest:Start()
	print("=========start:" .. self.name)
	self:Add(10, 30)
end

function UITest:OnDestroy()
	print("=========destroy:" .. self.name)
end

function UITest:OnCloseButtonClicked()
	print("=========close: " .. self.name)
end

function handler(obj, method)
    return function(...)
        return method(obj, ...)
    end
end

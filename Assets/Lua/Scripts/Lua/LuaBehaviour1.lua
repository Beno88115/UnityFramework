
local behaviour2 = require "LuaBehaviour2"
local LuaBehaviour1 = Component("LuaBehaviour1", behaviour2)

-- 这个类只是用于测试，并没有存在的意义
-- 提供一个DEMO

-- function LuaBehaviour1:Attach(cmpt)
-- 	-- LuaBehaviour1.__index = LuaBehaviour1
-- 	setmetatable(LuaBehaviour1, { __index = cmpt })

-- 	-- LuaBehaviour1:Add(10, 20)
-- 	-- t.Awake(component, component.gameObject, ...)
-- end

function LuaBehaviour1:Create()
end

function LuaBehaviour1:Awake(widgets)
	print("==========awake")
end

function LuaBehaviour1:Start()
	print("11111===============start: " .. self.name)
end

function LuaBehaviour1:OnDestroy()
end

return LuaBehaviour1
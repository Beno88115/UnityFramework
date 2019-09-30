
local behaviour3 = require "LuaBehaviour3"
local LuaBehaviour2 = component("LuaBehaviour2", behaviour3)

-- 这个类只是用于测试，并没有存在的意义
-- 提供一个DEMO

function LuaBehaviour2:OnXYZ()
	print("=========================xyz")
end

return LuaBehaviour2
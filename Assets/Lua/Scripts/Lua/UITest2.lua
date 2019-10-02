local mt = {
	__index = function(_, key) print("-----------------key: " .. key) end
}

local t = {}
t.__index = t
setmetatable(t, mt)
t.money()
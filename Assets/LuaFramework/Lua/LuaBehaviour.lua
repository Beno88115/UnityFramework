local LuaBehaviour = {}

function LuaBehaviour.Attach(cmpt)
	-- LuaBehaviour.__index = LuaBehaviour
	setmetatable(LuaBehaviour, { __index = cmpt })

	-- LuaBehaviour:Add(10, 20)
	-- t.Awake(component, component.gameObject, ...)
end

function LuaBehaviour:Awake()
	print("============3333")
end

function LuaBehaviour:Start()
end

function LuaBehaviour:OnDestroy()
end

print("=======VVVVVVVV")

return LuaBehaviour
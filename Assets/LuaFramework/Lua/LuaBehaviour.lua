local LuaBehaviour = {}

function LuaBehaviour.Create(component)
	local mt = {}
	mt.__index = component
	setmetatable(LuaBehaviour, mt)
	-- t.Awake(component, component.gameObject, ...)
end

function LuaBehaviour:Awake(gameObject, binders)
end

function LuaBehaviour:Start()
end

function LuaBehaviour:OnDestroy()
end

return LuaBehaviour
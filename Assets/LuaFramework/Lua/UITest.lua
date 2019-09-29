-- local LuaBehaviour = require "LuaBehaviour"
-- print(tostring(LuaBehaviour))
-- -- print("=========lua:" .. type(LuaBehaviour.Start))
-- dump(LuaBehaviour, "vvvv", 10)
-- LuaBehaviour:Awake()

-- UITest = class("UITest", LuaBehaviour)
UITest = {}

function UITest.Attach(cmpt)
	-- LuaBehaviour.__index = LuaBehaviour

	print("====type: " .. type(cmpt))
	-- UITest.__index = UITest
	UITest.__super = cmpt
	setmetatable(UITest, { __index = function(_, key)
		-- print("=========vvv: " .. vv)
		local super = UITest.__super
		if super[key] then
			-- local typeName = type(super)
			-- if typeName == "userdata" then

            -- end
			return super[key]
        end
	end })

	cmpt:Attach(UITest)
	-- LuaBehaviour:Add(10, 20)
	-- t.Awake(component, component.gameObject, ...)
end

function UITest:Awake()
	-- attach(obj, UITest)
	-- print("=======awake:" .. self.name)

	-- local component = gameObject.transform:GetComponent("LuaBehaviour")
	-- component:AddClick(btn, this.OnCloseButtonClicked)

	-- print("==========binder: " .. binders.button)

	-- binders.button:AddClick(UITest.OnCloseButtonClicked)
end

function UITest:Start()
	print("=========start:" .. self.name)

	-- binders.button:AddClick(handler(self, UITest.OnCloseButtonClicked))

	self:Sub(10, 30)
	-- self.Add(UITest.__super, 10, 30)
	self:Add(10, 300)
	-- self.__super:Add(110, 30)
	-- self.cmpt:Add(110, 30)
end

function UITest:OnDestroy()
	print("=========destroy:" .. self.name)
end

function UITest:OnCloseButtonClicked()
	print("=========close: " .. self.name)
end

function UITest:Sub(a, b)
	print("========sub")
end

UITest.Add = function(obj, ...)
	return UITest.__super:Add(...)
end
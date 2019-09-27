UITest = {}
local this = UITest

this.gameObject = nil

function UITest.Awake(gameObject)
	this.gameObject = gameObject
	print("=======awake:" .. gameObject.name)

	local component = gameObject.transform:GetComponent("LuaBehaviour")
	-- component:AddClick(btn, this.OnCloseButtonClicked)
end

function UITest.Start()
	print("=========start:" .. this.gameObject.name)
end

function UITest.OnDestroy()
	print("=========destroy:" .. this.gameObject.name)
end

function UITest.OnCloseButtonClicked()
	print("=========close")
end
UILuaCtrl = {}
local this = UILuaCtrl

this.gameObject = nil

local UILoginCtrl = require "Ctrl/UILoginCtrl"

function UILuaCtrl.Awake(gameObject, btn)
	this.gameObject = gameObject
	print("=======uilua awake:" .. gameObject.name .. ", btn:" .. btn.name)

	local window = gameObject.transform:GetComponent("UILuaWindow")
	window:AddClick(btn, this.OnCloseButtonClicked)
end

function UILuaCtrl.Start()
	print("=========uilua start:" .. this.gameObject.name)
end

function UILuaCtrl.OnCloseButtonClicked()
	print("=========uilua close")
end
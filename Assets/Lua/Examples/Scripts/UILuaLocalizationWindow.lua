local UILuaLocalizationWindow = Component("UILuaLocalizationWindow")

function UILuaLocalizationWindow:Awake()
	self.btnClose:AddClick(Helper.Handler(self, UILuaLocalizationWindow.OnCloseButtonClicked))
	self.btnShow:AddClick(Helper.Handler(self, UILuaLocalizationWindow.OnShowButtonClicked))
	self.btnLoad:AddClick(Helper.Handler(self, UILuaLocalizationWindow.OnLoadButtonClicked))
	self.btnEnglish:AddClick(Helper.Handler(self, UILuaLocalizationWindow.OnEnglishButtonClicked))
	self.btnChinese:AddClick(Helper.Handler(self, UILuaLocalizationWindow.OnChineseButtonClicked))
end

function UILuaLocalizationWindow:Start()
	self.btnEnglish.interactable = false
	self.btnChinese.interactable = false
end

function UILuaLocalizationWindow:OnOpen(userData)
	self.txtTip.text = userData.tip
	self.txtTip2.text = userData.tip2
	self.imgIcon.sprite = nil
end

function UILuaLocalizationWindow:OnCloseButtonClicked()
	UIManager.Instance:PopWindow(self.SerialId)
end

function UILuaLocalizationWindow:OnShowButtonClicked()
	self.imgIcon:SetSprite("Chess")
end

function UILuaLocalizationWindow:OnLoadButtonClicked()
	LocalizationManager.Instance:Load(Helper.Handler(self, UILuaLocalizationWindow.OnLoadLocalizationCompleted), 
		Helper.Handler(self, UILuaLocalizationWindow.OnLoadLocalizationFailure))
    self.btnLoad.interactable = false
end

function UILuaLocalizationWindow:OnEnglishButtonClicked()
	LocalizationManager.Instance.CurrentLanguage = Language.English
	self:UpdateLanguageStatus()
end

function UILuaLocalizationWindow:OnChineseButtonClicked()
	LocalizationManager.Instance.CurrentLanguage = Language.ChineseSimplified
	self:UpdateLanguageStatus()
end

function UILuaLocalizationWindow:OnLoadLocalizationCompleted()
	print("====================success")

	self:UpdateLanguageStatus()

	local str = LocalizationManager.Instance:GetString(2004)
	print(str)

	str = LocalizationManager.Instance:GetString(1003003, "XXXXVVVV")
	print(str)
end

function UILuaLocalizationWindow:OnLoadLocalizationFailure()
	print("====================failure")
	self.btnLocalization.interactable = true
end

function UILuaLocalizationWindow:UpdateLanguageStatus()
	self.btnChinese.interactable = not (Language.ChineseSimplified == LocalizationManager.Instance.CurrentLanguage)
	self.btnEnglish.interactable = not (Language.English == LocalizationManager.Instance.CurrentLanguage)
end

return UILuaLocalizationWindow
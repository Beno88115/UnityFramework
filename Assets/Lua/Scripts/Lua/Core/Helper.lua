Helper = {}

function Helper.Extend(cmpt, cls)
    if type(cmpt) == "userdata" then
		local peer = tolua.getpeer(cmpt)
        if not peer then
			tolua.setpeer(cmpt, cls)
        end
    end
end

function Helper.AddEventHandler(node)
    local function onEvent(event, ...)
        -- Behaviour
        if event == "Awake" then
            if node.Awake then node:Awake(...) end
        elseif event == "Start" then
            if node.Start then node:Start() end
        elseif event == "OnDestroy" then
            if node.OnDestroy then node:OnDestroy() end
        elseif event == "OnEnable" then
            if node.OnEnable then node:OnEnable() end
        elseif event == "OnDisable" then
            if node.OnDisable then node:OnDisable() end

        -- UI
        elseif event == "OnInit" then
            if node.OnInit then node:OnInit(...) end
        elseif event == "OnRecycle" then
            if node.OnRecycle then node:OnRecycle() end
        elseif event == "OnOpen" then
            if node.OnOpen then node:OnOpen(...) end
        elseif event == "OnClose" then
            if node.OnClose then node:OnClose(...) end
        elseif event == "OnPause" then
            if node.OnPause then node:OnPause() end
        elseif event == "OnResume" then
            if node.OnResume then node:OnResume() end
        elseif event == "OnCover" then
            if node.OnCover then node:OnCover() end
        elseif event == "OnReveal" then
            if node.OnReveal then node:OnReveal() end
        elseif event == "OnRefocus" then
            if node.OnRefocus then node:OnRefocus(...) end
        end
    end
    node:AddEventHandler(onEvent)
end

function Helper.Handler(obj, method)
    return function(...)
        return method(obj, ...)
    end
end
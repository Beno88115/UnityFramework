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
        end
    end
    node:RegisterEventHandler(onEvent)
end

function Helper.Handler(obj, method)
    return function(...)
        return method(obj, ...)
    end
end
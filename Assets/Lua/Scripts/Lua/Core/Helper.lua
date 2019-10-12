Helper = {}

function Helper.Extend(cmpt, cls)
    if type(cmpt) == "userdata" then
		local peer = tolua.getpeer(cmpt)
        if not peer then
			tolua.setpeer(cmpt, cls)
        end
    end
end

function Helper.Handler(obj, method)
    return function(...)
        return method(obj, ...)
    end
end
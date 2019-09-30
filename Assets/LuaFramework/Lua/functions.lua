function class(classname, super)
	local cls = { __cname = classname }
    cls.__index = cls

    if super then
        setmetatable(cls, super)

        -- cls.__super = super
        -- setmetatable(cls, { __index = function(_, key) 
        --     print("==========cls: " .. key)
        --     if cls.__super[key] then
        --         return cls.__super[key]
        --     end
        -- end })
    end
    
	return cls
end

function extend(cmpt, cls)
    if type(cmpt) == "userdata" then
		local peer = tolua.getpeer(cmpt)
		if not peer then
			tolua.setpeer(cmpt, cls)
		end
    end
end

function registerEventHandler(node)
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

-- function class(classname, ...)
--     local cls = {__cname = classname}

--     local supers = {...}
--     for _, super in ipairs(supers) do
--         local superType = type(super)
--         assert(superType == "nil" or superType == "table" or superType == "function",
--             string.format("class() - create class \"%s\" with invalid super class type \"%s\"",
--                 classname, superType))

--         if superType == "function" then
--             assert(cls.__create == nil,
--                 string.format("class() - create class \"%s\" with more than one creating function",
--                     classname));
--             -- if super is function, set it to __create
--             cls.__create = super
--         elseif superType == "table" then
--             -- if super[".isclass"] then
--             --     -- super is native class
--             --     assert(cls.__create == nil,
--             --         string.format("class() - create class \"%s\" with more than one creating function or native class",
--             --             classname));
--             --     cls.__create = function() return super:create() end
--             -- else
--                 -- super is pure lua class
--                 cls.__supers = cls.__supers or {}
--                 cls.__supers[#cls.__supers + 1] = super
--                 if not cls.super then
--                     -- set first super pure lua class as class.super
--                     cls.super = super
--                 end
--             -- end
--         else
--             error(string.format("class() - create class \"%s\" with invalid super type",
--                         classname), 0)
--         end
--     end

--     cls.__index = cls
--     if not cls.__supers or #cls.__supers == 1 then
--         setmetatable(cls, {__index = cls.super})
--     else
--         setmetatable(cls, {__index = function(_, key)
--             local supers = cls.__supers
--             for i = 1, #supers do
--                 local super = supers[i]
--                 if super[key] then return super[key] end
--             end
--         end})
--     end

--     -- if not cls.ctor then
--     --     -- add default constructor
--     --     cls.ctor = function() end
--     -- end
--     -- cls.new = function(...)
--     --     local instance
--     --     if cls.__create then
--     --         instance = cls.__create(...)
--     --     else
--     --         instance = {}
--     --     end
--     --     setmetatableindex(instance, cls)
--     --     instance.class = cls
--     --     instance:ctor(...)
--     --     return instance
--     -- end
--     -- cls.create = function(_, ...)
--     --     return cls.new(...)
--     -- end

--     return cls
-- end

function handler(obj, method)
    return function(...)
        return method(obj, ...)
    end
end
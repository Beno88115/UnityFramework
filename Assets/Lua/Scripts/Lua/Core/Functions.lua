-- local inspect = require "inspect"

function Class(classname, ...)
	local cls = { __cname = classname }
    cls.__index = cls

    local supers = { ... }
    for _, super in ipairs(supers) do
        local superType = type(super)
        if superType == "table" then
            cls.__supers = cls.__supers or {}
            cls.__supers[#cls.__supers + 1] = super
            if not cls.super then
                cls.super = super
            end
        end
    end

    if not cls.__supers or #cls.__supers == 1 then
        setmetatable(cls, {__index = cls.super})
        -- setmetatable(cls, cls.super)
    else
        setmetatable(cls, {__index = function(_, key)
            local supers = cls.__supers
            for i = 1, #supers do
                local super = supers[i]
                if super[key] then return super[key] end
            end
        end})

        -- setmetatable(cls, function(_, key)
        --     local supers = cls.__supers
        --     for i = 1, #supers do
        --         local super = supers[i]
        --         if super[key] then return super[key] end
        --     end
        -- end)
    end
    
	return cls
end

function Component(cmptname, ...)
	local cls = { __cname = cmptname }
    cls.__index = cls

    local supers = { ... }
    for _, super in ipairs(supers) do
        local superType = type(super)
        if superType == "table" then
            cls.__supers = cls.__supers or {}
            cls.__supers[#cls.__supers + 1] = super
            if not cls.super then
                cls.super = super
            end
        end
    end

    if not cls.__supers or #cls.__supers == 1 then
        setmetatable(cls, cls.super)
    else
        -- TODO:
        -- setmetatable(cls, { __index = function(_, key)
        --     local supers = cls.__supers
        --     for i = 1, #supers do
        --         local super = supers[i]
        --         if super[key] then return super[key] end
        --     end
        -- end })
    end
    
	return cls
end

LocalizationManager = LuaLocalizationManager
ResourceManager = LuaResourceManager 
SettingManager = LuaSettingManager 
GameObject = UnityEngine.GameObject
Language = GameFramework.Localization.Language
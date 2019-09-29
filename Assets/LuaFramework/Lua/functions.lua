function class(className, superClass)
	local t = { __cname = className }
	t.__index = t
	setmetatable(t, { __index = superClass })
	t.Awake()
	return t
end

function string.ltrim(input)
    return string.gsub(input, "^[ \t\n\r]+", "")
end

function string.rtrim(input)
    return string.gsub(input, "[ \t\n\r]+$", "")
end

function string.trim(input)
    input = string.gsub(input, "^[ \t\n\r]+", "")
    return string.gsub(input, "[ \t\n\r]+$", "")
end

function dump(value, desciption, nesting)
    if type(nesting) ~= "number" then nesting = 3 end

    local lookupTable = {}
    local result = {}

    local traceback = string.split(debug.traceback("", 2), "\n")
    print("dump from: " .. string.trim(traceback[3]))

    local function dump_(value, desciption, indent, nest, keylen)
        desciption = desciption or "<var>"
        local spc = ""
        if type(keylen) == "number" then
            spc = string.rep(" ", keylen - string.len(dump_value_(desciption)))
        end
        if type(value) ~= "table" then
            result[#result +1 ] = string.format("%s%s%s = %s", indent, dump_value_(desciption), spc, dump_value_(value))
        elseif lookupTable[tostring(value)] then
            result[#result +1 ] = string.format("%s%s%s = *REF*", indent, dump_value_(desciption), spc)
        else
            lookupTable[tostring(value)] = true
            if nest > nesting then
                result[#result +1 ] = string.format("%s%s = *MAX NESTING*", indent, dump_value_(desciption))
            else
                result[#result +1 ] = string.format("%s%s = {", indent, dump_value_(desciption))
                local indent2 = indent.."    "
                local keys = {}
                local keylen = 0
                local values = {}
                for k, v in pairs(value) do
                    keys[#keys + 1] = k
                    local vk = dump_value_(k)
                    local vkl = string.len(vk)
                    if vkl > keylen then keylen = vkl end
                    values[k] = v
                end
                table.sort(keys, function(a, b)
                    if type(a) == "number" and type(b) == "number" then
                        return a < b
                    else
                        return tostring(a) < tostring(b)
                    end
                end)
                for i, k in ipairs(keys) do
                    dump_(values[k], k, indent2, nest + 1, keylen)
                end
                result[#result +1] = string.format("%s}", indent)
            end
        end
    end
    dump_(value, desciption, "- ", 1)

    for i, line in ipairs(result) do
        print(line)
    end
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
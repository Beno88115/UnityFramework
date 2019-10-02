--主入口函数。从这里开始lua逻辑
a = "1000"
b = 2033
c = 10.333
t = {}
t.name = "xbb"
t.age = 20

print("xxxx: " .. xxx)

function Main()					
	print("logic start")	 		
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end
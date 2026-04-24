local hmmlib = require('hmm')
local hmm = hmmlib.new()
hmm.DllEncoding = 1 --0 for utf-8, 1 for gbk
hmm:call("create")
local json = require('json')
-- lordstar地图文件
require('rooms')
local allroom = {}

---自定义的exit 的出口转换函数
local function convert_exit(exit)
    return exit
end

for _, roomdata in pairs(rooms) do
    local room = hmmlib.Room.new()
    room.Key = roomdata.id .. ""
    room.Name = roomdata.name
    room.Group = roomdata.area
    if (roomdata.relation ~= "") then
        local data = hmmlib.Data.new()
        data.Key = "relation"
        data.Value = roomdata.relation
        table.insert(room.Data, data)
    end
    if (roomdata.links ~= "") then
        local data = hmmlib.Data.new()
        data.Key = "links"
        data.Value = roomdata.links
        table.insert(room.Data, data)
    end
    if (roomdata.desc ~= "") then
        local data = hmmlib.Data.new()
        data.Key = "desc"
        data.Value = roomdata.desc
        table.insert(room.Data, data)
    end
    if (roomdata.info ~= "") then
        local data = hmmlib.Data.new()
        data.Key = "info"
        data.Value = roomdata.info
        table.insert(room.Data, data)
    end
    if (roomdata.mark ~= "") then
        local data = hmmlib.Data.new()
        data.Key = "mark"
        data.Value = roomdata.mark
        table.insert(room.Data, data)
    end
    if (roomdata.enter ~= "") then
        local data = hmmlib.Data.new()
        data.Key = "enter"
        data.Value = roomdata.enter
        table.insert(room.Data, data)
    end
    allroom[room.Key] = room
end
for _, roomdata in pairs(rooms) do
    for command, target in pairs(roomdata.exits) do
        local exit = hmmlib.Exit.new()
        exit.From = roomdata.id .. ""
        exit.To = target .. ""
        exit.Command = command
        exit = convert_exit(exit)
        if (exit ~= nil) then
            table.insert(allroom[exit.From].Exits, exit)
        end
    end
end
local inputrooms = hmmlib.Rooms.new()
for key, value in pairs(allroom) do
    table.insert(inputrooms.Rooms, value)
end
hmm:call("insertrooms", json.encode(inputrooms))
local output = hmm:call("export")
print(output)
local file = assert(io.open("lordstar.hmm", "w"))
file:write(output)
file:close()

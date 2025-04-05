using System;
using System.Collections.Generic;
using System.IO;
using DungeonMaker.Objects;
using DungeonMaker.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DungeonMaker.Data;

public static class JsonParser
{
    private const String DataPath = "../../../Data/Objects";
    
    public static void LoadAll()
    {
        LoadAllRooms();
        LoadAllMonsters();
    }
    
    private static void LoadAllRooms()
    {
        String path = Path.Combine(DataPath, "Rooms");
        JsonSerializerSettings settings = new()
        {
            Converters = { new StringEnumConverter() }
        };
        
        if (!Directory.Exists(path))
        {
            Logger.Warning($"Room data path not found: {path}");
            return;
        }

        foreach (String file in Directory.GetFiles(path, "*.json"))
        {
            try
            {
                String json = File.ReadAllText(file);
                var rooms = JsonConvert.DeserializeObject<List<DMRoom>>(json, settings) ?? [];
                foreach (DMRoom room in rooms)
                {
                    DMObjectRegistry.Register(room);
                    Logger.Debug($"[Loaded] Room '{room.Name}' from {Path.GetFileName(file)}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to load room file {file}: {ex.Message}");
            }
        }
    }

    private static void LoadAllMonsters()
    {
        String path = Path.Combine(DataPath, "Monsters");
        JsonSerializerSettings settings = new()
        {
            Converters = { new StringEnumConverter() }
        };
        
        if (!Directory.Exists(path))
        {
            Logger.Warning($"Room data path not found: {path}");
            return;
        }

        foreach (String rankDir in Directory.GetDirectories(path))
        {
            foreach (String file in Directory.GetFiles(rankDir, "*.json"))
            {
                try
                {
                    String json = File.ReadAllText(file);
                    var monsters = JsonConvert.DeserializeObject<List<DMMonster>>(json, settings) ?? [];
                    foreach (DMMonster monster in monsters)
                    {
                        DMObjectRegistry.Register(monster);
                        Logger.Debug($"[Loaded] Monster '{monster.Name}' from {Path.GetFileName(file)}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Failed to load monster file {file}: {ex.Message}");
                }
            }
        }
    }
}

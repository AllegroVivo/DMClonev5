using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DungeonMaker.Utilities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMaker.Core;

public static class TextureManager
{
    private const String BaseDir = "../../../Content/";
    private static Dictionary<DMObjectType, Dictionary<String, Dictionary<DMAnimationType, Texture2D[]>>> _textures = new();
    private static Dictionary<String, Texture2D> _singleTextures = new();
    
    internal static void LoadAllTextures()
    {
        LoadSingleTextures();
        LoadRoomTextures();
        LoadMonsterTextures();
    }

    private static void LoadSingleTextures()
    {
        String texturesPath = Path.Combine(BaseDir, "Textures", "Core");
        if (!Directory.Exists(texturesPath))
        {
            Logger.Error($"Room Textures path not found: {texturesPath}");
            return;
        }

        String[] files = Directory.GetFiles(texturesPath, "*.png");
        foreach (String fileName in files)
        {
            String filePath = Path.Combine(texturesPath, fileName);
            String textureName = Path.GetFileNameWithoutExtension(fileName);
            if (_singleTextures.ContainsKey(textureName))
            {
                Logger.Warning($"Texture already loaded: {textureName}");
                continue;
            }

            Texture2D texture = LoadTexture(filePath);
            _singleTextures[textureName] = texture;
            Logger.Debug($"Loaded single texture: {textureName}");
        }
    }
    
    private static void LoadRoomTextures()
    {
        var roomPath = Path.Combine(BaseDir, "Textures", "Objects", "Rooms");
        if (!Directory.Exists(roomPath))
        {
            Logger.Error($"Room Textures path not found: {roomPath}");
            return;
        }

        var roomDict = new Dictionary<String, Dictionary<DMAnimationType, Texture2D[]>>();

        foreach (var roomFolder in Directory.GetDirectories(roomPath))
        {
            String roomName = Path.GetFileName(roomFolder);
            var animDict = new Dictionary<DMAnimationType, Texture2D[]>();

            foreach (var animFolder in Directory.GetDirectories(roomFolder))
            {
                String animName = Path.GetFileName(animFolder);
                if (!Enum.TryParse<DMAnimationType>(animName, true, out var animType))
                {
                    Logger.Warning($"Unknown animation type: {animName}");
                    continue;
                }

                var textures = Directory
                    .GetFiles(animFolder, "*.png")
                    .OrderBy(f => f) // alphabetical order = frame order
                    .Select(LoadTexture)
                    .ToArray();
                
                if(textures.Length == 0)
                {
                    Logger.Warning($"No textures found for {roomName}.{animName}");
                    continue;
                }

                animDict[animType] = textures;
            }

            roomDict[roomName] = animDict;
            Logger.Debug($"Loaded textures for room: {roomName}");
        }

        _textures[DMObjectType.Room] = roomDict;
    }

    private static void LoadMonsterTextures()
    {
        var monsterPath = Path.Combine(BaseDir, "Textures", "Objects", "Monsters");
        if (!Directory.Exists(monsterPath))
        {
            Logger.Error($"Monster Textures path not found: {monsterPath}");
            return;
        }

        var monsterDict = new Dictionary<String, Dictionary<DMAnimationType, Texture2D[]>>();

        foreach (var rankFolder in Directory.GetDirectories(monsterPath))
        {
            foreach (var monsterFolder in Directory.GetDirectories(rankFolder))
            {
                String monsterName = Path.GetFileName(monsterFolder);
                var animDict = new Dictionary<DMAnimationType, Texture2D[]>();

                foreach (var animFolder in Directory.GetDirectories(monsterFolder))
                {
                    String animName = Path.GetFileName(animFolder);
                    if (!Enum.TryParse<DMAnimationType>(animName, true, out var animType))
                    {
                        Logger.Warning($"Unknown animation type: {animName}");
                        continue;
                    }

                    var textures = Directory
                        .GetFiles(animFolder, "*.png")
                        .OrderBy(f => f) // alphabetical order = frame order
                        .Select(LoadTexture)
                        .ToArray();
                
                    if(textures.Length == 0)
                    {
                        Logger.Warning($"No textures found for {monsterName}.{animName}");
                        continue;
                    }

                    animDict[animType] = textures;
                }

                monsterDict[monsterName] = animDict;
                Logger.Debug($"Loaded textures for monster: {monsterName}");
            }
        }

        _textures[DMObjectType.Monster] = monsterDict;
    }
    
    private static Texture2D LoadTexture(String filePath)
    {
        using var stream = File.OpenRead(filePath);
        return Texture2D.FromStream(GameContext.GraphicsDevice, stream);
    }
    
    public static Texture2D GetSingleTexture(String name)
    {
        if (_singleTextures.TryGetValue(name, out Texture2D? texture))
            return texture;

        throw new KeyNotFoundException($"Texture not found: {name}");
    }
    
    public static Texture2D[] GetAnimation(DMObjectType type, String name, DMAnimationType animType)
    {
        if (_textures.TryGetValue(type, out var nameDict)
            && nameDict.TryGetValue(name, out var animDict)
            && animDict.TryGetValue(animType, out var frames))
        {
            return frames;
        }

        throw new KeyNotFoundException($"Texture not found for {type}.{name}.{animType}");
    }
}
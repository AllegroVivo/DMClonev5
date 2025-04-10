using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DungeonMaker.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace DungeonMaker.Core;

public static class TextureManager
{
    private const String BaseDir = "../../../Content/";

    private static readonly Dictionary<DMObjectType, Dictionary<String, Dictionary<DMAnimationType, SpriteFrame[]>>> _textures = new();
    private static readonly Dictionary<String, Texture2D> _singleTextures = new();

    internal static void LoadAllTextures()
    {
        LoadSingleTextures();
        LoadTextures(DMObjectType.Room, "Rooms");
        LoadTextures(DMObjectType.Monster, "Monsters");
        LoadTextures(DMObjectType.Hero, "Heroes");
    }

    #region Single Textures

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

    public static Texture2D GetSingleTexture(String name)
    {
        if (_singleTextures.TryGetValue(name, out Texture2D? texture))
            return texture;

        throw new KeyNotFoundException($"Texture not found: {name}");
    }

    #endregion

    #region Object Textures (Unified)

    private static void LoadTextures(DMObjectType type, String folderName)
    {
        String basePath = Path.Combine(BaseDir, "Textures", "Objects", folderName);
        if (!Directory.Exists(basePath))
        {
            Logger.Error($"{type} textures path not found: {basePath}");
            return;
        }

        var typeDict = new Dictionary<String, Dictionary<DMAnimationType, SpriteFrame[]>>();

        var objectFolders = type is DMObjectType.Monster or DMObjectType.Hero
            ? Directory.GetDirectories(basePath).SelectMany(Directory.GetDirectories)
            : Directory.GetDirectories(basePath);

        foreach (var objFolder in objectFolders)
        {
            String objName = Path.GetFileName(objFolder);
            var animDict = new Dictionary<DMAnimationType, SpriteFrame[]>();

            foreach (var animFolder in Directory.GetDirectories(objFolder))
            {
                String animName = Path.GetFileName(animFolder);
                if (!Enum.TryParse(animName, true, out DMAnimationType animType))
                {
                    Logger.Warning($"Unknown animation type: {animName}");
                    continue;
                }

                var frames = Directory
                    .GetFiles(animFolder, "*.png")
                    .OrderBy(f => f)
                    .Select((file, index) => new SpriteFrame
                    {
                        Texture = LoadTexture(file),
                        Name = Path.GetFileNameWithoutExtension(file),
                        Order = index
                    })
                    .ToArray();

                if (frames.Length == 0)
                {
                    Logger.Warning($"No textures found for {objName}.{animName}");
                    continue;
                }

                animDict[animType] = frames;
            }

            typeDict[objName] = animDict;
            Logger.Debug($"Loaded textures for {type}: {objName}");
        }

        _textures[type] = typeDict;
    }

    public static SpriteFrame[] GetAnimation(DMObjectType type, String name, DMAnimationType animType)
    {
        if (_textures.TryGetValue(type, out var nameDict)
            && nameDict.TryGetValue(name, out var animDict)
            && animDict.TryGetValue(animType, out var frames))
        {
            return frames;
        }

        throw new KeyNotFoundException($"Texture not found for {type}.{name}.{animType}");
    }

    #endregion

    #region Helpers

    private static Texture2D LoadTexture(String filePath)
    {
        using var stream = File.OpenRead(filePath);
        return Texture2D.FromStream(GameContext.GraphicsDevice, stream);
    }

    #endregion
}

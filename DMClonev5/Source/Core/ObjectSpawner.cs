using System;
using System.Collections.Generic;
using DungeonMaker.Components;
using DungeonMaker.Entities;
using DungeonMaker.Objects;

namespace DungeonMaker.Core;

public static class ObjectSpawner
{
    private static readonly Dictionary<DMObjectType, Func<DMObject, GameObject>> _handlers = new();

    public static void Register(DMObjectType type, Func<DMObject, GameObject> spawner)
    {
        _handlers[type] = spawner;
    }

    public static GameObject Spawn(DMObjectType type, String name)
    {
        if (!DMObjectRegistry.TryGet(type, name, out DMObject? obj))
            throw new Exception($"No DMObject registered with type {type} and name '{name}'.");

        if (!_handlers.TryGetValue(type, out var handler))
            throw new Exception($"No ObjectSpawner registered for type: {type}");

        return handler(obj!);
    }

    internal static void InitSpawners()
    {
        EntityManager em = GameContext.EntityManager;
        
        Register(DMObjectType.Room, obj =>
        {
            if (obj is not DMRoom room)
                throw new Exception("Invalid DMObject type for Room spawner!");

            GameObject go = em.CreateGameObject();

            go.AddComponent(new DMObjectReference(room));
            go.AddComponent(new BattleRoomComponent { Effects = room.Effects });

            var texture = TextureManager.GetAnimation(DMObjectType.Room, room.Name, DMAnimationType.Static);
            go.AddComponent(new SpriteComponent { Texture = texture[0] });
            
            return go;
        });
        Register(DMObjectType.Monster, obj =>
        {
            if (obj is not DMMonster monster)
                throw new Exception("Invalid DMObject type for Monster spawner!");
            
            GameObject go = em.CreateGameObject();
            
            go.AddComponent(new DMObjectReference(monster));
            go.AddComponent(new StatsComponent(monster.Stats.Life, monster.Stats.Attack, monster.Stats.Defense));
            go.AddComponent(new LevelComponent());
            
            var texture = TextureManager.GetAnimation(DMObjectType.Monster, monster.Name, DMAnimationType.Idle);
            go.AddComponent(new SpriteComponent { Texture = texture[0] });

            return go;
        });
    }
}
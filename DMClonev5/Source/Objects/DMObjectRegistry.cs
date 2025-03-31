using System;
using System.Collections.Generic;

namespace DungeonMaker.Objects;

public static class DMObjectRegistry
{
    private static readonly Dictionary<DMObjectType, Dictionary<String, DMObject>> _registry = new();

    public static void Register(DMObject obj)
    {
        if (!_registry.TryGetValue(obj.Type, out var map))
            _registry[obj.Type] = map = new Dictionary<String, DMObject>();

        map[obj.Name] = obj;
    }

    public static T? Get<T>(DMObjectType type, String name) where T : DMObject
    {
        return _registry[type][name] as T;
    }

    public static Boolean TryGet(DMObjectType type, String id, out DMObject? obj)
    {
        obj = null;
        if (_registry.TryGetValue(type, out var map) && map.TryGetValue(id, out DMObject? found))
        {
            obj = found;
            return true;
        }
        return false;
    }
}
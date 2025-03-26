﻿using System;
using System.Collections.Generic;

namespace DungeonMaker.Entities;

public class EntityManager
{
    private Int32 _nextEntityId = 1;
    private readonly Dictionary<Int32, Dictionary<Type, IComponent>> _entities = new();

    public Entity CreateEntity()
    {
        Int32 id = _nextEntityId++;
        _entities[id] = new Dictionary<Type, IComponent>();
        return new Entity(id);
    }

    public void AddComponent<T>(Entity entity, T component) where T : IComponent
    {
        _entities[entity.ID][typeof(T)] = component;
    }
    
    public T GetComponent<T>(Entity entity) where T : IComponent
    {
        return (T)_entities[entity.ID][typeof(T)];
    }
    
    public Boolean HasComponent<T>(Entity entity) where T : IComponent
    {
        return _entities[entity.ID].ContainsKey(typeof(T));
    }
    
    public IEnumerable<Entity> GetAllEntitiesWith<T>() where T : IComponent
    {
        foreach (var (entityId, componentDict) in _entities)
        {
            if (componentDict.ContainsKey(typeof(T)))
                yield return new Entity(entityId);
        }
    }
    
    public IEnumerable<Entity> GetAllEntitiesWith<T1, T2>() where T1 : IComponent where T2 : IComponent
    {
        foreach (var (entityId, componentDict) in _entities)
        {
            if (componentDict.ContainsKey(typeof(T1)) && componentDict.ContainsKey(typeof(T2)))
                yield return new Entity(entityId);
        }
    }
}
using System;
using System.Runtime.InteropServices;
using DungeonMaker.Entities;

namespace DungeonMaker.Core;

public class GameObject(Entity entity)
{
    private static EntityManager _entityManager => GameContext.EntityManager;
    public Entity Entity { get; } = entity;
    
    public void AddComponent<T>(T component) where T : IComponent => _entityManager.AddComponent(Entity, component);
    public T GetComponent<T>() where T : IComponent => _entityManager.GetComponent<T>(Entity);
    public void RemoveComponent<T>() where T : IComponent => _entityManager.RemoveComponent<T>(Entity);
    public Boolean HasComponent<T>() where T : IComponent => _entityManager.HasComponent<T>(Entity);
}
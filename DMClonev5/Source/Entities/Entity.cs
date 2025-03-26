using System;

namespace DungeonMaker.Entities;

public class Entity(Int32 id)
{
    public Int32 ID { get; } = id;

    public void AddComponent<T>(T component) where T : IComponent => GameContext.EntityManager.AddComponent(this, component);
    public T GetComponent<T>() where T : IComponent => GameContext.EntityManager.GetComponent<T>(this);
    public Boolean HasComponent<T>() where T : IComponent => GameContext.EntityManager.HasComponent<T>(this);
}
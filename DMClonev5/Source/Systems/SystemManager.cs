using System;
using System.Collections.Generic;
using DungeonMaker.Entities;

namespace DungeonMaker.Systems;

public class SystemManager
{
    private readonly List<SystemBase> _systems = [];
    
    public static EntityManager EntityManager => GameContext.EntityManager;
    
    public void AddSystem(SystemBase system) => _systems.Add(system);

    public void Update()
    {
        foreach (SystemBase system in _systems)
            system.Update();
    }
}
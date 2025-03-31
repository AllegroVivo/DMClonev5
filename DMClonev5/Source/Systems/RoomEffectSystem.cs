using System;
using System.Collections.Generic;
using DungeonMaker.Events;

namespace DungeonMaker.Systems;

public class RoomEffectSystem : SystemBase
{
    private readonly Queue<MonsterDeployedEvent> _eventQueue = new();
    
    public override void Update()
    {
        while (_eventQueue.TryDequeue(out MonsterDeployedEvent? @event))
        {
            
        }
    }
}
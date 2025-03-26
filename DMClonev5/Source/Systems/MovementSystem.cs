using System;
using DungeonMaker.Components;
using DungeonMaker.Entities;

namespace DungeonMaker.Systems;

public class MovementSystem : SystemBase
{
    public override void Update()
    {
        Single dt = (Single)GameContext.GameTime.ElapsedGameTime.TotalSeconds;

        foreach (Entity entity in GameContext.EntityManager.GetAllEntitiesWith<Transform, Velocity>())
        {
            Transform transform = entity.GetComponent<Transform>();
            Velocity velocity = entity.GetComponent<Velocity>();
            
            transform.Position += velocity.Speed * dt;
        }
    }
}
using System;
using DungeonMaker.Components;
using DungeonMaker.Entities;

namespace DungeonMaker.Systems;

public class MovementSystem : SystemBase
{
    public override void Update()
    {
        Single dt = (Single)GameContext.GameTime.ElapsedGameTime.TotalSeconds;

        foreach (Entity entity in GameContext.EntityManager.GetAllEntitiesWith<TransformComponent, VelocityComponent>())
        {
            TransformComponent transform = entity.GetComponent<TransformComponent>();
            VelocityComponent velocity = entity.GetComponent<VelocityComponent>();
            
            transform.Position += velocity.Speed * dt;
        }
    }
}
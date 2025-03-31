using System;
using DungeonMaker.Components;
using DungeonMaker.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMaker.Systems;

public class RenderSystem : SystemBase
{
    private static SpriteBatch MainSpriteBatch => GameContext.MainSpriteBatch;
    private static EntityManager EntityManager => GameContext.EntityManager;
    
    public override void Update()
    {
        MainSpriteBatch.Begin();

        foreach (Entity entity in EntityManager.GetAllEntitiesWith<TransformComponent, SpriteComponent>())
        {
            TransformComponent transform = entity.GetComponent<TransformComponent>();
            SpriteComponent sprite = entity.GetComponent<SpriteComponent>();
            
            // MainSpriteBatch.Draw(
            //     sprite.Texture,
            //     new Rectangle(transform.Position.X, transform.Position.Y, sprite.SourceRectangle?.Width ?? sprite.Texture.Width, sprite.SourceRectangle?.Height ?? sprite.Texture.Height),
            //     sprite.SourceRectangle,
            //     sprite.Color,
            //     0f,
            //     Vector2.Zero,
            //     transform.Scale,
            //     0f
            // );
        }
        
        MainSpriteBatch.End();
    }
}
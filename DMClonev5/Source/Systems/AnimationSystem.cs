using System;
using DungeonMaker.Components;
using DungeonMaker.Entities;

namespace DungeonMaker.Systems;

public class AnimationSystem : SystemBase
{
    private static EntityManager EntityManager => GameContext.EntityManager;

    public override void Update()
    {
        Single deltaTime = (Single)GameContext.GameTime.ElapsedGameTime.TotalSeconds;
        var animations = EntityManager.GetAllComponentsOfType<AnimationComponent>();

            foreach (var animation in animations)
        {
            if (animation.Frames.Length == 0)
                continue;

            animation.TimeElapsed += deltaTime;

            if (animation.TimeElapsed >= animation.FrameTime)
            {
                animation.TimeElapsed -= animation.FrameTime;

                if (animation.Loop)
                {
                    animation.CurrentFrame = (animation.CurrentFrame + 1) % animation.Frames.Length;
                }
                else
                {
                    if (animation.CurrentFrame < animation.Frames.Length - 1)
                        animation.CurrentFrame++;
                    // else: reached end, stay at last frame
                }
            }
        }
    }
}
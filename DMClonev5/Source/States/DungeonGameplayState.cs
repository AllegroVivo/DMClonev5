using System;
using DungeonMaker.Dungeon;
using DungeonMaker.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DungeonMaker.States;

public class DungeonGameplayState : IGameState
{
    public static DungeonGrid Dungeon => GameContext.Dungeon;
    public DungeonRenderer Renderer { get; } = new();
    public DungeonInputSystem InputSystem { get; } = new();
    
    public void Enter()
    {
        Logger.Info($"Entering {nameof(DungeonGameplayState)}");
    }

    public void Exit()
    {
        Logger.Info($"Exiting {nameof(DungeonGameplayState)}");
    }

    public void Update()
    {
        if (GameContext.InputManager.IsKeyPressed(Keys.Insert))
        {
            GameContext.Dungeon.DeployRoom(new Point(1, 1), "Arena");
            GameContext.Dungeon.DeployRoom(new Point(1, 2), "Arena");
            GameContext.Dungeon.DeployRoom(new Point(1, 3), "Arena");
            GameContext.Dungeon.DeployRoom(new Point(2, 1), "Arena");
            GameContext.Dungeon.DeployRoom(new Point(2, 2), "Arena");
            GameContext.Dungeon.DeployRoom(new Point(2, 3), "Arena");
            GameContext.Dungeon.DeployRoom(new Point(3, 1), "Arena");
            GameContext.Dungeon.DeployRoom(new Point(3, 2), "Arena");
            GameContext.Dungeon.DeployRoom(new Point(3, 3), "Arena");
        }
        else if (GameContext.InputManager.IsKeyPressed(Keys.PageUp))
        {
            GameContext.Dungeon.ExpandHeight(true);
        }
        else if (GameContext.InputManager.IsKeyPressed(Keys.PageDown))
        {
            GameContext.Dungeon.ExpandHeight(false);
        }
        else if (GameContext.InputManager.IsKeyPressed(Keys.Home))
        {
            GameContext.Dungeon.ExpandWidth();
        }
        
        InputSystem.Update();
    }

    public void Draw()
    {
        Renderer.Redraw();
    }
}
using System;
using DungeonMaker.Core;
using DungeonMaker.Dungeon;
using DungeonMaker.Entities;
using DungeonMaker.Input;
using DungeonMaker.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMaker;

public static class GameContext
{
    // Numeric Constants
    public const Int32 ScreenWidth = 1440;
    public const Int32 ScreenHeight = 810;
    public const Int32 TileSize = 128;
    public const Int32 BossTileSize = 200;
    public const Int32 TilePadding = 20;
    public const Int32 DungeonPaddingX = 140;
    public const Int32 DungeonPaddingY = 20;
    
    // Sprite Batches
    public static SpriteBatch MainSpriteBatch;
    public static SpriteBatch SecondarySpriteBatch;

    // Global Components
    public static GraphicsDevice GraphicsDevice;
    public static ContentManager Content;
    public static GameTime GameTime;
    public static InputManager InputManager;
    public static EntityManager EntityManager;
    public static SystemManager SystemManager;
    public static DungeonGrid Dungeon;
    public static StateMachine StateMachine;
    public static Random Random;
}
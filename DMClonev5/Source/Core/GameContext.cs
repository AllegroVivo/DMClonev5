using System;
using DungeonMaker.Core;
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

    // Global Components
    public static GraphicsDevice GraphicsDevice;
    public static ContentManager Content;
    public static GameTime GameTime;
    public static SpriteBatch MainSpriteBatch;
    public static InputManager InputManager;
    public static EntityManager EntityManager;
    public static SystemManager SystemManager;
}
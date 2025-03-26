using System;
using DungeonMaker.Entities;
using DungeonMaker.Input;
using DungeonMaker.Systems;
using DungeonMaker.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DungeonMaker;

public class DMGame : Game
{
    private static DMGame _instance;
    public static DMGame Instance
    {
        get
        {
            if (_instance == null)
                throw new Exception("DMGame instance has not been initialized!");
            return _instance;
        }
    }
    
    private Boolean _initialized;
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public DMGame()
    {
        if (_instance != null)
            throw new Exception("DMGame already exists! Use DMGame.Instance instead.");
        _instance = this;
        
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Logger.Initialize();
        
        InitializeGameContext();
        InitializeSystems();

        _graphics.PreferredBackBufferWidth = GameContext.ScreenWidth;
        _graphics.PreferredBackBufferHeight = GameContext.ScreenHeight;
        _graphics.ApplyChanges();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        GameContext.MainSpriteBatch = _spriteBatch;

        _initialized = true;
    }

    protected override void Update(GameTime gameTime)
    {
        if (!_initialized)
            return;
        
        GameContext.GameTime = gameTime;
        GameContext.InputManager.Update();
        
        if (GameContext.InputManager.IsKeyPressed(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (!_initialized)
            return;
        
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

    private void InitializeGameContext()
    {
        GameContext.GraphicsDevice = GraphicsDevice;
        GameContext.Content = Content;

        GameContext.EntityManager = new EntityManager();
        GameContext.InputManager = new InputManager();
        GameContext.SystemManager = new SystemManager();
    }
    
    private static void InitializeSystems()
    {
        GameContext.SystemManager.AddSystem(new MovementSystem());
    }
}

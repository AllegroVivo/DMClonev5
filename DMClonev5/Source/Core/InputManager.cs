using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DungeonMaker.Input;

public class InputManager
{
    private KeyboardState _currentKeyboardState;
    private KeyboardState _previousKeyboardState;
    
    private MouseState _currentMouseState;
    private MouseState _previousMouseState;

    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();
        
        _previousMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();
    }
    
    // === Keyboard ===
    public Boolean IsKeyPressed(Keys key) 
        => _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);

    public Boolean IsKeyReleased(Keys key) 
        => _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
    
    public Boolean IsKeyHeld(Keys key) 
        => _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyDown(key);
    
    // === Mouse ===
    public Boolean IsLeftClick() 
        => _currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released;

    public Boolean IsRightClick() 
        => _currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released;

    public Boolean IsLeftHeld() 
        => _currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed;

    public Boolean IsRightHeld() 
        => _currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Pressed;

    public Point MousePosition => _currentMouseState.Position;
}
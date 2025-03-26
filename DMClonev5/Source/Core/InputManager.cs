using System;
using Microsoft.Xna.Framework.Input;

namespace DungeonMaker.Input;

public class InputManager
{
    private KeyboardState _currentKeyboardState;
    private KeyboardState _previousKeyboardState;

    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();
    }
    
    public Boolean IsKeyPressed(Keys key) 
        => _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);

    public Boolean IsKeyReleased(Keys key) 
        => _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
    
    public Boolean IsKeyHeld(Keys key) 
        => _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyDown(key);
}
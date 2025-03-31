using System;
using System.Collections.Generic;

namespace DungeonMaker.Core;

public class StateMachine
{
    private readonly Stack<IGameState> _states = new();
    
    public void PushState(IGameState state)
    {
        _states.Peek().Exit();
        _states.Push(state);
        state.Enter();
    }

    public void PopState()
    {
        if (_states.Count > 0)
        {
            _states.Pop().Exit();
            if (_states.TryPeek(out var newState))
                newState.Enter();
        }
    }
    
    public void ChangeState(IGameState state)
    {
        while (_states.Count > 0)
            _states.Pop().Exit();

        _states.Push(state);
        state.Enter();
    }

    public void Update()
    {
        if (_states.TryPeek(out var state))
            state.Update();
    }

    public void Draw()
    {
        if (_states.TryPeek(out var state))
            state.Draw();
    }
}
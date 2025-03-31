using System;

namespace DungeonMaker;

public interface IGameState
{
    void Enter();
    void Exit();
    void Update();
    void Draw();
}
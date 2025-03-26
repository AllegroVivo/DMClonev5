using System;
using System.Collections.Generic;

namespace DungeonMaker.Core;

public class EventBus
{
    public delegate void EventHandler<in T>(T e) where T : IGameEvent;
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();
    
    public void Subscribe<T>(EventHandler<T> handler) where T : IGameEvent
    {
        Type type = typeof(T);
        if (!_subscribers.TryGetValue(type, out var delegates)) 
        {
            delegates = [];
            _subscribers[type] = delegates;
        }

        delegates.Add(handler);
    }
    
    public void Unsubscribe<T>(EventHandler<T> handler) where T : IGameEvent 
    {
        Type type = typeof(T);
        if (_subscribers.TryGetValue(type, out var list))
        {
            list.Remove(handler);
            if (list.Count == 0)
                _subscribers.Remove(type);
        }
    }
    
    public void Publish<T>(T eventData) where T : IGameEvent 
    {
        Type type = typeof(T);
        if (_subscribers.TryGetValue(type, out var list))
        {
            foreach (Delegate handler in list)
                ((EventHandler<T>)handler)?.Invoke(eventData);
        }
    }
}
using System;
using System.Collections.Generic;

namespace DungeonMaker.Core;

public static class EventBus
{
    public delegate void EventHandler<in T>(T e) where T : IGameEvent;
    private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();
    
    public static void Subscribe<T>(EventHandler<T> handler) where T : IGameEvent
    {
        Type type = typeof(T);
        if (!_subscribers.TryGetValue(type, out var delegates)) 
        {
            delegates = [];
            _subscribers[type] = delegates;
        }

        if (!delegates.Contains(handler))
            delegates.Add(handler);
    }
    
    public static void Unsubscribe<T>(EventHandler<T> handler) where T : IGameEvent 
    {
        Type type = typeof(T);
        if (_subscribers.TryGetValue(type, out var list))
        {
            list.Remove(handler);
            if (list.Count == 0)
                _subscribers.Remove(type);
        }
    }
    
    public static void Publish<T>(T eventData) where T : IGameEvent 
    {
        Type type = typeof(T);
        if (_subscribers.TryGetValue(type, out var list))
        {
            var handlers = list.ToArray(); // Copy to avoid modification during iteration
            foreach (var handler in handlers)
                ((EventHandler<T>)handler)?.Invoke(eventData);
        }
    }
    
    public static void Clear() => _subscribers.Clear();
}
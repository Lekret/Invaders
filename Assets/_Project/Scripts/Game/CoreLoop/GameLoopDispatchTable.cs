using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Game.Core
{
    public class GameLoopDispatchTable<T>
    {
        private readonly Dictionary<Type, List<T>> _itemsMap = new();
        private readonly List<(Type, List<T>)> _orderedItems = new();

        public List<(Type, List<T>)> OrderedItems => _orderedItems;

        public void AddItem(T item)
        {
            if (!_itemsMap.TryGetValue(item.GetType(), out var items))
            {
                Debug.LogWarning($"[GameLoopItem<{typeof(T).Name}>] Type is not registered: {item.GetType()}");
                InternalAddToDispatchOrder<T>(out items);
            }
                
            items.Add(item);
        }

        public void RemoveItem(T item)
        {
            if (!_itemsMap.TryGetValue(item.GetType(), out var items))
            {
                Debug.LogWarning($"[GameLoopDispatchTable<{typeof(T).Name}>] Type is not registered: {item.GetType()}");
                return;
            }
                
            items.Remove(item);
        }
        
        public void AddToDispatchOrder<TImpl>() where TImpl : T
        {
            InternalAddToDispatchOrder<TImpl>(out _);
        }
            
        private void InternalAddToDispatchOrder<TImpl>(out List<T> items) where TImpl : T
        {
            if (_itemsMap.TryGetValue(typeof(TImpl), out var existingItems))
            {
                Debug.LogError($"[GameLoopDispatchTable<{typeof(T).Name}>] Type is already registered: {typeof(TImpl)}");
                items = existingItems;
                return;
            }

            items = new List<T>();
            _itemsMap.Add(typeof(TImpl), items);
            _orderedItems.Add((typeof(TImpl), items));
        }
        
#if UNITY_EDITOR
        public void ValidateMissingDispatchTypes()
        {
            foreach (var derivedType in UnityEditor.TypeCache.GetTypesDerivedFrom<T>())
            {
                if (_itemsMap.ContainsKey(derivedType))
                    continue;

                Debug.LogWarning($"[GameLoopDispatchTable<{typeof(T).Name}>] Type is not registered for dispatch: {derivedType}");
            }
        }
#endif
    }
}
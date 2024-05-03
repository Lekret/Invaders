using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Game.Core
{
    public class GameLoop
    {
        private class InnerGameLoop<T>
        {
            private readonly Dictionary<Type, List<T>> _itemsMap = new();
            private readonly List<(Type, List<T>)> _orderedItems = new();

            public List<(Type, List<T>)> OrderedItems => _orderedItems;

            public void AddToDispatchOrder<TItem>() where TItem : T
            {
                if (_itemsMap.ContainsKey(typeof(TItem)))
                {
                    Debug.LogError($"[GameLoopItem<{typeof(T).Name}>] Type is already registered: {typeof(TItem)}");
                    return;
                }

                var items = new List<T>();
                _itemsMap.Add(typeof(TItem), items);
                _orderedItems.Add((typeof(TItem), items));
            }

            public void AddItem(T item)
            {
                if (_itemsMap.TryGetValue(item.GetType(), out var items))
                {
                    items.Add(item);
                }
                else
                {
                    Debug.LogError($"[GameLoopItem<{typeof(T).Name}>] Type is not registered: {item.GetType()}");
                }
            }

            public void RemoveItem(T item)
            {
                if (_itemsMap.TryGetValue(item.GetType(), out var items))
                {
                    items.Add(item);
                }
                else
                {
                    Debug.LogError($"[GameLoopItem<{typeof(T).Name}>] Type is not registered: {item.GetType()}");
                }
            }

#if UNITY_EDITOR
            public void ValidateMissingDispatchTypes()
            {
                foreach (var derivedType in UnityEditor.TypeCache.GetTypesDerivedFrom<T>())
                {
                    if (_itemsMap.ContainsKey(derivedType) ||
                        derivedType.Namespace == null ||
                        !derivedType.Namespace.Contains("Scripts/Game"))
                        continue;

                    Debug.LogError($"[GameLoopItem<{typeof(T).Name}>] Type is not registered for dispatch: {derivedType}");
                }
            }
#endif
        }

        private readonly InnerGameLoop<IAwakeable> _awakeLoop = new();
        private readonly InnerGameLoop<IStartable> _startLoop = new();
        private readonly InnerGameLoop<IUpdatable> _updateLoop = new();
        private readonly InnerGameLoop<IFixedUpdatable> _fixedUpdateLoop = new();
        private readonly InnerGameLoop<IDisposable> _disposeLoop = new();

#if UNITY_EDITOR
        public void EditorValidateMissingDispatchTypes()
        {
            _awakeLoop.ValidateMissingDispatchTypes();
            _startLoop.ValidateMissingDispatchTypes();
            _updateLoop.ValidateMissingDispatchTypes();
            _fixedUpdateLoop.ValidateMissingDispatchTypes();
            _disposeLoop.ValidateMissingDispatchTypes();
        }
#endif

        public GameLoop ThenAwake<T>() where T : IAwakeable
        {
            _awakeLoop.AddToDispatchOrder<T>();
            return this;
        }

        public GameLoop ThenStart<T>() where T : IStartable
        {
            _startLoop.AddToDispatchOrder<T>();
            return this;
        }

        public GameLoop ThenUpdate<T>() where T : IUpdatable
        {
            _updateLoop.AddToDispatchOrder<T>();
            return this;
        }

        public GameLoop ThenFixedUpdate<T>() where T : IFixedUpdatable
        {
            _fixedUpdateLoop.AddToDispatchOrder<T>();
            return this;
        }

        public GameLoop ThenDispose<T>() where T : IDisposable
        {
            _disposeLoop.AddToDispatchOrder<T>();
            return this;
        }

        public void AddItem<T>(T item)
        {
            if (item is IAwakeable awakeable)
                _awakeLoop.AddItem(awakeable);

            if (item is IStartable startable)
                _startLoop.AddItem(startable);

            if (item is IUpdatable updatable)
                _updateLoop.AddItem(updatable);

            if (item is IFixedUpdatable fixedUpdatable)
                _fixedUpdateLoop.AddItem(fixedUpdatable);

            if (item is IDisposable disposable)
                _disposeLoop.AddItem(disposable);
        }

        public void RemoveItem<T>(T item)
        {
            if (item is IAwakeable awakeable)
                _awakeLoop.RemoveItem(awakeable);

            if (item is IStartable startable)
                _startLoop.RemoveItem(startable);

            if (item is IUpdatable updatable)
                _updateLoop.RemoveItem(updatable);

            if (item is IFixedUpdatable fixedUpdatable)
                _fixedUpdateLoop.RemoveItem(fixedUpdatable);

            if (item is IDisposable disposable)
                _disposeLoop.RemoveItem(disposable);
        }

        public void Awake()
        {
            var orderedItems = _awakeLoop.OrderedItems;

            for (var i = 0; i < orderedItems.Count; i++)
            {
                var (_, items) = orderedItems[i];

                for (var k = 0; k < items.Count; k++)
                {
                    items[k].OnAwake();
                }
            }
        }

        public void Start()
        {
            var orderedItems = _startLoop.OrderedItems;

            for (var i = 0; i < orderedItems.Count; i++)
            {
                var (_, items) = orderedItems[i];

                for (var k = 0; k < items.Count; k++)
                {
                    items[k].OnStart();
                }
            }
        }

        public void Update(float deltaTime)
        {
            var orderedItems = _updateLoop.OrderedItems;

            for (var i = 0; i < orderedItems.Count; i++)
            {
                var (_, items) = orderedItems[i];

                for (var k = 0; k < items.Count; k++)
                {
                    items[k].OnUpdate(deltaTime);
                }
            }
        }

        public void FixedUpdate(float deltaTime)
        {
            var orderedItems = _fixedUpdateLoop.OrderedItems;

            for (var i = 0; i < orderedItems.Count; i++)
            {
                var (_, items) = orderedItems[i];

                for (var k = 0; k < items.Count; k++)
                {
                    items[k].OnFixedUpdate(deltaTime);
                }
            }
        }

        public void Dispose()
        {
            var orderedItems = _disposeLoop.OrderedItems;

            for (var i = 0; i < orderedItems.Count; i++)
            {
                var (_, items) = orderedItems[i];

                for (var k = 0; k < items.Count; k++)
                {
                    items[k].Dispose();
                }
            }
        }
    }
}
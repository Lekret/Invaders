using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Core
{
    public class GameLoop : IDisposable, ITickable, IFixedTickable
    {
        private readonly List<IDisposable> _disposables = new();
        private readonly InnerGameLoop<IUpdatable> _updateLoop = new();
        private readonly InnerGameLoop<IFixedUpdatable> _fixedUpdateLoop = new();

#if UNITY_EDITOR
        public void EditorValidateMissingDispatchTypes()
        {
            _updateLoop.ValidateMissingDispatchTypes();
            _fixedUpdateLoop.ValidateMissingDispatchTypes();
        }
#endif

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

        public void Add<T>(T item)
        {
            if (item is IUpdatable updatable)
                _updateLoop.AddItem(updatable);

            if (item is IFixedUpdatable fixedUpdatable)
                _fixedUpdateLoop.AddItem(fixedUpdatable);

            if (item is IDisposable disposable)
                _disposables.Add(disposable);
            
            if (item is IAwakeable awakeable)
                awakeable.OnAwake();
        }

        public void Remove<T>(T item)
        {
            if (item is IUpdatable updatable)
                _updateLoop.RemoveItem(updatable);

            if (item is IFixedUpdatable fixedUpdatable)
                _fixedUpdateLoop.RemoveItem(fixedUpdatable);
            
            if (item is IDisposable disposable)
            {
                _disposables.Remove(disposable);
                disposable.Dispose();
            }
        }
        
        void ITickable.Tick()
        {
            var deltaTime = Time.deltaTime;
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

        void IFixedTickable.FixedTick()
        {
            var deltaTime = Time.deltaTime;
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

        void IDisposable.Dispose()
        {
            foreach (var item in _disposables.ToList())
            {
                item.Dispose();
            }
            
            _disposables.Clear();
        }
    }
}
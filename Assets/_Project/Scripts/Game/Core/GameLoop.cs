using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game.Core
{
    public class GameLoop : IDisposable, ITickable, ILateTickable, IFixedTickable
    {
        private readonly List<IDisposable> _disposables = new();
        private readonly List<IDisposable> _toDisposeEndFrame = new();
        private readonly GameLoopDispatchTable<IUpdatable> _updateLoop = new();
        private readonly GameLoopDispatchTable<IFixedUpdatable> _fixedUpdateLoop = new();
        private bool _isPaused;

#if UNITY_EDITOR
        public void EditorValidateMissingDispatchTypes()
        {
            _updateLoop.ValidateMissingDispatchTypes();
            _fixedUpdateLoop.ValidateMissingDispatchTypes();
        }
#endif
        
        public void SetPaused(bool isPaused)
        {
            _isPaused = isPaused;
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

        public void Add<T>(T item)
        {
            if (item is IUpdatable updatable)
                _updateLoop.AddItem(updatable);

            if (item is IFixedUpdatable fixedUpdatable)
                _fixedUpdateLoop.AddItem(fixedUpdatable);

            if (item is IDisposable disposable)
            {
                _toDisposeEndFrame.Remove(disposable);
                _disposables.Add(disposable);
            }
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
                _toDisposeEndFrame.Add(disposable);
            }
        }
        
        void ITickable.Tick()
        {
            if (_isPaused)
                return;
            
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
            if (_isPaused)
                return;
            
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

        void ILateTickable.LateTick()
        {
            DisposeMany(_toDisposeEndFrame);
        }

        void IDisposable.Dispose()
        {
            DisposeMany(_disposables);
        }

        private static void DisposeMany(List<IDisposable> disposables)
        {
            if (disposables.Count == 0)
                return;

            var buffer = ListPool<IDisposable>.Instance.Spawn();
            buffer.AddRange(disposables);
            
            foreach (var disposable in buffer)
            {
                disposable.Dispose();
            }
            
            disposables.Clear();
            ListPool<IDisposable>.Instance.Despawn(buffer);
        }
    }
}
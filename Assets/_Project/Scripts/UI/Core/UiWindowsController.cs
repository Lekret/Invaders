using System;
using System.Collections.Generic;
using _Project.Scripts.Events;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI.Core
{
    public class UiWindowsController : IInitializable, IDisposable
    {
        private readonly UiWindowsConfig _windowsConfig;
        private readonly IMessageBroker _messageBroker;
        private readonly IInstantiator _instantiator;
        private readonly CompositeDisposable _subscriptions = new();
        private readonly List<UiWindow> _windows = new();

        public UiWindowsController(
            UiWindowsConfig windowsConfig,
            IMessageBroker messageBroker, 
            IInstantiator instantiator)
        {
            _windowsConfig = windowsConfig;
            _messageBroker = messageBroker;
            _instantiator = instantiator;
        }

        void IInitializable.Initialize()
        {
            foreach (var windowPrefab in _windowsConfig.WindowPrefabs)
            {
                var window = _instantiator.InstantiatePrefabForComponent<UiWindow>(windowPrefab);
                _windows.Add(window);
            }

            foreach (var window in _windows)
            {
                window.Init();
            }

            _messageBroker.Receive<UiWindowEvent>().Subscribe(HandleWindowEvent).AddTo(_subscriptions);
            Debug.Log($"[WindowsController] Windows created: {_windows.Count}");
        }

        void IDisposable.Dispose()
        {
            _subscriptions.Dispose();

            foreach (var window in _windows)
            {
                window.Dispose();
            }
            
            _windows.Clear();
        }

        private void HandleWindowEvent(UiWindowEvent windowEvent)
        {
            var window = FindWindow(windowEvent.WindowType);
            if (window == null)
            {
                Debug.LogError($"[WindowsController] Window not found: {windowEvent.WindowType}");
                return;
            }

            windowEvent.Action?.Invoke(window);
        }
                
        private UiWindow FindWindow(Type type)
        {
            foreach (var window in _windows)
            {
                if (type.IsInstanceOfType(window))
                    return window;
            }

            return null;
        }
    }
}
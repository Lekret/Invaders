using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI.Core
{
    public class WindowsController : IInitializable, IDisposable
    {
        private readonly WindowsConfig _windowsConfig;
        private readonly IMessageBroker _messageBroker;
        private readonly IInstantiator _instantiator;
        private readonly CompositeDisposable _subscriptions = new();
        private readonly List<UiWindow> _windows = new();

        public WindowsController(
            WindowsConfig windowsConfig,
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
                window.Init();
                _windows.Add(window);
            }

            _messageBroker.Receive<ShowWindowEvent>().Subscribe(ShowWindow).AddTo(_subscriptions);
            _messageBroker.Receive<HideWindowEvent>().Subscribe(HideWindow).AddTo(_subscriptions);
            
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

        private void ShowWindow(ShowWindowEvent windowEvent)
        {
            var window = FindWindow(windowEvent.WindowType);
            if (window == null)
            {
                Debug.LogError($"[WindowsController] Window not found: {windowEvent.WindowType}");
                return;
            }
            
            windowEvent.BeforeShow?.Invoke(window);
            window.Show();
        }

        private void HideWindow(HideWindowEvent windowEvent)
        {
            var window = FindWindow(windowEvent.WindowType);
            if (window == null)
            {
                Debug.LogError($"[WindowsController] Window not found: {windowEvent.WindowType}");
                return;
            }
            
            window.Hide();
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
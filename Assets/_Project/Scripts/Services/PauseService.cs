using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class PauseService
    {
        private readonly HashSet<object> _pauseDemanders = new();
        private readonly ReactiveCommand<bool> _pauseChangedCommand = new();

        public bool IsPaused => _pauseDemanders.Count > 0;
        
        public IObservable<bool> PausedChangedAsObservable() => _pauseChangedCommand;

        public void AddPauseDemander(object obj)
        {
            if (_pauseDemanders.Add(obj))
                DispatchChanged();
        }

        public void RemovePauseDemander(object obj)
        {
            if (_pauseDemanders.Remove(obj))
                DispatchChanged();
        }

        private void DispatchChanged()
        {
            Debug.Log($"[PauseService] Pause changed: {IsPaused}");
            _pauseChangedCommand.Execute(IsPaused);
        }
    }
}
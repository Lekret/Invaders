using System;
using UnityEngine;

namespace _Project.Scripts.UI.Core
{
    public class UiWindow : MonoBehaviour, IDisposable
    {
        private enum State
        {
            None,
            Shown,
            Hidden,
            Disposed,
        }

        [SerializeField] private GameObject _content;

        private State _state = State.None;

        public bool IsVisible => _state == State.Shown;

        public void Init()
        {
            if (_state == State.Disposed)
                return;
            
            _state = State.Hidden;
            _content.SetActive(false);
            OnInit();
        }

        public void Dispose()
        {
            if (_state == State.Disposed)
                return;
            
            _state = State.Disposed;
            OnDisposed();
        }

        public void Show()
        {
            if (_state == State.Shown || _state == State.Disposed)
                return;

            Debug.Log($"[Window] Show: {GetType()}");
            _state = State.Shown;
            _content.SetActive(true);
            OnShown();
        }

        public void Hide()
        {
            if (_state == State.Hidden || _state == State.Disposed)
                return;

            Debug.Log($"[Window] Hide: {GetType()}");
            _state = State.Hidden;
            _content.SetActive(false);
            OnHidden();
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnDisposed()
        {
        }

        protected virtual void OnShown()
        {
        }

        protected virtual void OnHidden()
        {
        }
    }
}
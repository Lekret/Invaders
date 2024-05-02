using UnityEngine;

namespace _Project.Scripts.UI.Core
{
    public class UiWindow : MonoBehaviour
    {
        private enum State
        {
            None,
            Shown,
            Hidden,
        }
        
        [SerializeField] private GameObject _content;

        private State _state = State.None;

        public void Init()
        {
            _state = State.Hidden;
            _content.SetActive(false);
        }

        public void Show()
        {
            if (_state == State.Shown)
                return;
            
            _content.SetActive(true);
            OnShown();
        }

        public void Hide()
        {
            if (_state == State.Hidden)
                return;
            
            _content.SetActive(false);
            OnHidden();
        }

        protected virtual void OnShown()
        {
        }

        protected virtual void OnHidden()
        {
        }
    }
}
using System;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Utils
{
    public class TouchButton : MonoBehaviour
    {
        [SerializeField] private Image _touchTarget;
        
        public IObservable<PointerEventData> OnPointerDownAsObservable()
        {
            return _touchTarget.OnPointerDownAsObservable();
        }
        
        public IObservable<PointerEventData> OnPointerUpAsObservable()
        {
            return _touchTarget.OnPointerUpAsObservable();
        }
    }
}
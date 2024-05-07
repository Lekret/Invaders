using System;
using _Project.Scripts.UI.Core;
using UniRx;

namespace _Project.Scripts.Events
{
    public readonly struct UiWindowEvent
    {
        public readonly Type WindowType;
        public readonly Action<UiWindow> Action;
        
        public UiWindowEvent(Type windowType, Action<UiWindow> action)
        {
            WindowType = windowType;
            Action = action;
        }
    }

    public static class WindowEventExtensions
    {
        public static void ShowWindow<TWindow>(
            this IMessagePublisher messagePublisher, 
            Action<TWindow> beforeShow = null)
            where TWindow : UiWindow
        {
            messagePublisher.Publish(new UiWindowEvent(typeof(TWindow), window =>
            {
                beforeShow?.Invoke((TWindow) window);
                window.Show();
            }));
        }
        
        public static void HideWindow<TWindow>(
            this IMessagePublisher messagePublisher) 
            where TWindow : UiWindow
        {
            messagePublisher.Publish(new UiWindowEvent(typeof(TWindow), window =>
            {
                window.Hide();
            }));
        }
        
        public static void SendToWindow<TWindow>(
            this IMessagePublisher messagePublisher, 
            Action<TWindow> action) 
            where TWindow : UiWindow
        {
            messagePublisher.Publish(new UiWindowEvent(typeof(TWindow), window =>
            {
                action?.Invoke((TWindow) window);
            }));
        }
    }
}
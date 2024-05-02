using System;
using UniRx;

namespace _Project.Scripts.UI.Core
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
            this IMessageBroker messageBroker, 
            Action<TWindow> beforeShow = null)
            where TWindow : UiWindow
        {
            messageBroker.Publish(new UiWindowEvent(typeof(TWindow), window =>
            {
                beforeShow?.Invoke((TWindow) window);
                window.Show();
            }));
        }
        
        public static void HideWindow<TWindow>(
            this IMessageBroker messageBroker) 
            where TWindow : UiWindow
        {
            messageBroker.Publish(new UiWindowEvent(typeof(TWindow), window =>
            {
                window.Hide();
            }));
        }
        
        public static void SendToWindow<TWindow>(
            this IMessageBroker messageBroker, 
            Action<TWindow> action) 
            where TWindow : UiWindow
        {
            messageBroker.Publish(new UiWindowEvent(typeof(TWindow), window =>
            {
                action?.Invoke((TWindow) window);
            }));
        }
    }
}
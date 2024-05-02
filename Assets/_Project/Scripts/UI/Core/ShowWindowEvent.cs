using System;
using UniRx;

namespace _Project.Scripts.UI.Core
{
    public readonly struct ShowWindowEvent
    {
        public readonly Type WindowType;
        public readonly Action<UiWindow> BeforeShow;

        public ShowWindowEvent(Type windowType, Action<UiWindow> beforeShow = null)
        {
            WindowType = windowType;
            BeforeShow = beforeShow;
        }
    }

    public readonly struct HideWindowEvent
    {
        public readonly Type WindowType;

        public HideWindowEvent(Type windowType)
        {
            WindowType = windowType;
        }
    }
    
    public static class ShowWindowEventExtensions
    {
        public static void ShowWindow<TWindow>(
            this IMessageBroker messageBroker, 
            Action<TWindow> beforeShow = null)
            where TWindow : UiWindow
        {
            messageBroker.Publish(new ShowWindowEvent(typeof(TWindow), w => beforeShow?.Invoke((TWindow) w)));
        }
        
        public static void HideWindow<TWindow>(
            this IMessageBroker messageBroker) 
            where TWindow : UiWindow
        {
            messageBroker.Publish(new HideWindowEvent());
        }
    }
}
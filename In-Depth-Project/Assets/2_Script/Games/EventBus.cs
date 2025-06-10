using System.Collections.Generic;
using UnityEngine.Events;

public class EventBus
{
    private static readonly IDictionary<EventType, UnityEvent<float>> Event = new Dictionary<EventType, UnityEvent<float>>();

    public static void Subscribe(EventType eventType, UnityAction<float> listener) // 备刀
    {
        UnityEvent<float> thisEvent;

        if (Event.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<float>();
            thisEvent.AddListener(listener);
            Event.Add(eventType, thisEvent);
        }
    }

    public static void Unsubscribe(EventType eventType, UnityAction<float> listener) // 备刀 秦力
    {
        UnityEvent<float> thisEvent;

        if (Event.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Publish(EventType type , float amount) // 角青
    {
        UnityEvent<float> thisEvent;

        if (Event.TryGetValue(type, out thisEvent))
        {
            thisEvent?.Invoke(amount);
        }
    }
}

public enum EventType
{
   HpUI,
   CurrencyUI
}
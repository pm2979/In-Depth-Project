using System.Collections.Generic;
using UnityEngine.Events;

public class EventBus
{
    private static readonly IDictionary<EventType, UnityEvent> Event = new Dictionary<EventType, UnityEvent>();

    public static void Subscribe(EventType eventType, UnityAction listener) // ����
    {
        UnityEvent thisEvent;

        if (Event.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Event.Add(eventType, thisEvent);
        }
    }

    public static void Unsubscribe(EventType eventType, UnityAction listener) // ���� ����
    {
        UnityEvent thisEvent;

        if (Event.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Publish(EventType type) // ����
    {
        UnityEvent thisEvent;

        if (Event.TryGetValue(type, out thisEvent))
        {
            thisEvent?.Invoke();
        }
    }
}

public enum EventType
{
   
}
using System.Collections;
using System.Collections.Generic;

public static partial class Core
{
    static Dictionary<string, GameEvent> eventBag = new Dictionary<string, GameEvent>();
    //subscribe event

    public static void SubscribeEvent(string eventName, GameEvent eventObj)
    {
        GameEvent existing;
        if(eventBag.TryGetValue(eventName, out existing))
        {
            existing += eventObj; // subscribe into delegate variable

        }
        else
        {
            existing = eventObj;
        }
        eventBag[eventName] = existing;
    }
    //unsubscribe event
    public static void UnsubscribeEvent(string eventName, GameEvent eventObj)
    {
        GameEvent existing;
        if (eventBag.TryGetValue(eventName, out existing))
        {
            existing -= eventObj; // Unsubscribe into delegate variable

        if (existing == null)
            eventBag.Remove(eventName);
        }
    }

    //broadcast event
    public static void BroadcastEvent(string eventName, object sender, params object[] args)
    {
        GameEvent existing;
        if(eventBag.TryGetValue(eventName, out existing))
        {
            existing(sender, args); //executing the boxed method in delegate variable
        }
    }
    //clear events
    public static void ClearEvents()
    {
        eventBag.Clear();
    }
}
public delegate void GameEvent(object sender, object[] args);

using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using UnityEngine;
using UnityEngine.Events;

public class DisplayEvent : MonoBehaviour,ICalenderAttendee
{
    public void AddSelfToEvent(Quincy.Calender.Event Event)
    {
        throw new System.NotImplementedException();
    }

    public void OnNotify(Quincy.Calender.Event e)
    {
        Debug.Log("Event Triggered: " + e.EventName);
    }

    public void RegisterNotify(Quincy.Calender.Event Event, UnityAction<string> notify)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveSelfFromEvent(Quincy.Calender.Event Event)
    {
        throw new System.NotImplementedException();
    }

    public void UnregisterNotify(Quincy.Calender.Event Event, UnityAction<string> notify)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

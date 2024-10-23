using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Quincy.Calender
{
    public class CalenderAttendeeComponent : MonoBehaviour, ICalenderAttendee
    {
        [SerializeField]private GameObject owner;

        public void AddSelfToEvent(Event Event)
        {
            Event.AddAttendee(this);
        }

        public void RemoveSelfFromEvent(Event Event)
        {
            Event.RemoveAttendee(this);
        }

        public void RegisterNotify(Event Event, UnityAction<string> notify)
        {
            Event.RegisterFunction(notify);
        }

        public void UnregisterNotify(Event Event, UnityAction<string> notify)
        {
            Event.UnregisterFunction(notify);
        }

        public void OnNotify()
        {
            
        }
    }

}

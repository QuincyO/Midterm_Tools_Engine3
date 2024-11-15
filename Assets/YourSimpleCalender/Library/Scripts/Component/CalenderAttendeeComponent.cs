using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Quincy.Calender
{
    public class CalenderAttendeeComponent : MonoBehaviour, ICalendarListener
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


        public void OnNotify(Event e)
        {
            
        }
    }

}

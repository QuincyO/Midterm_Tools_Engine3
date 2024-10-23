using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Quincy.Calender
{
    public partial class MyCalender : MonoBehaviour
    {
        private List<CalenderEvent> events;
        
        
        [SerializeField] private List<ScriptableObjectEvent> _eventsScriptableObjects;

        public void Awake()
        {
            events = new List<CalenderEvent>();
            foreach (ScriptableObjectEvent scriptableObject in _eventsScriptableObjects)
            {
                events.Add(new CalenderEvent(scriptableObject));
            }
        }

        //Overloaded AddEvent
        void AddEvent(CalenderEvent e)
        {
            events.Add(e);
        }

        void AddEvent(Date eventDate, string eventName, Color color = default)
        {
            events.Add(new CalenderEvent(eventDate, eventName, color));
        }

        void RemoveEvent(CalenderEvent e)
        {
            events.Remove(e);
        }
        
    }

}

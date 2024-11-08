using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Quincy.Calender
{
    public partial class MyCalender : MonoBehaviour
    {
        public string CalenderName;



        public  LinkedList<Event> events; //The List that will actually be used to store events
        [SerializeField] private List<KeyDate> keyDates = new List<KeyDate>(); //Editor List of Dates, created with SCriptable Objects

        public void Awake()
        {
            events = new LinkedList<Event>();
            CalenderManager.AddCalender(this);
            if (keyDates.Count > 0 && keyDates != null) 
            {
                foreach (KeyDate scriptableObject in keyDates)
                {
                    AddEvent(scriptableObject);
                }   
            }
        }
        

        public void AddEvent( string eventName,Date eventStartDate,Date? eventEndDate = null, Color color = default)
        {
            events.AddLast(new Event(eventStartDate, eventName,eventEndDate, color));
            CalenderManager.UpdateManager();
        }
        public void AddEvent(KeyDate keyDate)
        {
            events.AddLast(new Event(keyDate));
            CalenderManager.UpdateManager();
        }

        void RemoveEvent(Event e)
        {
            events.Remove(e);
            CalenderManager.UpdateManager();
        }
    }

}

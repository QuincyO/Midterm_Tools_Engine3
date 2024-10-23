using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Quincy.Calender
{
    public partial class MyCalender : MonoBehaviour
    {
        private List<Event> events; //The List that will actually be used to store events
        [SerializeField] private List<KeyDate> keyDates; //Editor List of Dates, created with SCriptable Objects

        public void Awake()
        {
            events = new List<Event>();
            foreach (KeyDate scriptableObject in keyDates)
            {
                events.Add(new Event(scriptableObject));
            }
            events.Sort();
        }
        

        //Overloaded AddEvent
        void AddEvent(Event e)
        {
            events.Add(e);
        }

        
        /*TODO 1: Need to implement a way to ensure that anytime an event gets added to the calender it also gets added to the CalenderManager so the eveent can get properly process and then dispatched
         todo:  Ideally I want that to be hidden from the the Client
        
        */ 
        void AddEvent( string eventName,Date eventStartDate,Date? eventEndDate = null, Color color = default)
        {
            events.Add(new Event(eventStartDate, eventName,eventEndDate, color));
        }

        void RemoveEvent(Event e)
        {
            events.Remove(e);
        }
    }

}

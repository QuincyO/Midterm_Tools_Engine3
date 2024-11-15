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
                    if (scriptableObject == null) {continue;}
                    AddEvent(scriptableObject);
                }   
            }
        }
        
        public List<Quincy.Calender.Event> GetEventsForMonth(Month month, int year)
        {

            List<Quincy.Calender.Event> eventsThisMonth = new List<Quincy.Calender.Event>();

            foreach (var e in events)
            {
                if (e.startingDate.Month == month && e.startingDate.Year == year)
                {
                    eventsThisMonth.Add(e);
                }
                if (e.startingDate.Month > month && e.startingDate.Year == year)
                {
                    break;
                }   
            }
            

            return eventsThisMonth;
        }

        #region Methods to get Events
        public Event GetEvent(string eventName)
        {
            foreach (Event e in events)
            {
                if (e.EventName == eventName)
                {
                    return e;
                }
            }
            Debug.LogWarning("Event not found");
            return null;
        }


        public Event GetEvent(int year,Month month, int day)
        {
            foreach (Event e in events)
            {
                if (e.startingDate.Year == year && e.startingDate.Month == month && e.startingDate.Day == day)
                {
                    return e;
                }
            }
            Debug.LogWarning("Event not found");
            return null;
        }

        #endregion
        public void AddEvent( string eventName,Date eventStartDate,Date? eventEndDate = null, Color color = default)
        {
            events.AddLast(new Event(eventStartDate, eventName,eventEndDate, color));
            CalenderManager.SortEvents();
        }
        public void AddEvent(KeyDate keyDate)
        {
            events.AddLast(new Event(keyDate));
            CalenderManager.SortEvents();
        }

        void RemoveEvent(Event e)
        {
            events.Remove(e);
            CalenderManager.SortEvents();
        }

        public void ListenToEvent(string eventName, ICalenderAttendee attendee)
        {
            Event e = GetEvent(eventName);
            if (e == null)
            {
                Debug.LogWarning("Event not found");
                return;
            }
            e.AddAttendee(attendee);
        }
    }
}

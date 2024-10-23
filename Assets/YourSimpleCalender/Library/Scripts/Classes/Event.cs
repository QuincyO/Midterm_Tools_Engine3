using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Events;

namespace Quincy.Calender
{
    public partial class MyCalender 
    {
        public static Date CurrentDate { get; private set; }
    }

    [Serializable]
    public class Event :IComparable<Event>
    {

        private bool EndDateTriggers = false;
        private Date startingDate;
        private Date? endDate;

        [SerializeField] public string EventName;
        
        private event UnityAction<string> OnEvent;

        public Color EventColor { get; set; }

        private List<ICalenderAttendee> _attendees;


        #region Boilerplate
        
        public void RegisterFunction(UnityAction<string> notify)
        {
            OnEvent += notify;
        }

        public void UnregisterFunction(UnityAction<string> notify)
        {
            OnEvent -= notify;
        }

        public void AddAttendee(ICalenderAttendee calenderAttendee)
        {
            _attendees.Add(calenderAttendee);
        }

        public void RemoveAttendee(ICalenderAttendee calenderAttendee)
        {
            _attendees.Remove(calenderAttendee);
        }

        internal void AddEndDate(Date date)
        {
            EndDateTriggers = true;
            endDate = date;
        }

        internal void RemoveEndDate(Date date)
        {
            EndDateTriggers = false;
            endDate = null;

        }

        #endregion

        #region Constructor

        public Event()
        {
            EventName = string.Empty;
            _attendees = new List<ICalenderAttendee>();
            EventColor = Color.white; 
            startingDate = MyCalender.CurrentDate;
            endDate = null;

        }

        public Event(KeyDate scriptable)
        {
            
            startingDate = scriptable.StartDate;
            endDate = scriptable.EndDate;
            EventName = scriptable.eventName;
            _attendees = new List<ICalenderAttendee>();
            
        }


        /// <summary>
        /// Event Constructor
        /// </summary>
        /// <param name="date"> Date to add to event</param>
        /// <param name="eventName">Name of the event</param>
        /// <param name="eventColor">Color of the UI</param>
        public Event(Date date, string eventName,Date? endDate = null, Color eventColor = default)
        {
            startingDate = date;
            this.endDate = endDate;            
            EventColor = eventColor;
            EventName = eventName;
            _attendees = new List<ICalenderAttendee>();
        }
        #endregion

        void NotifyAttendees()
        {
            OnEvent?.Invoke(EventName);
        }

        public int CompareTo(Event other)
        {
            if (other == null) return 1;
            
            
            return startingDate.CompareTo(other.startingDate);
        }
    }
    
    

}



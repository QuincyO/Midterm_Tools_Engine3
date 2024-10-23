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

    public class CalenderEvent
    {
        private List<Date> _dates;

        public string EventName { get; set; }
        
        private event UnityAction<CalenderEvent> OnEvent;

        public Color EventColor { get; set; }

        private List<ICalenderAttendee> _attendees;


        #region Boilerplate
        
        public void RegisterFunction(UnityAction<CalenderEvent> notify)
        {
            OnEvent += notify;
        }

        public void UnregisterFunction(UnityAction<CalenderEvent> notify)
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

        internal void AddDate(Date date)
        {
            _dates.Add(date);
        }

        internal void RemoveDate(Date date)
        {
            _dates.Remove(date);
        }

        #endregion

        #region Constructor

        public CalenderEvent()
        {
            _dates = new List<Date>();
            EventName = string.Empty;
            _attendees = new List<ICalenderAttendee>();
            EventColor = Color.white; 
            _dates.Add(MyCalender.CurrentDate);
        }

        public CalenderEvent(ScriptableObjectEvent scriptable)
        {
            EventName = scriptable.eventName;
        }


        /// <summary>
        /// CalenderEvent Constructor
        /// </summary>
        /// <param name="date"> Date to add to event</param>
        /// <param name="eventName">Name of the event</param>
        /// <param name="eventColor">Color of the UI</param>
        public CalenderEvent(Date date, string eventName, Color eventColor = default)
        {
            _dates = new List<Date>();
            _attendees = new List<ICalenderAttendee>();
            _dates.Add(date);
            EventColor = eventColor;
            EventName = eventName;
        }
        #endregion

        void NotifyAttendees()
        {
            OnEvent?.Invoke(this);
        }
    }
    
    

}



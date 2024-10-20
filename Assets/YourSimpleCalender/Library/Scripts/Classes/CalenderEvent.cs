using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

namespace Quincy.Calender
{
    public partial class Calender
    {
        public static Date CurrentDate { get; private set; }
    }

    public class CalenderEvent
    {
        private List<Date> _dates;

        public string EventName { get; set; }
        
        private event Action<CalenderEvent> onEvent;

        public Color EventColor { get; set; }

        private List<ICalenderAttendee> _attendees;


        #region Boilerplate
        
        public void RegisterFunction(Action<CalenderEvent> notify)
        {
            onEvent += notify;
        }

        public void UnregisterFunction(Action<CalenderEvent> notify)
        {
            onEvent -= notify;
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
            _dates.Add(Calender.CurrentDate);
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
            onEvent?.Invoke(this);
        }


    }
    
    

}



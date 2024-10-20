using System;
using System.Collections;
using System.Collections.Generic;
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

        public Color EventColor { get; set; }

        private List<IAttendee> _attendees;


        #region Boilerplate

        public void AddAttendee(IAttendee attendee)
        {
            _attendees.Add(attendee);
        }

        public void RemoveAttendee(IAttendee attendee)
        {
            _attendees.Remove(attendee);
        }

        public void AddDate(Date date)
        {
            _dates.Add(date);
        }

        public void RemoveDate(Date date)
        {
            _dates.Remove(date);
        }

        #endregion

        #region Constructor

        public CalenderEvent()
        {
            _dates = new List<Date>();
            EventName = string.Empty;
            _attendees = new List<IAttendee>();
            EventColor = Color.white;
            _dates.Add(Calender.CurrentDate);
        }

        #endregion



    }

}



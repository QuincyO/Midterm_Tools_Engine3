using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using UnityEngine.Events;

namespace Quincy.Calender
{
    public interface ICalenderAttendee
    {
        /// <summary>
        /// Add this class to the event to be able to attend an Event
        /// </summary>
        /// <param name="Event">Event you want to subsribe to </param>
        public void AddSelfToEvent(Event Event);
    
        void OnNotify(Event @event);
    }

}

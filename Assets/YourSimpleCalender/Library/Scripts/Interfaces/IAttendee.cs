using System.Collections;
using System.Collections.Generic;

namespace Quincy.Calender
{
    public interface IAttendee
    {
        /// <summary>
        /// Add this class to the event to be able to attend an Event
        /// </summary>
        /// <param name="calenderCalenderEvent">Event you want to subsribe to </param>
        public void AddSelfToEvent(CalenderEvent calenderCalenderEvent);
    
        /// <summary>
        /// Remove yourself from this event
        /// </summary>
        /// <param name="calenderCalenderEvent"></param>
        public void RemoveSelfFromEvent(CalenderEvent calenderCalenderEvent);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;

namespace Quincy.Calender
{
    public interface ICalenderAttendee
    {
        /// <summary>
        /// Add this class to the event to be able to attend an Event
        /// </summary>
        /// <param name="Event">Event you want to subsribe to </param>
        public void AddSelfToEvent(CalenderEvent Event);
    
        /// <summary>
        /// Remove yourself from this event
        /// </summary>
        /// <param name="Event"></param>
        public void RemoveSelfFromEvent(CalenderEvent Event);

        /// <summary>
        /// Pass In the function you want to get called when the event triggers.
        /// </summary>
        /// <param name="FunctionName">This Function is going to be the one that gets called</param>
        /// <remarks>To use write <see cref="Event.RegisterFunction()"/></remarks>
        void RegisterNotify(CalenderEvent Event,Action<CalenderEvent> notify);
        
        void UnregisterNotify(CalenderEvent Event,Action<CalenderEvent> notify);
    }

}

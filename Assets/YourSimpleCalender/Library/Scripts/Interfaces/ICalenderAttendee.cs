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
    
        /// <summary>
        /// Remove yourself from this event
        /// </summary>
        /// <param name="Event"></param>
        public void RemoveSelfFromEvent(Event Event);

        /// <summary>
        /// Pass In the function you want to get called when the event triggers.
        /// </summary>
        /// <param name="FunctionName">This Function is going to be the one that gets called</param>
        /// <remarks>To use write <see cref="Event.RegisterFunction()"/></remarks>
        void RegisterNotify(Event Event,UnityAction<string> notify);
        
        void UnregisterNotify(Event Event,UnityAction<string> notify);


        void OnNotify();
    }

}

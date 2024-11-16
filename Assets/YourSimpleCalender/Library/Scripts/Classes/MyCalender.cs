using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Quincy.Calender
{
    public partial class MyCalendar : MonoBehaviour
    {
        /// <summary>
        /// Gets the current date.
        /// </summary>
        public static Date CurrentDate { get; private set; }

        public string CalenderName;
        public LinkedList<Event> events; //The List that will actually be used to store events
        [SerializeField] private List<KeyDate> keyDates = new List<KeyDate>(); //Editor List of Dates, created with Scriptable Objects

        /// <summary>
        /// Initializes the calendar, adds it to the CalendarManager, and adds events from keyDates.
        /// </summary>
        public void Awake()
        {
            events = new LinkedList<Event>();
            CalendarManager.AddCalender(this);
            if (keyDates.Count > 0 && keyDates != null)
            {
                foreach (KeyDate scriptableObject in keyDates)
                {
                    if (scriptableObject == null) { continue; }
                    AddEvent(scriptableObject);
                }
            }
        }

        /// <summary>
        /// Retrieves a list of events for a specific month and year.
        /// </summary>
        /// <param name="month">The month to retrieve events for.</param>
        /// <param name="year">The year to retrieve events for.</param>
        /// <returns>A list of events occurring in the specified month and year.</returns>
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

        /// <summary>
        /// Retrieves an event by its name.
        /// </summary>
        /// <param name="eventName">The name of the event to retrieve.</param>
        /// <returns>The event with the specified name, or null if not found.</returns>
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

        /// <summary>
        /// Retrieves an event by its date.
        /// </summary>
        /// <param name="year">The year of the event.</param>
        /// <param name="month">The month of the event.</param>
        /// <param name="day">The day of the event.</param>
        /// <returns>The event occurring on the specified date, or null if not found.</returns>
        public Event GetEvent(int year, Month month, int day)
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

        /// <summary>
        /// Adds a new event to the calendar.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventStartDate">The start date of the event.</param>
        /// <param name="eventEndDate">The optional end date of the event.</param>
        /// <param name="color">The color associated with the event.</param>
        public void AddEvent(string eventName, Date eventStartDate, Date? eventEndDate = null, Color color = default)
        {
            events.AddLast(new Event(eventStartDate, eventName, eventEndDate, color));
            CalendarManager.SortEvents();
        }

        /// <summary>
        /// Adds a new event to the calendar using a KeyDate.
        /// </summary>
        /// <param name="keyDate">The KeyDate object containing event information.</param>
        public void AddEvent(KeyDate keyDate)
        {
            events.AddLast(new Event(keyDate));
            CalendarManager.SortEvents();
        }

        /// <summary>
        /// Removes an event from the calendar.
        /// </summary>
        /// <param name="e">The event to remove.</param>
        void RemoveEvent(Event e)
        {
            events.Remove(e);
            CalendarManager.SortEvents();
        }

        /// <summary>
        /// Registers a listener to be notified when a specific event occurs.
        /// </summary>
        /// <param name="eventName">The name of the event to listen to.</param>
        /// <param name="attendee">The listener that wants to be notified.</param>
        public void ListenToEvent(string eventName, ICalendarListener attendee)
        {
            Event e = GetEvent(eventName);
            if (e == null)
            {
                Debug.LogWarning("Event not found");
                return;
            }
            e.AddAttendee(attendee);
        }

        /// <summary>
        /// Registers a listener to be notified when any event occurs.
        /// </summary>
        /// <param name="attendee">The listener that wants to be notified.</param>
        public void ListenToAllEvents(ICalendarListener attendee)
        {
            foreach (Event e in events)
            {
                e.AddAttendee(attendee);
            }
        }
    }
}

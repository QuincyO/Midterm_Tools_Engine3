using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Quincy.Calender
{
    [Serializable]
    public class Event : IComparable<Event>
    {
        public bool EndDateTriggers = false;
        public Date startingDate;

        [SerializeField] public string EventName;

        public UnityAction<string> OnEvent;

        public List<GameObject> EventEffectsPrefabs = new List<GameObject>();
        public LinkedList<EffectScript> effectScripts = new LinkedList<EffectScript>();

        public Color EventColor { get; set; }

        public List<ICalendarListener> _attendees;

        public Sprite EventIcon;

        #region Boilerplate

        /// <summary>
        /// Adds an attendee to the event.
        /// </summary>
        /// <param name="calenderAttendee">The attendee to add.</param>
        public void AddAttendee(ICalendarListener calenderAttendee)
        {
            foreach (var attendee in _attendees)
            {
                if (attendee == calenderAttendee)
                {
                    Debug.LogWarning("Attendee already exists in event. You are trying to add the same attendee twice");
                    return;
                }
            }
            _attendees.Add(calenderAttendee);
        }

        /// <summary>
        /// Removes an attendee from the event.
        /// </summary>
        /// <param name="calenderAttendee">The attendee to remove.</param>
        public void RemoveAttendee(ICalendarListener calenderAttendee)
        {
            _attendees.Remove(calenderAttendee);
        }



        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class with default values.
        /// </summary>
        public Event()
        {
            EventName = string.Empty;
            _attendees = new List<ICalendarListener>();
            EventColor = Color.white;
            startingDate = MyCalendar.CurrentDate;
            EventIcon = null;
            EventEffectsPrefabs = new List<GameObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class using a KeyDate.
        /// </summary>
        /// <param name="scriptable">The KeyDate scriptable object containing event details.</param>
        public Event(KeyDate scriptable)
        {
            startingDate = scriptable.StartDate;
            EventName = scriptable.eventName;
            _attendees = new List<ICalendarListener>();
            EventColor = scriptable.eventColor;
            EventIcon = scriptable.eventIcon;
            foreach (var prefab in scriptable.eventEffectsPrefab)
            {
                if (prefab == null) continue;
                EventEffectsPrefabs.Add(prefab);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class with specified parameters.
        /// </summary>
        /// <param name="date">The start date of the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="endDate">Optional end date of the event.</param>
        /// <param name="eventColor">The color associated with the event UI.</param>
        public Event(Date date, string eventName, Color eventColor = default)
        {
            startingDate = date;
            EventColor = eventColor;
            EventName = eventName;
            _attendees = new List<ICalendarListener>();
            EventIcon = null;
            EventEffectsPrefabs = new List<GameObject>();
        }

        #endregion

        /// <summary>
        /// Triggers the event, activating effects and notifying attendees.
        /// </summary>
        public void TriggerEvent()
        {
            OnEvent?.Invoke(EventName);

            foreach (var effect in effectScripts)
            {
                effect.gameObject.SetActive(true);
                effect.TriggerEffect();
            }
            foreach (var attendee in _attendees)
            {
                attendee.OnNotify(this);
            }
        }

        /// <summary>
        /// Compares this event to another based on starting date.
        /// </summary>
        /// <param name="other">The other event to compare to.</param>
        /// <returns>An integer indicating the relative order.</returns>
        public int CompareTo(Event other)
        {
            if (other == null) return 1;

            return startingDate.CompareTo(other.startingDate);
        }
    }
}

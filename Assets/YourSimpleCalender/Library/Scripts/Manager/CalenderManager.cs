using System.Collections;
using System;
using System.Collections.Generic;
using Quincy.Calender;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices;

namespace Quincy.Calender
{
    public partial class CalenderManager : MonoBehaviour
    {


        private static LinkedList<MyCalender> _calenders = new LinkedList<MyCalender>();
        private static LinkedList<Date> _eventsToRemove = new LinkedList<Date>();
        private static SortedList<Date, Event> _eventsByDate = new SortedList<Date, Event>();
        private static SortedList<Date, Event> _eventsToday = new SortedList<Date, Event>();
        private Date _lastProcessedDate;
        [HideInInspector] public Date CurrentDate {get; private set;}

        [Tooltip("The Date the the calender will start at, Must be set in hours from 0-23")]
        [SerializeField] public Date StartingDate;
        public int TimeStepInMinutes = 1;

        public bool IsMilitaryTime = false;
 


        public static MyCalender CreateCalender(string calenderName)
        {
            GameObject obj = new GameObject(calenderName);
            MyCalender myCalender = obj.AddComponent<MyCalender>();

            return myCalender;

        }

        public static void AddCalender(MyCalender calender)
        {
            if (_calenders.Contains(calender))
            {
                Debug.LogWarning("Calender already exists in the manager");
                return;
            }
            _calenders.AddLast(calender);
            UpdateManager();

        }

        public static void UpdateManager()
        {
            foreach (var calender in _calenders)
            {
                foreach (var e in calender.events)
                {
                    if (!_eventsByDate.ContainsKey(e.startingDate))
                        _eventsByDate.Add(e.startingDate, e);
                }
            }
        }


         public List<Quincy.Calender.Event> GetEventsForMonth(Month? month, int year)
        {

            List<Quincy.Calender.Event> events = new List<Quincy.Calender.Event>();

            foreach (var e in _eventsByDate.Values)
            {
                if (e.startingDate.Month == month && e.startingDate.Year == year)
                {
                    events.Add(e);
                }
                if (e.startingDate.Month > month && e.startingDate.Year == year)
                {
                    break;
                }   
            }
            

            return events;
        }

        public static void SetPause(bool isPaused)
        {
            TimeManager.isPaused = isPaused;
        }

        private void AdvanceCurrentDate()
        {
            CurrentDate = CurrentDate.AddMinutes(TimeStepInMinutes);

            //Debug.Log(CurrentDate);
        }


        private void Tick()
        {
            AdvanceCurrentDate();
            
            PrepareEventsForToday();

            ProcessTodayEvents();

            RemoveOldEvents();
        }

        private void RemoveOldEvents()
        {
            if(_eventsToRemove.Count == 0)
                return;
            foreach (var date in _eventsToRemove)
            {
                if(_eventsByDate.ContainsKey(date))
                    _eventsByDate.Remove(date);
                if (_eventsToday.ContainsKey(date))
                    _eventsToday.Remove(date);
            }
            _eventsToRemove.Clear();
        }

        private void PrepareEventsForToday()
        {
            if (CurrentDate.Day == _lastProcessedDate.Day &&
                CurrentDate.Month == _lastProcessedDate.Month &&
                CurrentDate.Year == _lastProcessedDate.Year)
                return;

            _lastProcessedDate = CurrentDate;

            foreach (var upcomingEvent in _eventsByDate)
            {
                if (upcomingEvent.Key.Day == CurrentDate.Day &&
                   upcomingEvent.Key.Month == CurrentDate.Month &&
                   upcomingEvent.Key.Year == CurrentDate.Year)
                {
                    _eventsToday.Add(upcomingEvent.Key, upcomingEvent.Value);
                    _eventsToRemove.AddLast(upcomingEvent.Key);

                }
                else if (upcomingEvent.Key > CurrentDate)
                {
                    break;
                }
            }
        }

        private void ProcessTodayEvents()
        {
            foreach (var Event in _eventsToday)
            {
                if (CurrentDate >= Event.Key)
                {
                    Event.Value.OnEvent?.Invoke(Event.Value.EventName);
                    _eventsToRemove.AddLast(Event.Key);
                }
            }
        }
    }
}

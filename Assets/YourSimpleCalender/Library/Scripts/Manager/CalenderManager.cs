using System.Collections;
using System;
using System.Collections.Generic;
using Quincy.Calender;
using UnityEngine;

namespace Quincy.Calender
{
    public partial class CalenderManager : MonoBehaviour
    {


        private static LinkedList<MyCalender> _calenders = new LinkedList<MyCalender>();
        private static SortedList<Date, Event> _eventsByDate = new SortedList<Date, Event>();
        private static SortedList<Date, Event> _eventsToday = new SortedList<Date, Event>();
        private Date _lastProcessedDate;
        private Date CurrentDate;
        public Date StartingDate;
        public int TimeStepInMinutes = 1;
 


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
                }
                else if (upcomingEvent.Key > CurrentDate)
                {
                    break;
                }
            }
        }

        private void ProcessTodayEvents()
        {
            LinkedList<Date> eventsToRemove = new LinkedList<Date>();
            foreach (var Event in _eventsToday)
            {
                if (CurrentDate == Event.Key)
                {
                    Event.Value.OnEvent.Invoke(Event.Value.EventName);
                    eventsToRemove.AddLast(Event.Key);
                }
            }
            foreach (var date in eventsToRemove)
            {
                _eventsToday.Remove(date);
            }
            eventsToRemove.Clear();
        }
    }
}

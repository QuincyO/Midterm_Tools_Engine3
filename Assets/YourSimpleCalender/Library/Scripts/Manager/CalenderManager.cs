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
         
        static SortedList<Date, Event> _eventsByDate = new SortedList<Date, Event>();
        static SortedList<Date,Event> _eventsToday = new SortedList<Date, Event>();
        public static Date CurrentDate;
        private static Date _lastProcessedDate;
        public static Date StartingDate;
        public static int IncrementAmount = 1;

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
                    if(!_eventsByDate.ContainsKey(e.startingDate))
                    _eventsByDate.Add(e.startingDate,e);
                }
            }
        }

        public static void SetPause(bool isPaused)
        {
            TimeManager.isPaused = isPaused;
        }

        private static void AdvanceCurrentDate()
        {
            CurrentDate = CurrentDate.AddMinutes(IncrementAmount);
        }

        private static void Tick()
        {
            AdvanceCurrentDate();

            PrepareEventsForToday();

            ProcessTodayEvents();
        }

        private static void PrepareEventsForToday()
        {
            if(CurrentDate.Day == _lastProcessedDate.Day &&
                CurrentDate.Month == _lastProcessedDate.Month && 
                CurrentDate.Year == _lastProcessedDate.Year)
                return;

            _lastProcessedDate = CurrentDate;


            foreach (var upcomingEvent in _eventsByDate)
            {
                if(upcomingEvent.Key.Day == CurrentDate.Day &&
                   upcomingEvent.Key.Month == CurrentDate.Month &&
                   upcomingEvent.Key.Year == CurrentDate.Year)
                   {
                    _eventsToday.Add(upcomingEvent.Key,upcomingEvent.Value);
                   }
                   else if (upcomingEvent.Key > CurrentDate)
                   {
                       break;
                   }
            }

            foreach (var Event in _eventsToday)
            {
                _eventsByDate.Remove(Event.Key);
            }

        }
        

        private static void ProcessTodayEvents()
        {
          foreach (var Event in _eventsToday)
          {
              if(CurrentDate == Event.Key)
              {
                  Event.Value.OnEvent.Invoke(Event.Value.EventName);
              }
          }
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

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
        public static event Action<Date> OnNewDay;
        public static event Action OnTimeChanged;

        public Coroutine intensityCoroutine;

        [Header("Calender Settings")]
        [Tooltip("The Date the the calender will start at, Must be set in hours from 0-23")]
        [SerializeField] public Date StartingDate;
        [SerializeField] public List<WeatherEvents> weatherPrefabs = new List<WeatherEvents>(); //This is just so I can add weather events in the inspector
        [SerializeField] public static Dictionary<string,GameObject> Weather = new Dictionary<string, GameObject>();

        
        
        [Space][Header("Time Settings")]
        public int TimeStepInMinutes = 1;
        public float TickRate = 1;
        [SerializeField] Light2D sun; 


        [HideInInspector]public bool IsMilitaryTime = false;

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


         public List<Quincy.Calender.Event> GetEventsForMonth(Month month, int year)
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

        private void AdvanceCurrentTime()
        {
            CurrentDate = CurrentDate.AddMinutes(TimeStepInMinutes);
            OnTimeChanged?.Invoke();

        if (sun != null)
            {
                float maxIntensity = 1.0f;
                float minIntensity = 0.05f;
                float targetIntensity;

                if (CurrentDate.TotalMinutes < 720) // Morning: 0 to 720 minutes (midday)
                {
                    targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.InverseLerp(0, 720, CurrentDate.TotalMinutes));
                }
                else // Afternoon to Midnight: 720 to 1440 minutes
                {
                    targetIntensity = Mathf.Lerp(maxIntensity, minIntensity, Mathf.InverseLerp(720, 1440, CurrentDate.TotalMinutes));
                }
                
                if (intensityCoroutine != null) StopCoroutine(intensityCoroutine);
                
                intensityCoroutine = StartCoroutine(TransitionSun(sun.intensity, targetIntensity, TickRate));

            }
        }

        private IEnumerator TransitionSun(float start, float end, float duration)
        {
            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                sun.intensity = Mathf.Lerp(start, end, time / duration);
                yield return null;
            }
        }


        private void Tick()
        {
            AdvanceCurrentTime();
            
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

            OnNewDay?.Invoke(CurrentDate);
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
                    Event.Value.NotifyAttendees();
                    _eventsToRemove.AddLast(Event.Key);
                }
            }
        }
    }
    [Serializable]
    public class WeatherEvents
    {
        public string Key;
        public GameObject weatherPrefab;
    }

}

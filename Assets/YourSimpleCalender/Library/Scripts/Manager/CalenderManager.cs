using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

namespace Quincy.Calender
{
    /// <summary>
    /// Manages the calendar system, including events, time progression, and weather effects.
    /// </summary>
    public partial class CalendarManager : MonoBehaviour
    {
        /// <summary>
        /// A linked list of all calendars managed by the CalendarManager.
        /// </summary>
        private static LinkedList<MyCalendar> _calenders = new LinkedList<MyCalendar>();

        /// <summary>
        /// A linked list of events that have already passed.
        /// </summary>
        private static LinkedList<Event> _passedEvents = new LinkedList<Event>();

        /// <summary>
        /// A sorted list of events organized by their associated date.
        /// </summary>
        private static SortedList<Date, Event> _eventsByDate = new SortedList<Date, Event>();

        /// <summary>
        /// A sorted list of events scheduled for the current day.
        /// </summary>
        private static SortedList<Date, Event> _eventsToday = new SortedList<Date, Event>();

        /// <summary>
        /// The date that was last processed for events.
        /// </summary>
        private Date _lastProcessedDate;

        /// <summary>
        /// The current date in the calendar system.
        /// </summary>
        [HideInInspector]
        public Date CurrentDate { get; private set; }

        /// <summary>
        /// Event triggered when a new day begins.
        /// </summary>
        public static event Action<Date> OnNewDay;

        /// <summary>
        /// Event triggered when time changes.
        /// </summary>
        public static event Action OnTimeChanged;

        /// <summary>
        /// Coroutine that handles the intensity transition of the sun.
        /// </summary>
        public Coroutine intensityCoroutine;

        [Header("Calender Settings")]
        /// <summary>
        /// The date the calendar will start at, must be set in hours from 0-23.
        /// </summary>
        [Tooltip("The Date the the calender will start at, Must be set in hours from 0-23")]
        [SerializeField]
        public Date StartingDate;



        /// <summary>
        /// The prefab used to display the calendar UI.
        /// </summary>
        [SerializeField]
        public GameObject CalenderUIPrefab;

        [Space]
        [Header("Time Settings")]

        /// <summary>
        /// The number of minutes to advance the time by in each step.
        /// </summary>
        [HideInInspector]
        public int TimeStepInMinutes = 1;

        /// <summary>
        /// The rate at which time ticks forward.
        /// </summary>
        [HideInInspector]
        public float TickRate = 1;

        [SerializeField] Light2D sun;
        [Space]

        /// <summary>
        /// Determines whether the time is displayed in military format.
        /// </summary>
        [HideInInspector]
        public bool IsMilitaryTime = false;


            


        public static CalendarManager Instance { get; private set; }


        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }

            TimeManager.OnTick += Tick;
            Date.isMilitaryTime = IsMilitaryTime;
            if (!StartingDate.isValid())
            {
                Debug.LogError("Starting Date not set");
                StartingDate = new Date(1,Month.January,1,1,1);
            }
            CurrentDate = StartingDate;
            _lastProcessedDate = CurrentDate;
            TimeManager.Initialize(TickRate);
            OnNewDay += DestroyPassedEventsEffects;
            


        }

        private void OnDestroy()
        {
            TimeManager.OnTick -= Tick;
        }

        /// <summary>
        /// Retrieves a calendar by its name.
        /// </summary>
        /// <param name="calenderName">The name of the calendar to retrieve.</param>
        /// <returns>The calendar with the specified name, or null if not found.</returns>
        public static MyCalendar GetCalender(string calenderName)
        {
            foreach (MyCalendar calender in _calenders)
            {
                if (calender.CalenderName == calenderName)
                {
                    return calender;
                }
            }
            Debug.LogWarning("Calender not found");
            return null;
        }

        /// <summary>
        /// Creates a new calendar with the specified name.
        /// </summary>
        /// <param name="calenderName">The name of the calendar to create.</param>
        /// <returns>The newly created calendar.</returns>
        public static MyCalendar CreateCalender(string calenderName)
        {
            GameObject obj = new GameObject(calenderName);
            MyCalendar myCalender = obj.AddComponent<MyCalendar>();

            return myCalender;
        }

        /// <summary>
        /// Displays the specified calendar using the calendar UI prefab.
        /// </summary>
        /// <param name="calender">The calendar to display.</param>
        public static void DisplayCalender(MyCalendar calender)
        {
            if (Instance.CalenderUIPrefab == null)
            {
                Debug.LogError("Calender UI Prefab is not set in the Calender Manager");
                return;
            }
            if (FindObjectOfType<CalendarUI>() != null)
            {
                Debug.LogWarning("Calender UI already exists in the scene");
                return;
            }
            GameObject calUI = Instantiate(Instance.CalenderUIPrefab);
            calUI.name = calender.CalenderName + "UI";
            calUI.GetComponentInChildren<CalendarUI>().SetCalender(calender);
        }

        /// <summary>
        /// Adds a calendar to the manager.
        /// </summary>
        /// <param name="calender">The calendar to add.</param>
        public static void AddCalender(MyCalendar calender)
        {
            if (_calenders.Contains(calender))
            {
                Debug.LogWarning("Calender already exists in the manager");
                return;
            }
            _calenders.AddLast(calender);
            SortEvents();
        }

        /// <summary>
        /// Sorts all events from all calendars by date.
        /// </summary>
        public static void SortEvents()
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

        /// <summary>
        /// Pauses or resumes the time progression.
        /// </summary>
        /// <param name="isPaused">True to pause time, false to resume.</param>
        public static void SetPause(bool isPaused)
        {
            TimeManager.isPaused = isPaused;
        }

        /// <summary>
        /// Sets the time step in minutes for advancing time.
        /// </summary>
        /// <param name="minutes">The number of minutes to advance each tick.</param>
        public static void SetTimeStep(int minutes)
        {
            Instance.TimeStepInMinutes = minutes;
        }

        /// <summary>
        /// Sets the tick rate for time progression.
        /// </summary>
        /// <param name="rate">The new tick rate.</param>
        public static void SetTickRate(float rate)
        {
            TimeManager.SetTickRate(rate);
        }

        /// <summary>
        /// Advances the current time by the specified time step.
        /// </summary>
        public void AdvanceCurrentTime()
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

        /// <summary>
        /// Smoothly transitions the sun's intensity over a duration.
        /// </summary>
        /// <param name="start">The starting intensity.</param>
        /// <param name="end">The target intensity.</param>
        /// <param name="duration">The duration over which to transition.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
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

        /// <summary>
        /// Advances time and processes events each tick.
        /// </summary>
        private void Tick()
        {
            AdvanceCurrentTime();

            PrepareEventsForToday();

            ProcessTodayEvents();

            RemoveOldEvents();
        }

        /// <summary>
        /// Removes old events that have already passed.
        /// </summary>
        private void RemoveOldEvents()
        {
            if (_passedEvents.Count == 0)
                return;

            foreach (var e in _passedEvents)
            {
                if (_eventsToday.ContainsKey(e.startingDate))
                {
                    _eventsToday.Remove(e.startingDate);
                }
            }
        }

        /// <summary>
        /// Destroys the effects of passed events on a specific date.
        /// </summary>
        /// <param name="date">The date for which to destroy passed event effects.</param>
        void DestroyPassedEventsEffects(Date date)
        {
            foreach (var Event in _passedEvents)
            {
                foreach (var effect in Event.effectScripts)
                {
                    Destroy(effect.gameObject);
                }
                Event.effectScripts.Clear();
            }
        }

        /// <summary>
        /// Prepares the events scheduled for the current day.
        /// </summary>
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
                    foreach (GameObject EffectGameObjectPrefab in upcomingEvent.Value.EventEffectsPrefabs)
                    {
                        var effect = Instantiate(EffectGameObjectPrefab, Vector3.zero, Quaternion.identity);
                        effect.name = $"{upcomingEvent.Value.EventName}'s {EffectGameObjectPrefab.name} effect";
                        effect.SetActive(false);
                        upcomingEvent.Value.effectScripts.AddLast(effect.GetComponent<EffectScript>());
                    }

                    _eventsToday.Add(upcomingEvent.Key, upcomingEvent.Value);

                }
                else if (upcomingEvent.Key > CurrentDate)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Processes the events scheduled for today.
        /// </summary>
        private void ProcessTodayEvents()
        {
            foreach (var Event in _eventsToday)
            {
                if (CurrentDate >= Event.Key)
                {
                    Event.Value.TriggerEvent();
                    _passedEvents.AddLast(Event.Value);
                }
            }
        }
    }
}

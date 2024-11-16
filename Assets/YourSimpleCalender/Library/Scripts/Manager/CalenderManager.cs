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
        private static LinkedList<Event> _passedEvents = new LinkedList<Event>();
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

        [SerializeField] public GameObject CalenderUIPrefab;
        
        
        [Space][Header("Time Settings")]
        [HideInInspector]public int TimeStepInMinutes = 1;
        [HideInInspector]public float TickRate = 1;
        [SerializeField] Light2D sun; 
        [Space]


        [HideInInspector] public bool IsMilitaryTime = false;

        public static MyCalender GetCalender(string calenderName)
        {
            foreach (MyCalender calender in _calenders)
            {
                if (calender.CalenderName == calenderName)
                {
                    return calender;
                }
            }
            Debug.LogWarning("Calender not found");
            return null;
        }

        public static MyCalender CreateCalender(string calenderName)
        {
            GameObject obj = new GameObject(calenderName);
            MyCalender myCalender = obj.AddComponent<MyCalender>();

            return myCalender;

        }

        public static void DisplayCalender(MyCalender calender)
        {
            if (Instance.CalenderUIPrefab == null)
            {
                Debug.LogError("Calender UI Prefab is not set in the Calender Manager");
                return;
            }
            if (FindObjectOfType<CalenderUI>() != null)
            {
                Debug.LogWarning("Calender UI already exists in the scene");
                return;
            }
            GameObject calUI = Instantiate(Instance.CalenderUIPrefab);
            calUI.name = calender.CalenderName + "UI";
            calUI.GetComponentInChildren<CalenderUI>().SetCalender(calender);

        }

        public static void AddCalender(MyCalender calender)
        {
            if (_calenders.Contains(calender))
            {
                Debug.LogWarning("Calender already exists in the manager");
                return;
            }
            _calenders.AddLast(calender);
            SortEvents();

        }

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


        

        public static void SetPause(bool isPaused)
        {
            TimeManager.isPaused = isPaused;
        }

        public static void SetTimeStep(int minutes)
        {
            Instance.TimeStepInMinutes = minutes;
        }

        public static void SetTickRate(float rate)
        {
            TimeManager.SetTickRate(rate);
        }
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
            if(_passedEvents.Count == 0)
                return;

                
            foreach (var e in _passedEvents)
            {
                if (_eventsToday.ContainsKey(e.startingDate))
                {
                    _eventsToday.Remove(e.startingDate);
                }
            }
        }

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

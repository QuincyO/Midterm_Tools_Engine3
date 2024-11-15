using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Quincy.Calender
{


    
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance{get; private set;}


        public static bool Initialize(float tickRate = 1.0f)
        {
            if (Instance == null)
            {
                GameObject timeManager = new GameObject("TimeManager");
            
                timeManager.hideFlags = HideFlags.HideAndDontSave;
            
                Instance = timeManager.AddComponent<TimeManager>();
            
                DontDestroyOnLoad(timeManager);

                TickRate = tickRate;
                return true;
            }
            else
            {
                Debug.LogWarning("TimeManager already initialized");
                return false;
            }
        }

        /// <summary>
        /// Time In Seconds for next Tick
        /// </summary>
        public static float TickRate{get; private set;}
        
        /// <summary>
        /// The Tick Rate is the time in seconds before the Timer Advances forward
        /// </summary>
        /// <param name="tickRate"></param>
        public static void SetTickRate(float tickRate)
        {
            TickRate = tickRate;
        }

        
        
        public static bool isPaused = false;
        private float timeSinceLastTick = 0;
        public static event Action OnTick;

        // Update is called once per frame
        void Update()
        {
            timeSinceLastTick += Time.deltaTime;
            if (timeSinceLastTick >= TickRate && !isPaused)
            {
                
               OnTick?.Invoke();
               timeSinceLastTick -= TickRate;
            }
        }
    }

}

using System.Collections;
using System;
using System.Collections.Generic;
using Quincy.Collections.Generic;
using Quincy.Calender;
using UnityEngine;


namespace Quincy.Calender
{
    public partial class CalenderManager : MonoBehaviour
    {


        private List<MyCalender> _calenders;
         
        static SortedList<Date,List<Event>> _events;

 
        
         
         private Date _currentDate;
    

        public static MyCalender CreateCalender(string calenderName,Date currentDate)
        {
            GameObject obj = new GameObject(calenderName);
            
            MyCalender myCalender = obj.AddComponent<MyCalender>();
            
            
            return myCalender;
        }

        public static void SetPause(bool isPaused)
        {
            TimeManager.isPaused = isPaused;
        }

        private static void AdvanceDate()
        {
            //todo: Create a Modular Arithmetic Rollover effect function AddMinute(int minsToAdd),AddHour(int hoursToAdd),etc. That function will be in Date.cs. 
            _currentDate++;
        }

        private static void Tick()
        {
            AdvanceDate();
          //  if (_currentDate == calenders)
        }
        
        
    }

}

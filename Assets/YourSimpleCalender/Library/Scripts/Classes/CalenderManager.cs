using System.Collections;
using System.Collections.Generic;
using Quincy.Calender;
using UnityEngine;


namespace Quincy.Calender
{
    public class CalenderManager : MonoBehaviour
    {

         private List<MyCalender> _calenders;
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
        
        
        
        
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Quincy.Calender
{
    [CreateAssetMenu(fileName = "New Scriptable Object Event", menuName = "Scriptable Object Event")]
    public class ScriptableObjectEvent : ScriptableObject
    {
        public string eventName;
        public Color eventColor;
        public List<DateScriptableObject> eventDates;
        //TODO: I need to finish creating a Scriptable object for List<events>, and on Calender Awake it will add it to the list of Events and orders it in chronological  

        
        public UnityEvent OnEvent;
    }


}

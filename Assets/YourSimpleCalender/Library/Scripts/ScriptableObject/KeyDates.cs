using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Quincy.Calender
{
    [CreateAssetMenu(fileName = "New Scriptable Object Event", menuName = "Scriptable Object Event")]
    public class KeyDate : ScriptableObject
    {
        public string eventName;
        public Color eventColor = Color.white;

        public Date StartDate;
    }
    
}

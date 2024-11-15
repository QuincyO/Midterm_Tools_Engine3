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

        public Sprite eventIcon;

        public List<GameObject> eventEffects = new List<GameObject>();

        private void OnValidate() {
            if (eventEffects.Count == 0)
            {
                return;
            }

            for(int i = 0; i < eventEffects.Count;i++)
            {
                if (eventEffects[i] == null) {continue;}
                if (!eventEffects[i].TryGetComponent<EffectScript>(out var effect)) {
                    Debug.LogWarning("Effect Script not found on " + eventEffects[i].name + " Removing from list");
                    eventEffects.Remove(eventEffects[i]);
                    i--;
            }
        }
    }
    
    }
}

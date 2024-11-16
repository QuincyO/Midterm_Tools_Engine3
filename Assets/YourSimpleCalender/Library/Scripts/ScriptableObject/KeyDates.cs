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

        public List<GameObject> eventEffectsPrefab = new List<GameObject>();


        private void OnValidate() {
            if (eventEffectsPrefab.Count == 0)
            {
                return;
            }

            for(int i = 0; i < eventEffectsPrefab.Count;i++)
            {
                if (eventEffectsPrefab[i] == null) {continue;}
                if (!eventEffectsPrefab[i].TryGetComponent<EffectScript>(out var effect)) {

                    Debug.LogWarning("Effect Script not found on " + eventEffectsPrefab[i].name + " Removing from list");
                    eventEffectsPrefab.Remove(eventEffectsPrefab[i]);
                    i--;
            }
        }
    }
    }
}

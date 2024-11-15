using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event Effect", menuName = "Event Effect")]
public class Effect : ScriptableObject
{

    [SerializeField] public GameObject EffectPrefab;
    public void TriggerEffect(){
        if (EffectPrefab.TryGetComponent<EffectScript>(out var effect)) {
            effect.TriggerEffect();
        }
    }
}

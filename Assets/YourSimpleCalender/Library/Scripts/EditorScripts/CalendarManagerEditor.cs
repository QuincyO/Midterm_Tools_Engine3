using Quincy.Calender;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CalenderManager))]
[CanEditMultipleObjects] // Enables multi-object editing
public class CalendarManagerEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Time Manager Settings", EditorStyles.boldLabel);
        GUIContent tickRateLabel = new GUIContent("Tick Rate", "Sets how often the Time Manager updates (in seconds).");
        float newTickRate = EditorGUILayout.FloatField(tickRateLabel, TimeManager.TickRate);

       if (newTickRate != TimeManager.TickRate)
       {
           TimeManager.SetTickRate(newTickRate);
          // EditorUtility.SetDirty(target);
       }
    }
}
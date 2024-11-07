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


        GUIContent tickRateLabel = new GUIContent("Tick Rate Per Second", "Sets how often the Time Manager updates (in seconds).");

        GUIContent IncrementAmount = new GUIContent("Minutes To Tick", "Sets how many minutes to increment the time by");

        int newIncrementAmount = EditorGUILayout.IntField(IncrementAmount, CalenderManager.IncrementAmount);





        if (newIncrementAmount != CalenderManager.IncrementAmount)
        {
            CalenderManager.IncrementAmount = newIncrementAmount;
        }

        float newTickRate = EditorGUILayout.FloatField(tickRateLabel, TimeManager.TickRate);

       if (newTickRate != TimeManager.TickRate)
       {
           TimeManager.SetTickRate(newTickRate);
       }
           EditorUtility.SetDirty(target);
    }
}
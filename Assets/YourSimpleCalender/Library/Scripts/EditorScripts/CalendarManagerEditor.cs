using Quincy.Calender;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CalenderManager))]
[CanEditMultipleObjects]
public class CalendarManagerEditor : Editor
{
    //private bool showDateSettings = true;
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        #region Calendar Manager Settings
        // EditorGUILayout.Space(10);
        // EditorGUILayout.LabelField("Calendar Manager Settings", EditorStyles.boldLabel);
        
        // // Date Settings Foldout
        // EditorGUI.indentLevel++;
        // showDateSettings = EditorGUILayout.Foldout(showDateSettings, "Starting Date Settings", true);
        
        // if (showDateSettings)
        // {
        //     EditorGUI.indentLevel++;
        //     EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
        //     Date startingDate = CalenderManager.StartingDate;
        //     startingDate.isMilitaryTime = true;
            
        //     // Date Fields
        //     EditorGUILayout.BeginVertical();
        //     startingDate.Year = EditorGUILayout.IntField("Year", startingDate.Year);
        //     startingDate.Month = (Month)EditorGUILayout.EnumPopup("Month", startingDate.Month);
        //     startingDate.Day = EditorGUILayout.IntField("Day", startingDate.Day);
            
        //     // Time Fields
        //     EditorGUILayout.Space(5);
        //     startingDate.Hours = EditorGUILayout.IntSlider("Hour", startingDate.Hours, 0, 23);
        //     startingDate.Minutes = EditorGUILayout.IntSlider("Minute", startingDate.Minutes, 0, 59);
        //     EditorGUILayout.EndVertical();
            
        //     if(GUI.changed && startingDate != CalenderManager.StartingDate)
        //     {
        //         CalenderManager.StartingDate = startingDate;
        //     }
            
        //     EditorGUILayout.EndVertical();
        //     EditorGUI.indentLevel--;
        // }
        // EditorGUI.indentLevel--;
        #endregion

    //     #region Time Manager Settings
    //     EditorGUILayout.Space(10);
    //     EditorGUILayout.LabelField("Time Manager Settings", EditorStyles.boldLabel);

    //     GUIContent tickRateLabel = new GUIContent("Tick Rate Per Second", "Sets how often the Time Manager updates (in seconds).");

    //     GUIContent IncrementAmount = new GUIContent("Minutes To Tick", "Sets how many minutes to increment the time by");

    //     int newIncrementAmount = EditorGUILayout.IntField(IncrementAmount, CalenderManager.IncrementAmount);

    //     if (newIncrementAmount != CalenderManager.IncrementAmount)
    //     {
    //         CalenderManager.IncrementAmount = newIncrementAmount;
    //     }

    //     float newTickRate = EditorGUILayout.FloatField(tickRateLabel, TimeManager.TickRate);

    //    if (newTickRate != TimeManager.TickRate)
    //    {
    //        TimeManager.SetTickRate(newTickRate);
    //    }
    //        EditorUtility.SetDirty(target);
    //     #endregion
    }
}
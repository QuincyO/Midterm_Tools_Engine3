using UnityEditor;
using UnityEngine;
using Quincy.Calender; // Ensure this matches your namespace

[CustomEditor(typeof(CalendarManager))]
[CanEditMultipleObjects]
public class CalendarManagerEditor : Editor
{
    SerializedProperty tickRateProp;
    SerializedProperty timeStepProp;

    private void OnEnable()
    {
        // Link SerializedProperties to their corresponding fields in the script
        tickRateProp = serializedObject.FindProperty("TickRate");
        timeStepProp = serializedObject.FindProperty("TimeStepInMinutes");
    }

public override void OnInspectorGUI()
{
    base.OnInspectorGUI();
    serializedObject.Update();

    EditorGUILayout.LabelField("Time Control Settings", EditorStyles.boldLabel);

    EditorGUI.BeginChangeCheck();

    EditorGUILayout.PropertyField(tickRateProp, new GUIContent("Tick Rate"));
    EditorGUILayout.PropertyField(timeStepProp, new GUIContent("Time Step (Minutes)"));

    if (EditorGUI.EndChangeCheck())
    {
        serializedObject.ApplyModifiedProperties();

        // If the methods are static, call them with the class name
        foreach (Object obj in targets)
        {
            CalendarManager.SetTickRate(tickRateProp.floatValue);
            CalendarManager.SetTimeStep(timeStepProp.intValue);
        }
    }

    EditorGUILayout.Space();
}

}

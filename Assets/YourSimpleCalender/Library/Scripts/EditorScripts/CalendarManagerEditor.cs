using Quincy.Calender;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CalenderManager))]
[CanEditMultipleObjects]
public class CalendarManagerEditor : Editor
{
    private CalenderManager manager;
    private SerializedProperty startingDateProp;

    private void OnEnable()
    {
        manager = (CalenderManager)target;
        startingDateProp = serializedObject.FindProperty("StartingDate");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Calendar Status", EditorStyles.boldLabel);

        // Current Date Display
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Current Time:", EditorStyles.boldLabel);
            EditorGUILayout.TextField(manager.CurrentDate.ToString());
            EditorGUILayout.EndVertical();
        }

        if (Application.isPlaying)
        {
            Repaint();
        }
    }
}
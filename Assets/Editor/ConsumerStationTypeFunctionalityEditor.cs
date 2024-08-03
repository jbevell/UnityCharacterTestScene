using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConsumerStationTypeFunctionality))]
public class ConsumerStationTypeFunctionalityEditor : Editor
{
	SerializedProperty queueLimit;

	private void OnEnable()
	{
		queueLimit = serializedObject.FindProperty("QueueLimit");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		ConsumerStationTypeFunctionality script = (ConsumerStationTypeFunctionality)target;

		if (script.UseQueue)
		{
			serializedObject.Update();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Max Queue Limit");
			queueLimit.intValue = EditorGUILayout.IntField(queueLimit.intValue);
			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
		}
	}
}

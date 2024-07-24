using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConsumerStationTypeFunctionality))]
public class ConsumerStationTypeFunctionalityEditor : Editor
{ 
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		ConsumerStationTypeFunctionality script = (ConsumerStationTypeFunctionality)target;

		if (script.UseQueue)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Max Queue Limit");
			script.QueueLimit = EditorGUILayout.IntField(script.QueueLimit);
			EditorGUILayout.EndHorizontal();
		}
	}
}

using System;
using UnityEngine;
using UnityEditor;
using Kernel.UI;
using UnityEditor.AnimatedValues;

namespace KernelEditor.UI
{
	[CustomEditor(typeof(Form), true)]
	public class FormInspector : Editor
	{

		protected void OnEnable()
		{
		}

		protected void OnDisable()
		{
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (PrefabUtility.GetPrefabType(target) != PrefabType.Prefab)
			{
				if (Application.isPlaying)
				{
					var form = target as Form;

					EditorGUILayout.Space();
					EditorGUILayout.BeginVertical(EditorStyles.helpBox);
					GUILayout.Label("Debug", EditorStyles.boldLabel);

					EditorGUILayout.LabelField("Tweens", form.TweensCount.ToString());
					EditorGUILayout.LabelField("State", form.State.ToString());

					if (!form.IsOpen)
					{
						if (GUILayout.Button("Open"))
							form.Open();
					}
					else
					{
						if (GUILayout.Button("Close"))
							form.Close();
					}

					EditorGUILayout.EndVertical();

					// Repaint state
					Repaint();
				}
			}
		}
	}
}

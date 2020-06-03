#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace CustomScriptTemplate
{
	public class Namespaced : EditorWindow
	{
		private string _scriptName = "";

		[MenuItem("Assets/Create/C# Scripts/Namespaced", priority=82)]
		public static void ShowCreateScriptWindow()
		{
			var instance = ScriptableObject.CreateInstance<Namespaced>();
			instance.position = new Rect(Screen.width / 2.0f, Screen.height / 2.0f, 250, 80);
			instance.ShowPopup();
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Enter script name:");

			GUI.SetNextControlName("ScriptName");
			_scriptName = GUILayout.TextField(_scriptName);
			GUI.FocusControl("ScriptName");

			if (GUILayout.Button("Create"))
			{
				CustomScriptTemplate.CreateScriptFromTemplate(GetNewScriptPath(), _scriptName, "Namespaced");
				AssetDatabase.Refresh();
				Close();
			}

			if(Event.current != null && Event.current.isKey)
			{
				// Return or Enter event
				if(Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
				{
					CustomScriptTemplate.CreateScriptFromTemplate(GetNewScriptPath(), _scriptName, "Namespaced");
					AssetDatabase.Refresh();
					Close();
				}

				// Escape event
				if(Event.current.keyCode == KeyCode.Escape)
				{
					Close();
				}
			}
		}

		private string GetNewScriptPath()
		{
			return $"{AssetDatabase.GetAssetPath(Selection.activeObject)}";
		}
	}
}
#endif
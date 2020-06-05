#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CustomScriptTemplate
{
    public class BulkCreateEditor : EditorWindow
    {
        [MenuItem("Assets/Create/C# Scripts/Bulk Create", priority = 0)]
        public static void ShowBulkCreateEditor()
        {
            GetWindow<BulkCreateEditor>();
        }

        private int _selectedTemplateIndex = 0;
        private string _scriptsToCreate;
        private Vector2 _scrollPosition;

        public void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Select template:");

                var templates = CustomScriptTemplate.GetTemplateNames();
                _selectedTemplateIndex = EditorGUILayout.Popup(_selectedTemplateIndex, templates);

                EditorGUILayout.LabelField("Script names (one in each line):");
                _scriptsToCreate = EditorGUILayout.TextArea(_scriptsToCreate);

                EditorGUILayout.Separator();

                if(GUILayout.Button("Create"))
                {
                    if (templates.Length > 0)
                    {
                        var scriptNames = _scriptsToCreate.Split('\n');
                        foreach (var scriptName in scriptNames)
                        {
                            if (!string.IsNullOrEmpty(scriptName))
                            {
                                CustomScriptTemplate.CreateScriptFromTemplate(GetNewScriptPath(), scriptName, templates[_selectedTemplateIndex]);
                            }
                        }
                        AssetDatabase.Refresh();
                        Close();
                    }
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        private string GetNewScriptPath()
        {
            return $"{AssetDatabase.GetAssetPath(Selection.activeObject)}";
        }

    }
}
#endif
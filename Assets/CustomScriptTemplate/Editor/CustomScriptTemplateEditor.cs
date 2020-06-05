﻿using UnityEngine;
using UnityEditor;
using System.IO;

namespace CustomScriptTemplate
{
    public class CustomScriptTemplateEditor : EditorWindow
    {
        [MenuItem("Assets/Custom Script Templates")]
        public static void ShowWindow()
        {
            GetWindow<CustomScriptTemplateEditor>();
        }

        private string _templatesDirectory;

        private int _selectedScriptIndex;
        private string[] _scripts;

        private int _readScript = -1;
        private string _editScript;

        private string _createTemplateName = "";

        private string _namespaceRootDirectory = "";
        private string _namespacePrefix = "";

        private Vector2 _scrollPosition;

        private void Awake()
        {
            _templatesDirectory = CustomScriptTemplate.GetTemplatesDirectory();
            _namespaceRootDirectory = CustomScriptTemplate.GetNamespaceRootDir();
            _namespacePrefix = CustomScriptTemplate.GetNamespacePrefix();
        }

        void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.BeginVertical();
            {
                /* === TEMPLATES === */

                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Templates", new GUIStyle() { fontSize = 16 });

                FindTemplates();

                if (_scripts.Length > 0)
                {
                    _selectedScriptIndex = EditorGUILayout.Popup(_selectedScriptIndex, _scripts);

                    if (_scripts != null && _scripts.Length > 0 && _readScript != _selectedScriptIndex)
                    {
                        var selectedScriptName = _scripts[_selectedScriptIndex];
                        _editScript = File.ReadAllText(GetTemplatePath(selectedScriptName));
                        _readScript = _selectedScriptIndex;
                    }
                    _editScript = EditorGUILayout.TextArea(_editScript);

                    if (GUILayout.Button("Open in editor") && _scripts != null && _scripts.Length > 0)
                    {
                        System.Diagnostics.Process.Start($"{Directory.GetCurrentDirectory()}/{GetTemplatePath(_scripts[_selectedScriptIndex])}");
                    }

                    if (GUILayout.Button("Save") && _scripts != null && _scripts.Length > 0)
                    {
                        File.WriteAllText(GetTemplatePath(_scripts[_selectedScriptIndex]), _editScript);
                    }

                    if (GUILayout.Button("Delete") && _scripts != null && _scripts.Length > 0)
                    {
                        CustomScriptTemplate.DeleteTemplate(_scripts[_selectedScriptIndex]);
                        AssetDatabase.Refresh();

                        _selectedScriptIndex = 0;
                        _readScript = -1;
                        _editScript = "";
                    }
                }
                else
                {
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField("No templates created.");
                    EditorGUILayout.Separator();
                }

                /* === CREATE NEW TEMPLATE === */

                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Create new template", new GUIStyle() { fontSize = 16 });

                EditorGUILayout.LabelField("Template name:");
                _createTemplateName = EditorGUILayout.TextField(_createTemplateName);
                if (GUILayout.Button("Create"))
                {
                    CustomScriptTemplate.CreateTemplate(_createTemplateName);
                    AssetDatabase.Refresh();
                }

                /* === SETTINGS === */

                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Settings", new GUIStyle() { fontSize = 16 });
                EditorGUILayout.LabelField("Templates directory (relative to Assets):");
                var templatesDirectory = EditorGUILayout.TextField(_templatesDirectory);
                if (templatesDirectory != _templatesDirectory)
                {
                    _templatesDirectory = templatesDirectory;
                    CustomScriptTemplate.SetTemplatesDirectory(_templatesDirectory);
                }

                EditorGUILayout.LabelField("Namespace root directory (relative to Assets):");
                var namespaceRootDir = EditorGUILayout.TextField(_namespaceRootDirectory);
                if (namespaceRootDir != _namespaceRootDirectory)
                {
                    _namespaceRootDirectory = namespaceRootDir;
                    CustomScriptTemplate.SetNamespaceRootDir(_namespaceRootDirectory);
                }

                EditorGUILayout.LabelField("Namespace prefix:");
                var namespacePrefix = EditorGUILayout.TextField(_namespacePrefix);
                if (namespacePrefix != _namespacePrefix)
                {
                    _namespacePrefix = namespacePrefix;
                    CustomScriptTemplate.SetNamespacePrefix(_namespacePrefix);
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void FindTemplates()
        {
            if (!Directory.Exists($"Assets/{_templatesDirectory}"))
            {
                Directory.CreateDirectory($"Assets/{_templatesDirectory}");
            }

            var files = Directory.GetFiles($"Assets/{_templatesDirectory}", "*.txt");
            _scripts = new string[files.Length];

            for (var i = 0; i < files.Length; i++)
            {
                _scripts[i] = Path.GetFileNameWithoutExtension(files[i]);
            }
        }

        private string GetTemplatePath(string templateName)
        {
            return CustomScriptTemplate.GetTemplatePath(templateName);
        }
    }
}

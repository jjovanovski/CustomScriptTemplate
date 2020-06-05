using UnityEngine;
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

        private int _selectedTemplateIndex;

        private int _readTemplate = -1;
        private string _editTemplate;

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

                var templates = CustomScriptTemplate.GetTemplateNames();

                if (templates.Length > 0)
                {
                    _selectedTemplateIndex = EditorGUILayout.Popup(_selectedTemplateIndex, templates);

                    if (templates != null && templates.Length > 0 && _readTemplate != _selectedTemplateIndex)
                    {
                        var selectedScriptName = templates[_selectedTemplateIndex];
                        _editTemplate = File.ReadAllText(GetTemplatePath(selectedScriptName));
                        _readTemplate = _selectedTemplateIndex;
                    }
                    _editTemplate = EditorGUILayout.TextArea(_editTemplate);

                    if (GUILayout.Button("Open in editor") && templates != null && templates.Length > 0)
                    {
                        System.Diagnostics.Process.Start($"{Directory.GetCurrentDirectory()}/{GetTemplatePath(templates[_selectedTemplateIndex])}");
                    }

                    if (GUILayout.Button("Save") && templates != null && templates.Length > 0)
                    {
                        File.WriteAllText(GetTemplatePath(templates[_selectedTemplateIndex]), _editTemplate);
                    }

                    if (GUILayout.Button("Delete") && templates != null && templates.Length > 0)
                    {
                        CustomScriptTemplate.DeleteTemplate(templates[_selectedTemplateIndex]);
                        AssetDatabase.Refresh();

                        _selectedTemplateIndex = 0;
                        _readTemplate = -1;
                        _editTemplate = "";
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

        private string GetTemplatePath(string templateName)
        {
            return CustomScriptTemplate.GetTemplatePath(templateName);
        }
    }
}

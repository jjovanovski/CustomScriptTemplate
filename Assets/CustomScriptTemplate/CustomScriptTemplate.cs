#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CustomScriptTemplate
{
    public class CustomScriptTemplate
    {
        public const string TEMPLATES_DIRECTORY_KEY = "templatesDirectory";
        public const string DEFAULT_TEMPLATES_DIRECTORY = "ScriptTemplates";

        public const string NAMESPACE_ROOT_DIR_KEY = "namespaceRootDir";
        public const string DEFAULT_NAMESPACE_ROOT_DIR = "Scripts";

        public const string NAMESPACE_PREFIX_KEY = "namespacePrefix";
        public const string DEFAULT_NAMESSPACE_PREFIX = "Company.Product";

        public const string CLASS_NAME_WILDCARD = "@{CLASS_NAME}@";
        public const string NAMESPACE_WILDCARD = "@{NAMESPACE}@";
        public const string NAMESPACE_PREFIX_WILDCARD = "@{NAMESPACE_PREFIX}@";

        public static void CreateTemplate(string templateName)
        {
            // create the template file
            File.WriteAllText(GetTemplatePath(templateName), GetDefaultTemplateContent());

            // create the menu script
            var menuScriptContent = GetMenuScriptTemplateContent();
            var className = templateName.Replace(".txt", "");
            menuScriptContent = menuScriptContent.Replace(CLASS_NAME_WILDCARD, className);
            File.WriteAllText(GetMenuScriptPath(templateName), menuScriptContent);
        }

        public static void CreateScriptFromTemplate(string scriptPath, string className, string templateName)
        {
            className = className.Trim();
            if (className.EndsWith(".cs"))
                className = className.Substring(0, className.Length - 3);

            var namespacePrefix = GetNamespacePrefix();
            var namespaceForPath = GetNamespaceForPath(scriptPath);
            if (!string.IsNullOrEmpty(namespacePrefix) && !string.IsNullOrEmpty(namespaceForPath))
                namespacePrefix += '.';

            var templateContent = File.ReadAllText(GetTemplatePath(templateName));
            templateContent = templateContent.Replace(CLASS_NAME_WILDCARD, className);
            templateContent = templateContent.Replace(NAMESPACE_WILDCARD, namespaceForPath);
            templateContent = templateContent.Replace(NAMESPACE_PREFIX_WILDCARD, namespacePrefix);

            File.WriteAllText(Path.Combine(scriptPath, $"{className}.cs"), templateContent);
        }

        public static void EditTemplate(string templateName, string content)
        {
            File.WriteAllText(GetTemplatePath(templateName), content);
        }

        public static void DeleteTemplate(string templateName)
        {
            File.Delete(GetTemplatePath(templateName));
            File.Delete(GetMenuScriptPath(templateName));
        }

        public static string GetTemplatesDirectory()
        {
            return EditorPrefs.GetString(TEMPLATES_DIRECTORY_KEY, DEFAULT_TEMPLATES_DIRECTORY);
        }

        public static void SetTemplatesDirectory(string directory)
        {
            EditorPrefs.SetString(TEMPLATES_DIRECTORY_KEY, directory);
        }

        public static string GetNamespaceRootDir()
        {
            return EditorPrefs.GetString(NAMESPACE_ROOT_DIR_KEY, DEFAULT_NAMESPACE_ROOT_DIR);
        }

        public static void SetNamespaceRootDir(string rootDir)
        {
            EditorPrefs.SetString(NAMESPACE_ROOT_DIR_KEY, rootDir);
        }

        public static string GetNamespacePrefix()
        {
            return EditorPrefs.GetString(NAMESPACE_PREFIX_KEY, DEFAULT_NAMESSPACE_PREFIX);
        }

        public static void SetNamespacePrefix(string prefix)
        {
            EditorPrefs.SetString(NAMESPACE_PREFIX_KEY, prefix);
        }

        public static string GetTemplatePath(string templateName)
        {
            return $"Assets/{GetTemplatesDirectory()}/{templateName}.txt";
        }

        public static string GetMenuScriptPath(string templateName)
        {
            return $"Assets/{GetTemplatesDirectory()}/{templateName}_MenuScript.cs";
        }

        private static string GetDefaultTemplateContent()
        {
            var defaultScriptTemplateAsset = Resources.Load<TextAsset>("DefaultScriptTemplate");
            return defaultScriptTemplateAsset.text;
        }

        private static string GetMenuScriptTemplateContent()
        {
            var menuScriptTemplateAsset = Resources.Load<TextAsset>("MenuScriptTemplate");
            return menuScriptTemplateAsset.text;
        }

        private static string GetNamespaceForPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var rootDir = Path.Combine("Assets", GetNamespaceRootDir());
            if (string.IsNullOrEmpty(rootDir))
                return path.Trim('\\', '/', '.').Replace('\\', '.').Replace('/', '.');

            var pathAbs = Path.GetFullPath(path);
            var rootDirAbs = Path.GetFullPath(rootDir);
            Debug.Log(pathAbs);
            Debug.Log(rootDirAbs);
            if (pathAbs.StartsWith(rootDirAbs))
            {
                path = pathAbs.Substring(rootDirAbs.Length);
            }

            return path.Trim('\\', '/', '.').Replace('\\', '.').Replace('/', '.');
        }
    }
}
#endif
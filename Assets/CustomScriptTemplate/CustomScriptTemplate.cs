#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CustomScriptTemplate
{
    public const string TEMPLATES_DIRECTORY_KEY = "templatesDirectory";
    public const string DEFAULT_TEMPLATES_DIRECTORY = "ScriptTemplates";

    public const string CLASS_NAME_WILDCARD = "@{CLASS_NAME}@";

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

        var templateContent = File.ReadAllText(GetTemplatePath(templateName));
        templateContent = templateContent.Replace(CLASS_NAME_WILDCARD, className);

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
}
#endif
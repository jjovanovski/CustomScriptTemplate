using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomScriptTemplate
{
    [CreateAssetMenu(fileName = "CustomScriptTemplateSettings", menuName = "Create Custom Script Template Settings", order = int.MaxValue)]
    public class CustomScriptTemplateSettings : ScriptableObject
    {
        public string TemplatesDirectory = CustomScriptTemplate.DEFAULT_TEMPLATES_DIRECTORY;

        public string NamespaceRootDirectory = CustomScriptTemplate.DEFAULT_NAMESPACE_ROOT_DIR;

        public string NamespacePrefix = CustomScriptTemplate.DEFAULT_NAMESSPACE_PREFIX;
    }
}

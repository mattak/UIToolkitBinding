using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace UIToolkitBinding.EditorRuntime
{
    public static class BindingGenerator
    {
        private const string TemplateFilePath = "Packages/me.mattak.uitoolkitbinding/Editor/Templates/BindingTemplate.txt";

        public static void GenerateAll(string writeDirectory, string @namespace)
        {
            var files = AssetDatabase.FindAssets("", new[] {"Assets"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(x => x.EndsWith(".uxml"));

            foreach (var readFilePath in files)
            {
                var writeFilePath = Generate(writeDirectory, @namespace, readFilePath);
                Debug.Log($"Generated: {readFilePath} -> {writeFilePath}");
                AssetDatabase.ImportAsset(writeFilePath);
            }
        }

        public static string Generate(string writeDirectory, string @namespace, string uxmlFilePath)
        {
            var basename = Path.GetFileNameWithoutExtension(uxmlFilePath);
            var className = basename.ToUpperCamel() + "Binding";

            var uxmlFileContent = File.ReadAllText(uxmlFilePath);
            var nameMap = ParseNameMap(uxmlFileContent);

            var info = new BindingGenerateInfo(@namespace, className, nameMap, uxmlFilePath);
            var content = GenerateFileContent(info, TemplateFilePath);
            var writeFilePath = $"{writeDirectory}/{className}.cs";

            if (!Directory.Exists(writeDirectory)) Directory.CreateDirectory(writeDirectory);
            File.WriteAllText(writeFilePath, content);
            return writeFilePath;
        }

        public static string GenerateFileContent(BindingGenerateInfo info, String templateFile)
        {
            var template = File.ReadAllText(templateFile);
            var properties = string.Join("\n", info.NameMap
                .Select(kv =>
                {
                    var name = kv.Key.ToUpperCamel();
                    var uiType = kv.Value.ToString();
                    return $"        public {uiType} {name} {{ get; }}";
                })
            );
            var initializerQueries = string.Join("\n", info.NameMap
                .Select(kv =>
                {
                    var name = kv.Key.ToUpperCamel();
                    var uiType = kv.Value.ToString();
                    return $"            this.{name} = rootVisualElement.Q<{uiType}>(\"{name}\");";
                })
            );

            var result = template
                    .Replace("#NAMESPACE#", info.Namespace)
                    .Replace("#CLASS_NAME#", info.ClassName)
                    .Replace("#SOURCE_LAYOUT_PATH#", info.SourceLayoutPath)
                    .Replace("#PROPERTIES#", properties)
                    .Replace("#INITIALIZER_QUERIES#", initializerQueries)
                ;
            return result;
        }

        public static IDictionary<string, UIElementType> ParseNameMap(string xmlText)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(new StringReader(xmlText));

            var result = new Dictionary<string, UIElementType>();
            var root = xmlDocument.FirstChild;
            WalkTree(root.ChildNodes, node =>
            {
                var uiElementType = UIElementTypeCreator.ParseByNodeName(node.Name);
                if (uiElementType.HasValue)
                {
                    var nodeName = FindAttribute(node.Attributes, "name");
                    if (nodeName != null && !nodeName.StartsWith("_"))
                    {
                        result[nodeName] = uiElementType.Value;
                    }
                }
            });

            return result;
        }

        private static void WalkTree(XmlNodeList children, Action<XmlNode> walker)
        {
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                walker(child);
                WalkTree(child.ChildNodes, walker);
            }
        }

        private static string FindAttribute(XmlAttributeCollection attributes, string key)
        {
            for (var i = 0; i < attributes.Count; i++)
            {
                var attr = attributes[i];
                if (attr.Name == key)
                {
                    return attr.Value;
                }
            }

            return null;
        }
    }
}
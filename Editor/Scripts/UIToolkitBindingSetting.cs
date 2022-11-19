using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UIToolkitBinding.EditorRuntime
{
    [CreateAssetMenu(fileName = "UIToolkitBindingSetting", menuName = "UIToolkitBinding/Setting")]
    [Serializable]
    public class UIToolkitBindingSetting : ScriptableObject
    {
        public bool IsEnableAssetPostprocessor;
        public string Namespace;
        public string WriteDirectory;

        private void Reset()
        {
            this.IsEnableAssetPostprocessor = true;
            this.Namespace = "DefaultBinding";
            this.WriteDirectory = "Assets/Scripts/Bindings";
        }

        public static UIToolkitBindingSetting LoadFromAsset()
        {
            var scriptableObject = AssetDatabase.FindAssets("t:ScriptableObject", new[] {"Assets"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath<UIToolkitBindingSetting>(path))
                .FirstOrDefault(x => x != null);
            if (scriptableObject != null) return scriptableObject;
            return null;
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace UIToolkitBinding.EditorRuntime
{
    public class UIToolkitBindingPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            if (importedAssets.Length > 0)
            {
                var path = importedAssets[0];
                if (path.EndsWith(".uxml") && path.StartsWith("Assets"))
                {
                    var uxmlFilePath = importedAssets[0];
                    Update(uxmlFilePath);
                }
            }
        }

        private static void Update(string uxmlFilePath)
        {
            var settings = UIToolkitBindingSetting.LoadFromAsset();
            if (!settings.IsEnableAssetPostprocessor) return;

            var bindingFilePath = BindingGenerator.Generate(settings.WriteDirectory, settings.Namespace, uxmlFilePath);
            Debug.Log($"GenerateBinding: {uxmlFilePath} -> {bindingFilePath}");
        }
    }
}
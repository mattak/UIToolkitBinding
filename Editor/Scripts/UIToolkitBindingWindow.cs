using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UIToolkitBinding.EditorRuntime
{
    public class UIToolkitBindingWindow : EditorWindow
    {
        [MenuItem("Window/UIToolkitBinding")]
        public static void ShowWindow()
        {
            GetWindow<UIToolkitBindingWindow>("UIToolkitBinding");
        }

        private void OnEnable()
        {
            Bind(rootVisualElement);
        }

        private void Bind(VisualElement rootVisualElement)
        {
            var uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Packages/me.mattak.uitoolkitbinding/Editor/Layout/UIToolkitBindingWindow.uxml"
            );
            var ui = uiAsset.CloneTree();
            rootVisualElement.Clear();
            rootVisualElement.Add(ui);

            var generateAllButton = rootVisualElement.Q<Button>("GenerateAllButton");
            var createSettingButton = rootVisualElement.Q<Button>("CreateSettingButton");
            var settingsInspector = rootVisualElement.Q<InspectorElement>("SettingsInspector");

            var serializedObject = FindScriptableObjectSetting();

            if (serializedObject != null)
            {
                generateAllButton.visible = true;
                generateAllButton.style.display = DisplayStyle.Flex;
                settingsInspector.visible = true;
                settingsInspector.style.display = DisplayStyle.Flex;
                createSettingButton.visible = false;
                createSettingButton.style.display = DisplayStyle.None;
                settingsInspector.Bind(serializedObject);
                generateAllButton.clicked += OnClickGenerateAll;
            }
            else
            {
                generateAllButton.visible = false;
                generateAllButton.style.display = DisplayStyle.None;
                settingsInspector.visible = false;
                settingsInspector.style.display = DisplayStyle.None;
                createSettingButton.visible = true;
                createSettingButton.style.display = DisplayStyle.Flex;
                createSettingButton.clicked += OnClickCreateSetting;
            }
        }

        private static void OnClickGenerateAll()
        {
            var settings = UIToolkitBindingSetting.LoadFromAsset();
            BindingGenerator.GenerateAll(settings.WriteDirectory, settings.Namespace);
        }

        private static void OnClickCreateSetting()
        {
            var so = CreateInstance<UIToolkitBindingSetting>();
            var path = "Assets/UIToolkitBindingSettings.asset";
            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = so;
        }

        private static SerializedObject FindScriptableObjectSetting()
        {
            var scriptableObject = UIToolkitBindingSetting.LoadFromAsset();
            if (scriptableObject != null) return new SerializedObject(scriptableObject);
            return null;
        }
    }
}
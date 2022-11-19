namespace UIToolkitBinding.EditorRuntime
{
    public enum UIElementType
    {
        VisualElement,
        Image,
        Button,
        TextField,
        Toggle,
        Label,
    }

    public static class UIElementTypeCreator
    {
        public static UIElementType? ParseByNodeName(string rawName)
        {
            switch (rawName)
            {
                case "ui:VisualElement": return UIElementType.VisualElement;
                case "ui:Image": return UIElementType.Image;
                case "ui:Button": return UIElementType.Button;
                case "ui:TextField": return UIElementType.TextField;
                case "ui:Toggle": return UIElementType.Toggle;
                case "ui:Label": return UIElementType.Label;
                default: return null;
            }
        }
    }
}
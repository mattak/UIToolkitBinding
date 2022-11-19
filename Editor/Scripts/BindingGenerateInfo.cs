using System.Collections.Generic;

namespace UIToolkitBinding.EditorRuntime
{
    public class BindingGenerateInfo
    {
        public string Namespace { get; }
        public string ClassName { get; }
        public IDictionary<string, UIElementType> NameMap { get; }
        public string SourceLayoutPath { get; }

        public BindingGenerateInfo(string @namespace, string className, IDictionary<string, UIElementType> nameMap, string sourceLayoutPath)
        {
            this.Namespace = @namespace;
            this.ClassName = className;
            this.NameMap = nameMap;
            this.SourceLayoutPath = sourceLayoutPath;
        }
    }
}
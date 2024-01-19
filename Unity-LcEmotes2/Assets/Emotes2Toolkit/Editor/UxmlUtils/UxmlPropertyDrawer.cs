using UnityEditor;
using UnityEngine.UIElements;

namespace Emotes2Toolkit.Editor.UxmlUtils
{
    public abstract class UxmlPropertyDrawer : PropertyDrawer
    {
        protected abstract string UxmlPath { get; }

        private VisualTreeAsset _uxmlVisualTreeAsset;

        protected VisualTreeAsset UxmlVisualTree
        {
            get
            {
                if (!_uxmlVisualTreeAsset)
                    _uxmlVisualTreeAsset = LoadUxml();
                
                return _uxmlVisualTreeAsset;
            }
        }

        private VisualTreeAsset LoadUxml()
        {
            var path = $"Assets/Emotes2Toolkit/Editor/{UxmlPath}";

            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        }

        protected abstract VisualElement CreateCustomPropertyGUI(SerializedProperty property);

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            var customRoot = CreateCustomPropertyGUI(property);
            
            customRoot.BindFields(this);
            customRoot.BindButtonMethods(this);
            customRoot.BindOnBindListeners(this, property);
            
            root.Add(customRoot);

            return root;
        }
    }
}
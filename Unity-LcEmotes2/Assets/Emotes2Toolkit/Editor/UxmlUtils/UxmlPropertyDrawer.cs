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
    }
}
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Emotes2Toolkit.Editor.UxmlUtils
{
    public abstract class UxmlCustomEditor<TTarget> : UnityEditor.Editor
        where TTarget : Object
    {
        protected TTarget Target => (TTarget)target;
        
        protected abstract VisualElement CreateCustomEditorGUI();
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            var customRoot = CreateCustomEditorGUI();
            
            customRoot.BindFields(this);
            customRoot.BindButtonMethods(this);
            customRoot.BindOnBindListeners(this);
            
            root.Add(customRoot);

            return root;
        }
    }
}
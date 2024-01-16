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
            
            BindFields(customRoot);
            BindButtonMethods(customRoot);
            BindOnBindListeners(customRoot);
            
            root.Add(customRoot);

            return root;
        }

        private void BindFields(VisualElement root)
        {
            var fieldInfoArray =
                GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var fieldInfo in fieldInfoArray)
            {
                var attr = fieldInfo.GetCustomAttribute(typeof(UxmlBindValueAttribute));
                if (attr is not UxmlBindValueAttribute bindValueAttr)
                    continue;
                
                var element = GetFromPath(root, bindValueAttr.BindPath);
                if (element is null)
                {
                    Debug.LogWarning($"Couldn't bind an element with path '{bindValueAttr.BindPath}'!");
                    continue;
                }

                var fieldType = fieldInfo.FieldType;
                
                if (fieldType == typeof(string))
                    TryBindFieldValueListener<string>(fieldInfo, element, true);
                else if (fieldType == typeof(Color))
                    TryBindFieldValueListener<Color>(fieldInfo, element);
                else if (fieldType == typeof(bool))
                    TryBindFieldValueListener<bool>(fieldInfo, element);
            }
        }
        
        private void TryBindFieldValueListener<T>(FieldInfo fieldInfo, VisualElement element, bool setDefault = false)
        {
            if (element is not INotifyValueChanged<T> notifyValueChanged)
            {
                Debug.LogWarning($"{element.name} is not the correct INotifyValueChanged Type! Expected: '{typeof(T).Name}' Got: '{element.GetType()}'!");
                return;
            }

            if (setDefault)
                fieldInfo.SetValue(this, default);

            notifyValueChanged.RegisterValueChangedCallback(evt => fieldInfo.SetValue(this, evt.newValue));
        }

        private void BindButtonMethods(VisualElement root)
        {
            var methodInfoArray = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static |
                                                       BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var methodInfo in methodInfoArray)
            {
                var attr = methodInfo.GetCustomAttribute(typeof(UxmlBindButtonAttribute));
                if (attr is not UxmlBindButtonAttribute bindButtonAttr)
                    continue;
                
                var button = GetFromPath<Button>(root, bindButtonAttr.BindPath);
                if (button is null)
                {
                    Debug.LogWarning($"Couldn't bind an button with path '{bindButtonAttr.BindPath}'!");
                    continue;
                }
                
                button.RegisterCallback<MouseUpEvent>(_ => methodInfo.Invoke(this, Array.Empty<object>()));
            }
        }

        private void BindOnBindListeners(VisualElement root)
        {
            var methodInfoArray = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static |
                                                       BindingFlags.Public | BindingFlags.NonPublic);
            
            foreach (var methodInfo in methodInfoArray)
            {
                var attr = methodInfo.GetCustomAttribute(typeof(UxmlOnBindElementAttribute));
                if (attr is not UxmlOnBindElementAttribute onBindAttr)
                    continue;
                
                var element = GetFromPath(root, onBindAttr.BindPath);
                if (element is null)
                {
                    Debug.LogWarning($"Couldn't find an element with path '{onBindAttr.BindPath}'!");
                    continue;
                }

                methodInfo.Invoke(this, new object[] { element });
            }
        }

        private static VisualElement GetFromPath(VisualElement root, string path)
        {
            var selectors = path.Split('.', StringSplitOptions.RemoveEmptyEntries);

            if (selectors.Length == 0)
                return null;

            var element = root;
            foreach (var selector in selectors)
                element = element.Q(selector);

            return element;
        }

        private static T GetFromPath<T>(VisualElement root, string path)
            where T : VisualElement
        {
            return (T)GetFromPath(root, path);
        }
    }
}
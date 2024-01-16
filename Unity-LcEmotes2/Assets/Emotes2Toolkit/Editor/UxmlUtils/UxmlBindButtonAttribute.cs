using System;

namespace Emotes2Toolkit.Editor.UxmlUtils
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UxmlBindButtonAttribute : Attribute
    {
        public string BindPath { get; }

        public UxmlBindButtonAttribute(string bindPath)
        {
            BindPath = bindPath;
        }
    }
}
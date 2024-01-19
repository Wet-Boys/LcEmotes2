using System;

namespace Emotes2Toolkit.Editor.UxmlUtils
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UxmlOnBindElementAttribute : Attribute
    {
        public string BindPath { get; }

        public UxmlOnBindElementAttribute(string bindPath)
        {
            BindPath = bindPath;
        }
    }
}
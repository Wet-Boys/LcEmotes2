using System;

namespace Emotes2Toolkit.Editor.UxmlUtils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class UxmlBindValueAttribute : Attribute
    {
        public string BindPath { get; }
        
        public UxmlBindValueAttribute(string bindPath)
        {
            BindPath = bindPath;
        }
    }
}
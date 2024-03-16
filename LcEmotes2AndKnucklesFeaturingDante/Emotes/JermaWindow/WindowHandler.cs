using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow
{
    public class WindowHandler : MonoBehaviour
    {
        public static List<WindowHandler> allInactiveWindows = new List<WindowHandler>();
        public BoneMapper owner;
        void Start()
        {
            allInactiveWindows.Add(this);
        }
        void OnDestroy()
        {
            allInactiveWindows.Remove(this);
        }
    }
}

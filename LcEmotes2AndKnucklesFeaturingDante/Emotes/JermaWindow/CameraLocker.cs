using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow
{
    [DefaultExecutionOrder(-20)]
    internal class CameraLocker : MonoBehaviour
    {
        internal Camera cam;
        void LateUpdate()
        {
            cam.transform.localEulerAngles = Vector3.zero;
        }
    }
}

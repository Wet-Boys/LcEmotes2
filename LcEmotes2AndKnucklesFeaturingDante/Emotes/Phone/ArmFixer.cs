using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.Phone
{
    [DefaultExecutionOrder(3)]
    public class ArmFixer : MonoBehaviour
    {
        public Transform emoteArm;
        public Transform realArm;
        Vector3 desiredPos;
        Quaternion desiredRot;
        public void Setup(Transform emoteArm, Transform realArm)
        {
            this.emoteArm = emoteArm;
            this.realArm = realArm;
        }
        void Update()
        {
            desiredPos = realArm.position;
            desiredRot = realArm.rotation;
        }
        void LateUpdate()
        {
            emoteArm.position = desiredPos;
            emoteArm.rotation = desiredRot;
        }
    }
}

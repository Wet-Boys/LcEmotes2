using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.JermaWindow
{
    internal class WindowFallChecker : MonoBehaviour
    {
        internal BoneMapper mapper;
        internal Transform bodyPosition;
        bool falling = false;
        void Update()
        {
            bool wasFalling = falling;
            //just copying this from the unity docs/spectator camera KEKW
            Ray ray = new Ray(bodyPosition.position, new Vector3(0,-10,0));
            RaycastHit hit;//                       v PlayerControlerB.walkableSurfacesNoPlayersMask... but it's private and I don't feel like publicizing it lmao
            if (Physics.Raycast(ray, out hit, 10f, 268437761, QueryTriggerInteraction.Ignore))
            {
                if (wasFalling)
                {
                    DebugClass.Log($"was falling and now landing");
                    mapper.PlayAnim($"{LcEmotes2AndKnucklesFeaturingDantePlugin.ModGuid}__Window1Land", -1);
                }
            }
            else 
            {
                DebugClass.Log($"falling");

                if (!wasFalling)
                {
                    DebugClass.Log($"need to play falling animation");

                    mapper.preserveProps = true;
                    mapper.PlayAnim($"{LcEmotes2AndKnucklesFeaturingDantePlugin.ModGuid}__falling", -1);
                }
                falling = true;
            }
        }
    }
}

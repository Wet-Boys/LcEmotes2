using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.PropHunt
{
    internal class PropMover : MonoBehaviour
    {
        internal int speed;
        internal float delay;
        internal float maxHeight;
        void Update()
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
            }
            else if (transform.localPosition.y < maxHeight)
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0), Space.Self);
                if (transform.localPosition.y > maxHeight)
                {
                    transform.localPosition = new Vector3(0, maxHeight, 0);
                }
            }
        }
    }
}

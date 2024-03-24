using System;
using System.Collections.Generic;
using System.Text;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.LightsCameraAction
{
    internal class WatermarkCompaty
    {
        internal static bool CantHaveWatermark()
        {
            return LiquidStainRemoverPlugin.disableMiscChanges.Value;
        }
    }
}

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ZSlime
{

    [HarmonyPatch(typeof(PawnRenderNode_AnimalPart), "GraphicFor")]
    static class RainbowSlimePatch
    {
        static void Postfix(Pawn pawn, ref Graphic __result)
        {
            CompRainbow isRainbow = pawn.TryGetComp<CompRainbow>();
            if (isRainbow == null)
            {
                return;
            }

            long ticks = pawn.ageTracker.AgeBiologicalTicks;

            float speed = 5000f;

            float hue = (float)(ticks % speed) / speed;

            Color color = Color.HSVToRGB(hue, 1f, 1f);

            __result = __result.GetColoredVersion(ShaderDatabase.Cutout, color, __result.colorTwo);
        }
    }

}

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
    [HarmonyPatch(typeof(PawnGraphicSet), "ResolveAllGraphics")]
    static class RainbowSlimePatch
    {
        static void Postfix(PawnGraphicSet __instance)
        {
            CompRainbow isRainbow = __instance.pawn.TryGetComp<CompRainbow>();
            if (isRainbow == null)
            {
                return;
            }

            long ticks = __instance.pawn.ageTracker.AgeBiologicalTicks;

            float speed = 1500f;

            float hue = (float)(ticks % speed) / speed;

            Color color = Color.HSVToRGB(hue, 1f, 1f);

            //Color color = new Color((float)(ticks % 255) / 255, 0.5f, 0.5f);
            CompColorable comp = __instance.pawn.TryGetComp<CompColorable>();
            if (comp != null)
            {
                __instance.nakedGraphic = __instance.nakedGraphic.GetColoredVersion(__instance.nakedGraphic.Shader, color, Color.white);
            }

        }
    }
}

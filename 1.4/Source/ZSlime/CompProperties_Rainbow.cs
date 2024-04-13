using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ZSlime
{
    public class CompProperties_Rainbow : CompProperties
    {
        public CompProperties_Rainbow()
        {
            compClass = typeof(CompRainbow);
        }
    }


    public class CompRainbow : ThingComp
    {
        public override void CompTick()
        {
            if (Find.TickManager.TicksGame % 11 != 0)
            {
                return;
            }
            Pawn pawn = parent as Pawn;
            pawn.Drawer.renderer.graphics.ResolveAllGraphics();
        }

    }
}

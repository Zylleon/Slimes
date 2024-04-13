using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ZSlime
{
	// a fire overlay, but using semi-transparent graphics
	public class CompProperties_FireSlimeOverlay : CompProperties_FireOverlay
	{
		public CompProperties_FireSlimeOverlay()
		{
			compClass = typeof(CompFireSlimeOverlay);
		}

		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(CompFireSlimeOverlay.FireSlimeGraphic, thingDef, ghostCol).DrawFromDef(center.ToVector3ShiftedWithAltitude(drawAltitude), rot, thingDef);
		}
	}



	[StaticConstructorOnStartup]
	public class CompFireSlimeOverlay : CompFireOverlayBase
	{
		protected CompRefuelable refuelableComp;

		public static readonly Graphic FireSlimeGraphic = GraphicDatabase.Get<Graphic_Flicker>("ZSlimes/Misc/SlimeFire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);

		public new CompProperties_FireSlimeOverlay Props => (CompProperties_FireSlimeOverlay)props;

		public override void PostDraw()
		{
			base.PostDraw();
			if (refuelableComp == null || refuelableComp.HasFuel)
			{
				Vector3 drawPos = parent.DrawPos;
				drawPos.y += 3f / 74f;
				FireSlimeGraphic.Draw(drawPos, Rot4.North, parent);
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			refuelableComp = parent.GetComp<CompRefuelable>();
		}
	}
}
﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ZSlime
{
    public class CompProperties_SlimeSpawner : Verse.CompProperties
    {
		public ThingDef thingToSpawn;

		public int spawnCount = 1;

		public IntRange spawnIntervalRange = new IntRange(100, 100);

		public bool writeTimeLeftToSpawn;

		public string saveKeysPrefix;


		public CompProperties_SlimeSpawner()
		{
			compClass = typeof(CompSlimeSpawner);
		}
	}


	public class CompSlimeSpawner : ThingComp
	{
		private int ticksUntilSpawn;

		public CompProperties_SlimeSpawner PropsSpawner => (CompProperties_SlimeSpawner)props;

		//private bool PowerOn => parent.GetComp<CompPowerTrader>()?.PowerOn ?? false;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				ResetCountdown();
			}
		}

		public override void CompTick()
		{
			TickInterval(1);
		}

		public override void CompTickRare()
		{
			TickInterval(250);
		}

		private void TickInterval(int interval)
		{
			if (!parent.Spawned)
			{
				return;
			}
			
			else if (parent.Position.Fogged(parent.Map))
			{
				return;
			}
			ticksUntilSpawn -= interval;
			CheckShouldSpawn();
		}

		private void CheckShouldSpawn()
		{
			if (ticksUntilSpawn <= 0)
			{
				ResetCountdown();
				TryDoSpawn();
			}
		}

		public bool TryDoSpawn()
		{
			if (!parent.Spawned)
			{
				return false;
			}
			if (TryFindSpawnCell(parent, PropsSpawner.thingToSpawn, PropsSpawner.spawnCount, out var result))
			{
				Thing thing = ThingMaker.MakeThing(PropsSpawner.thingToSpawn);
				thing.stackCount = PropsSpawner.spawnCount;
				if (thing == null)
				{
					Log.Error("Could not spawn anything for " + parent);
				}
				
				GenPlace.TryPlaceThing(thing, result, parent.Map, ThingPlaceMode.Direct, out var lastResultingThing);


				Pawn slime = parent as Pawn;

				if(slime != null)
				{
					if (slime.Faction?.IsPlayer != true)
					{
						lastResultingThing.SetForbidden(value: true);
					}
				}
				
				return true;
			}
			return false;
		}

		public static bool TryFindSpawnCell(Thing parent, ThingDef thingToSpawn, int spawnCount, out IntVec3 result)
		{
			foreach (IntVec3 item in GenAdj.CellsAdjacent8Way(parent).InRandomOrder())
			{
				if (!item.Walkable(parent.Map))
				{
					continue;
				}
				Building edifice = item.GetEdifice(parent.Map);
				if (edifice != null && thingToSpawn.IsEdifice())
				{
					continue;
				}
				Building_Door building_Door = edifice as Building_Door;
				if ((building_Door != null && !building_Door.FreePassage) || (parent.def.passability != Traversability.Impassable && !GenSight.LineOfSight(parent.Position, item, parent.Map)))
				{
					continue;
				}
				bool flag = false;
				List<Thing> thingList = item.GetThingList(parent.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing.def.category == ThingCategory.Item && (thing.def != thingToSpawn || thing.stackCount > thingToSpawn.stackLimit - spawnCount))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					result = item;
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		private void ResetCountdown()
		{
			ticksUntilSpawn = PropsSpawner.spawnIntervalRange.RandomInRange;
		}

		public override void PostExposeData()
		{
			string text = (PropsSpawner.saveKeysPrefix.NullOrEmpty() ? null : (PropsSpawner.saveKeysPrefix + "_"));
			Scribe_Values.Look(ref ticksUntilSpawn, text + "ticksUntilSpawn", 0);
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "DEBUG: Spawn " + PropsSpawner.thingToSpawn.label;
				command_Action.icon = TexCommand.DesirePower;
				command_Action.action = delegate
				{
					ResetCountdown();
					TryDoSpawn();
				};
				yield return command_Action;
			}
		}
		
	}

}

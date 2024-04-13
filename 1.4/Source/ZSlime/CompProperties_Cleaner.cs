using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ZSlime
{
    public class CompProperties_Cleaner : Verse.CompProperties
    {

		public CompProperties_Cleaner()
		{
			compClass = typeof(CompCleaner);
		}
	}



	public class CompCleaner : ThingComp
	{

		public CompProperties_Cleaner PropsCleaner => (CompProperties_Cleaner)props;

		//private bool PowerOn => parent.GetComp<CompPowerTrader>()?.PowerOn ?? false;




		public override void CompTickRare()
		{
			if (!parent.Spawned)
			{
				return;
			}

			else if (parent.Position.Fogged(parent.Map))
			{
				return;
			}

			List<Thing> thingList = parent.Position.GetThingList(parent.Map);

			for(int i = 0; i < thingList.Count; i++)
            {
				if (thingList[i] is Fire)
				{
					thingList[i].Destroy();
				}
			}

		
			// only destroy one filth at a time
			if (Rand.Chance(0.35f))
			{

				foreach (Thing thing in thingList)
				{
					if (thing is Filth)
					{
						thing.Destroy();
						break;
					}
				}
				
			}
		}


	}


}

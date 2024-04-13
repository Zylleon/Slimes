using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ZSlime
{
    public sealed class Slimes : Mod
    {
        public Slimes(ModContentPack content) : base(content)
        {
            new Harmony("zylle.Slimes").PatchAll();
            Log.Message("Initializing Slimes");
        }
    }
}

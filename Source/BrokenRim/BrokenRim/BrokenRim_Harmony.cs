using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BrokenRim
{
    [StaticConstructorOnStartup]

    public static class BrokenRim_Harmony
    {
        static BrokenRim_Harmony() {
            Harmony harmony = new Harmony("planetsaturn.brokenrim");
            harmony.PatchAll();
        }
    }
}

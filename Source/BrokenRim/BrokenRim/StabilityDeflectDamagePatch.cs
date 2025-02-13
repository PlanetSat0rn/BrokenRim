using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BrokenRim
{
    [HarmonyPatch(typeof(ThingWithComps), "PreApplyDamage")]

    public static class StabilityDeflectDamagePatch
    {
        [HarmonyPostfix]

        public static void PostPreApplyDamage(ThingWithComps __instance, ref DamageInfo dinfo, ref bool absorbed) {
            if (absorbed || !(__instance is Pawn pawn)) {
                return;
            }
            foreach (HediffComp_Stability item in Utils.GetHediffCompsOfType<HediffComp_Stability>(pawn))
            {
                item.PreApplyDamage(dinfo, ref absorbed);
                if (absorbed) {
                    break;
                }
            }

        }
    } 
}

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace BrokenRim
{
    [HarmonyPatch(typeof(Bullet), "Impact")]
    public class Patches_ChargeDamage
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Bullet_Impact_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase methodBase)
        {
            List<CodeInstruction> il = instructions.ToList();
            LocalVariableInfo damageDef = methodBase.GetMethodBody().LocalVariables.First((LocalVariableInfo lv) => lv.LocalType == typeof(DamageInfo));

            for (int i = 0; i < il.Count; i++)
            {
                if (il[i].opcode == OpCodes.Ldc_I4_1 &&
                    il[i + 1].opcode == OpCodes.Ldc_I4_2 &&
                    il[i + 2].opcode == OpCodes.Ldc_I4_1 &&
                    il[i + 3].opcode == OpCodes.Call)
                {
                    yield return il[i];
                    yield return il[i + 1];
                    yield return il[i + 2];
                    yield return il[i + 3];
                    i += 3;
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Projectile), "launcher"));
                    yield return new CodeInstruction(OpCodes.Ldloca_S, damageDef.LocalIndex);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patches_ChargeDamage), "TryReplaceChargeDamage"));

                    continue;
                }
                yield return il[i];
            }
        }

        public static void TryReplaceChargeDamage(Thing launcher, ref DamageInfo dinfo)
        {
            if (launcher is Pawn p)
            {
                var equipment = p.equipment.AllEquipmentListForReading;
                for (int i = 0; i < equipment.Count; i++)
                {
                    if (equipment[i].GetComp<Comp_Charges>() is Comp_Charges comp)
                    {
                        dinfo.SetAmount(dinfo.Amount + (comp.Props.additionalDamage * comp.Props.oldCharges));
                    }
                }
            }
        }
    }
}

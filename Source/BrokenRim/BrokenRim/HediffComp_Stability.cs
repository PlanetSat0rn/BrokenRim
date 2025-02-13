using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace BrokenRim
{
    public class HediffComp_Stability : HediffComp
    {

        public int Stability = 50;
        public int Cooldown = 600;

        public HediffComp_Stability()
        {

        }

        public HediffCompProperties_Stability Props
        {
            get
            {
                return (HediffCompProperties_Stability)this.props;
            }
        }

        public void PreApplyDamage(DamageInfo dinfo, ref bool absorbed)
        {
            absorbed = true;
            bool stabilityCheck = this.Stability > 0;
            if (stabilityCheck)
            {
                bool damageCheck = !dinfo.Def.harmsHealth && dinfo.Def != null;
                if (!damageCheck)
                {
                    this.Stability = Math.Max(0, this.Stability - Math.Max((int)dinfo.Amount, 1));
                    if (this.Stability <= 0)
                    {
                        Explode();
                        return;
                    }
                    else
                    {
                        Defs_BR.Stability_Hit.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.parent.pawn.Position,this.parent.pawn.Map)));
                    }
                }
                else
                {
                    return;
                }
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look<int>(ref this.Stability, "Stability", 0, false);
            Scribe_Values.Look<int>(ref this.Cooldown, "CoolDown", 0, false);
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            Pawn pawn = this.parent.pawn;
            if (Find.Selector.SelectedPawns.Contains(pawn))
            {
                yield return new Gizmo_CosmokinStability
                {
                    comp = this
                };
            }
            yield break;
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            Pawn parentPawn = this.parent.pawn;
            bool deathCheck = this.Stability <= 0;
            bool sleepCheck = parentPawn.CurJob.def == JobDefOf.Wait_WithSleeping || parentPawn.CurJob.def == JobDefOf.Wait_Asleep || parentPawn.InBed();
            if (deathCheck)
            {
                Explode();
            }
            if (sleepCheck)
            {
                Cooldown--;
                if (Cooldown <= 0)
                {
                    Cooldown = 600;
                    Stability += 5;
                    if (Stability > Props.maxStability) { Stability = Props.maxStability; }
                }
            }
        }

        public void Explode()
        {
            Pawn parentPawn = this.parent.pawn;
            MoteMaker.ThrowText(parentPawn.TrueCenter(), parentPawn.Map, "Destabilized!");
            GenExplosion.DoExplosion(parentPawn.Position, parentPawn.Map, Props.explosionRadius, Props.damageDef, parentPawn, Props.damageAmount, -1, null, null, null, null, null, 0, 1, null, false, null, 0, 1, 0, false, null, null, null, true, 1, 0, true, null, 1f);
            parentPawn.Destroy();
        }
    }
}

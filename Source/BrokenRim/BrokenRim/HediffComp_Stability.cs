using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace BrokenRim
{
    public class HediffComp_Stability : HediffComp
    {

        public int Stability = 50;
        public int Cooldown = 600;
        public bool recovering = false;
        public bool forceRest = false;

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
                        Defs_BR.BR_Damage_Stability.Spawn(parent.pawn.Position,parent.pawn.Map);
                        Defs_BR.Stability_Hit.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.parent.pawn.Position,this.parent.pawn.Map)));
                        if (this.Stability <= Props.maxStability*0.15) {
                            MoteMaker.ThrowText(parent.pawn.TrueCenter(),parent.pawn.Map,"Stability is critically low", new Color(255,0,0), -1);
                        }
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
            bool sleepCheck = parentPawn.CurJob.def == JobDefOf.LayDown || parentPawn.CurJob.def == JobDefOf.Wait_Asleep || parentPawn.InBed();
            this.parent.Severity = (float)Stability / (float)Props.maxStability;
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
            if (Stability <= Props.maxStability * 0.20) {
                recovering = true;
            }
            if ((Stability >= Props.maxStability * 0.80 && recovering) || (recovering && parentPawn.Drafted))
            {
                recovering = false;
                if (parentPawn.CurJobDef == JobDefOf.LayDown) {
                    parentPawn.jobs.EndCurrentJob(JobCondition.Succeeded);
                }
            }
            if ((recovering && !parent.pawn.Drafted && parent.pawn.Faction == Faction.OfPlayer && parent.pawn.CurJobDef != JobDefOf.LayDown) || forceRest && !parentPawn.Drafted) {
                Job job = JobMaker.MakeJob(JobDefOf.LayDown, parentPawn.Position);
                parentPawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
            }
        }

        public void Explode()
        {
            Pawn parentPawn = this.parent.pawn;
            MoteMaker.ThrowText(parentPawn.TrueCenter(), parentPawn.Map, "Destabilized!");
            GenExplosion.DoExplosion(parentPawn.Position, parentPawn.Map, Props.explosionRadius, Props.damageDef, parentPawn, Props.damageAmount, -1, null, null, null, null, null, 0, 1, null, false, null, 0, 1, 0, false, null, null, null, true, 1, 0, true, null, 1f);
            parentPawn.Destroy(DestroyMode.KillFinalize);
            parentPawn.Kill(new DamageInfo(Defs_BR.Unstability,999));
        }
        public void Kamikaze()
        {
            Pawn parentPawn = this.parent.pawn;
            GenExplosion.DoExplosion(parentPawn.Position, parentPawn.Map, Stability/12, Props.damageDef, parentPawn, Stability/2, -1, null, null, null, null, null, 0, 1, null, false, null, 0, 1, 0, false, null, null, null, true, 1, 0, true, null, 1f);
            parentPawn.Kill(new DamageInfo(Defs_BR.Unstability, 999));
            parentPawn.Destroy(DestroyMode.KillFinalize);
            parentPawn.Kill(new DamageInfo(Defs_BR.Unstability, 999));
        }
    }
}

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
    public class Verb_WeaponWithCharges : Verb_Shoot
    {
        public SoundDef shootSound;

        public Comp_Charges comp
        {
            get
            {
                return this.CasterPawn.equipment.Primary.GetComp<Comp_Charges>();
            }
        }

        public override void WarmupComplete()
        {
            if (comp.Props.currentCharges >= comp.Props.targetCharges)
            {
                base.WarmupComplete();
                comp.Props.currentCharges = 0;
                Pawn pawn = this.currentTarget.Thing as Pawn;
                if (pawn != null && !pawn.Downed && !pawn.IsColonyMech && this.CasterIsPawn && this.CasterPawn.skills != null)
                {
                    float num = pawn.HostileTo(this.caster) ? 170f : 20f;
                    float num2 = this.verbProps.AdjustedFullCycleTime(this, this.CasterPawn);
                    this.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2, false, false);
                }
            }
            else
            {
                comp.Props.currentCharges++;
                comp.Props.chargeSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.CasterPawn)));
            }

        }
        protected override bool TryCastShot()
        {
            bool flag = base.TryCastShot();
            if (flag && this.CasterIsPawn)
            {
                this.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
            }
            return flag;
        }
    }

    public class VerbProperties_WeaponWithCharges : VerbProperties
    { 
        
    }

    //Comp
    public class Comp_Charges : CompEquippable
    {

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<int>(ref this.Props.charges, "charges", 5, false);
            Scribe_Values.Look<int>(ref this.Props.currentCharges, "currentCharges", 0, false);
            Scribe_Values.Look<SoundDef>(ref this.Props.chargeSound, "lastFireTick", Defs_BR.BR_ChargeSound, false);
        }
        public CompProperties_Charges Props
        {
            get
            {
                return (CompProperties_Charges)this.props;
            }
        }


        public override IEnumerable<Gizmo> CompGetEquippedGizmosExtra()
        {
            if (this.Holder != null)
            {
                this.LogStuff();
                Log.Message("the");
                if (Find.Selector.SelectedPawns.Contains(this.Holder) && this.Holder.Drafted && this.Holder.Faction == Faction.OfPlayer)
                {
                    Gizmo_WeaponWithCharges gizmo = new Gizmo_WeaponWithCharges();
                    gizmo.comp = this;
                    yield return gizmo;
                }
                yield break;
            }
        }

        public void LogStuff()
        {
            Log.Message(this.Holder);
            Log.Message(this.Holder.Drafted);
            Log.Message(this.Holder.Faction);
        }
    }

    
        public class CompProperties_Charges : CompProperties
    {

        public CompProperties_Charges() {
            this.compClass = typeof(Comp_Charges);
        }

        public SoundDef chargeSound = Defs_BR.BR_ChargeSound;
        public int charges = 3;
        public float targetCharges = 5;
        public int currentCharges = 0;
    }
}

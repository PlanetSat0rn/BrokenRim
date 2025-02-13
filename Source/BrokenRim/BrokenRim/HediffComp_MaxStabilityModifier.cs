using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BrokenRim
{
    public class HediffComp_MaxStabilityModifier : HediffComp
    {
        public HediffCompProperties_MaxStabilityModifier Props
        {
            get
            {
                return (HediffCompProperties_MaxStabilityModifier)this.props;
            }
        }

        public HediffComp_MaxStabilityModifier()
        {
            
        }

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            HediffWithComps stability = (HediffWithComps)parent.pawn.health.GetOrAddHediff(Defs_BR.BR_Stability);
            HediffComp_Stability comp = stability.TryGetComp<HediffComp_Stability>();
            comp.Props.maxStability = Props.maxStability;
        }
    }
    public class HediffCompProperties_MaxStabilityModifier : HediffCompProperties
    {
        public HediffCompProperties_MaxStabilityModifier()
        {
            this.compClass = typeof(HediffComp_MaxStabilityModifier);
        }

        public int maxStability;
    }
}

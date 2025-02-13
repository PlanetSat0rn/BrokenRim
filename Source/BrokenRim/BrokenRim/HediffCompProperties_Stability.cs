using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BrokenRim
{
    public class HediffCompProperties_Stability : HediffCompProperties
    {
        public HediffCompProperties_Stability()
        {
            this.compClass = typeof(HediffComp_Stability);
        }

        public DamageDef damageDef;
        public float explosionRadius;
        public int maxStability = 100;
        public int damageAmount = -1;
    }
}

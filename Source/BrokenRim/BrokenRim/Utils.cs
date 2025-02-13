using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BrokenRim
{
    public class Utils
    {
        public static List<T> GetHediffCompsOfType<T>(Pawn p) where T : HediffComp
        {
            List<T> list = new List<T>();
            var hediffs = p.health.hediffSet.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {
                if (hediffs[i].TryGetComp<T>() is T comp)
                    list.Add(comp);
            }
            return list;
        }
    }
}

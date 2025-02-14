using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BrokenRim
{
    [StaticConstructorOnStartup]
    public class Gizmo_WeaponWithCharges : Gizmo
    {
        private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.8f, 0.8f, 0.8f));
        private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

        public Gizmo_WeaponWithCharges()
        {
            this.Order = -100;
        }

        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect rect = new Rect(topLeft, new Vector2(140f, 75f));
            Rect rect2 = rect.ContractedBy(6f);
            Widgets.DrawWindowBackground(rect);
            float progress = (float)comp.Props.currentCharges / (float)comp.Props.charges;
            Widgets.FillableBar(rect2,progress,FullBarTex,EmptyBarTex,true);
            Widgets.HorizontalSlider(rect2, ref comp.Props.targetCharges, new FloatRange(1, comp.Props.charges), "Charges", 1);
            return new GizmoResult(GizmoState.Clear);
        }

        public Comp_Charges comp;
    }
}

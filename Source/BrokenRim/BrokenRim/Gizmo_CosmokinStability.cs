using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BrokenRim
{
    public class Gizmo_CosmokinStability : Gizmo
    {

        public Gizmo_CosmokinStability()
        {
            this.Order = -100;
        }

        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }

        private static readonly Texture2D FullStabilityTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.8f,0.8f,0.8f));
        private static readonly Texture2D EmptyStabilityTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect rect = new Rect(topLeft, new Vector2(this.GetWidth(maxWidth), 75f));
            Rect rect2 = rect.ContractedBy(6f);
            Widgets.DrawWindowBackground(rect);
            Rect rect3 = rect2;
            rect3.height = rect.height / 2f;
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect3, "BR.UI.Stability".Translate());
            Rect rect4 = rect2;
            rect4.yMin = rect2.y + rect2.height / 2f;
            float fillPercent = (float)this.comp.Stability / Mathf.Max(1f, 100f);
            Widgets.FillableBar(rect4, fillPercent, FullStabilityTex, EmptyStabilityTex, false);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect4, this.comp.Stability.ToString() + "/ 100");
            Text.Anchor = TextAnchor.UpperLeft;
            return new GizmoResult(GizmoState.Clear);
        }

        public HediffComp_Stability comp;
    }
}

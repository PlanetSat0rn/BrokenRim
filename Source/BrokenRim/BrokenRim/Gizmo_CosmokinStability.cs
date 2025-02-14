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
    public class Gizmo_CosmokinStability : Gizmo
    {

        public Gizmo_CosmokinStability()
        {
            this.Order = -100;
        }

        public override float GetWidth(float maxWidth)
        {
            return 160f;
        }

        private static readonly Texture2D FullStabilityTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.8f,0.8f,0.8f));
        private static readonly Texture2D EmptyStabilityTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);
        private static readonly Texture2D KamikazeTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f,0f,0f));

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect rect = new Rect(topLeft, new Vector2(this.GetWidth(maxWidth), 75f));
            Rect rect2 = new Rect(topLeft+new Vector2(3,3), new Vector2(134f, 69f));
            Widgets.DrawWindowBackground(rect);
            Rect rect3 = rect2;
            rect3.height = rect.height / 2f;
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect3, "BR.UI.Stability".Translate());
            Rect rect4 = rect2;
            rect4.yMin = rect2.y + rect2.height / 2f;
            float fillPercent = (float)this.comp.Stability / Mathf.Max(1f, (float)comp.Props.maxStability);
            Widgets.FillableBar(rect4, fillPercent, FullStabilityTex, EmptyStabilityTex, false);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect4, this.comp.Stability.ToString() + "/ " + comp.Props.maxStability.ToString());
            Text.Anchor = TextAnchor.UpperLeft;
            bool kamikaze = Widgets.ButtonImage(new Rect(topLeft + new Vector2(130f,10f), new Vector2(20f,20f)), KamikazeTex,true,"Use your remaining Stability to create a big explosion.");
            if (kamikaze )
            {
                comp.Kamikaze();
            }
           Widgets.Checkbox(topLeft + new Vector2(100f, 10f), ref comp.forceRest, 20f);

            return new GizmoResult(GizmoState.Clear);
        }

        public HediffComp_Stability comp;
    }
}

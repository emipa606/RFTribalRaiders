using UnityEngine;
using Verse;

namespace TribalRaiders_Code;

public class Settings : ModSettings
{
    public bool TribalPlanet;

    public void DoWindowContents(Rect canvas)
    {
        var list = new Listing_Standard { ColumnWidth = canvas.width };
        list.Begin(canvas);
        list.Gap();
        list.CheckboxLabeled("TribalRaiders.TribalPlanet".Translate(), ref TribalPlanet,
            "TribalRaiders.TribalPlanetTip".Translate());
        if (Controller.currentVersion != null)
        {
            list.Gap();
            GUI.contentColor = Color.gray;
            list.Label("TribalRaiders.CurrentModVersion".Translate(Controller.currentVersion));
            GUI.contentColor = Color.white;
        }

        list.End();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref TribalPlanet, "tribalPlanet");
    }
}
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace TribalRaiders_Code;

[HarmonyPatch(typeof(FactionGenerator), "GenerateFactionsIntoWorldLayer", typeof(PlanetLayer),
    typeof(List<FactionDef>))]
public static class FactionGenerator_GenerateFactionsIntoWorldLayer
{
    public static bool Prefix()
    {
        if (!Controller.Settings.TribalPlanet)
        {
            return true;
        }

        var num = 0;
        foreach (var allDef in DefDatabase<FactionDef>.AllDefs)
        {
            if (allDef.defName is "OutlanderCivil" or "OutlanderRough" or "Pirate" or "Empire")
            {
                continue;
            }

            for (var i = 0; i < allDef.requiredCountAtGameStart; i++)
            {
                var faction = FactionGenerator.NewGeneratedFaction(new FactionGeneratorParms(allDef));
                Find.FactionManager.Add(faction);
                if (!allDef.hidden)
                {
                    num++;
                }
            }
        }

        while (num < 5)
        {
            var factionDef = (
                from fa in DefDatabase<FactionDef>.AllDefs
                where fa.defName is not "OutlanderCivil" and not "OutlanderRough" and not "Pirate" and not "Empire" &&
                      Find.FactionManager.AllFactions.Count(f => f.def == fa) < fa.maxConfigurableAtWorldCreation
                select fa).RandomElement();

            var faction1 = FactionGenerator.NewGeneratedFaction(new FactionGeneratorParms(factionDef));
            Find.World.factionManager.Add(faction1);
            num++;
        }

        var tilesCount = Find.WorldGrid.TilesCount / 100000f;
        var settlementsPer100kTiles = new FloatRange(75f, 85f);
        var count = GenMath.RoundRandom(tilesCount * settlementsPer100kTiles.RandomInRange);
        count -= Find.WorldObjects.Settlements.Count;
        for (var j = 0; j < count; j++)
        {
            var faction2 = (
                from x in Find.World.factionManager.AllFactionsListForReading
                where !x.def.isPlayer && !x.def.hidden
                select x).RandomElementByWeight(x => x.def.settlementGenerationWeight);
            var settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
            settlement.SetFaction(faction2);
            settlement.Tile = TileFinder.RandomSettlementTileFor(faction2);
            settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement);
            Find.WorldObjects.Add(settlement);
        }

        return false;
    }
}
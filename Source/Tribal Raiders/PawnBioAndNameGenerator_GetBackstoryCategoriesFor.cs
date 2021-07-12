using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace TribalRaiders_Code
{
    [HarmonyPatch(typeof(PawnBioAndNameGenerator), "GetBackstoryCategoryFiltersFor", typeof(Pawn), typeof(FactionDef))]
    public static class PawnBioAndNameGenerator_GetBackstoryCategoriesFor
    {
        public static void Postfix(Pawn pawn, FactionDef faction, ref List<BackstoryCategoryFilter> __result)
        {
            if (faction.defName != "TribalRaiders")
            {
                return;
            }

            var strs = new List<BackstoryCategoryFilter>();
            var backstoryCategoryFilter = new BackstoryCategoryFilter {categories = new List<string> {"TribalRaider"}};
            strs.Add(backstoryCategoryFilter);
            __result = strs;
        }
    }
}
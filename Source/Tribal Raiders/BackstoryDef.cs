using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld;
using Verse;

namespace TribalRaiders_Code
{
    public class BackstoryDef : Def
    {
        public bool addToDatabase = true;

        public string baseDesc;
        public string bodyTypeFemale;
        public string bodyTypeGlobal;
        public string bodyTypeMale;
        public List<BackstoryDefTraitListItem> disallowedTraits = new List<BackstoryDefTraitListItem>();
        public List<BackstoryDefTraitListItem> forcedTraits = new List<BackstoryDefTraitListItem>();
        public List<WorkTags> requiredWorkTags = new List<WorkTags>();
        public bool shuffleable = true;
        public List<BackstoryDefSkillListItem> skillGains = new List<BackstoryDefSkillListItem>();
        public BackstorySlot slot = BackstorySlot.Adulthood;
        public List<string> spawnCategories = new List<string>();
        public string title;
        public string titleShort;
        public List<WorkTags> workAllows = new List<WorkTags>();
        public List<WorkTags> workDisables = new List<WorkTags>();

        public static BackstoryDef Named(string defName)
        {
            return DefDatabase<BackstoryDef>.GetNamed(defName);
        }

        public override void ResolveReferences()
        {
            base.ResolveReferences();
            if (!addToDatabase)
            {
                return;
            }

            if (BackstoryDatabase.allBackstories.ContainsKey(this.UniqueSaveKey()))
            {
                Log.Error("Backstory Error (" + defName + "): Duplicate defName.");
                return;
            }

            var b = new Backstory();
            if (!title.NullOrEmpty())
            {
                b.SetTitle(title, title);
            }
            else
            {
                return;
            }

            if (!titleShort.NullOrEmpty())
            {
                b.SetTitleShort(titleShort, titleShort);
            }
            else
            {
                b.SetTitleShort(b.title, b.title);
            }

            b.baseDesc = !baseDesc.NullOrEmpty() ? baseDesc : "Empty.";

            var bodyTypeSet = false;
            if (!string.IsNullOrEmpty(bodyTypeGlobal))
            {
                bodyTypeSet = SetGlobalBodyType(b, bodyTypeGlobal);
            }

            if (!bodyTypeSet)
            {
                if (!SetMaleBodyType(b, bodyTypeMale))
                {
                    SetMaleBodyType(b, "Male");
                }

                if (!SetFemaleBodyType(b, bodyTypeFemale))
                {
                    SetFemaleBodyType(b, "Female");
                }
            }

            b.slot = slot;
            b.shuffleable = shuffleable;
            if (spawnCategories.NullOrEmpty())
            {
                return;
            }

            b.spawnCategories = spawnCategories;

            if (workAllows.Count > 0)
            {
                foreach (WorkTags current in Enum.GetValues(typeof(WorkTags)))
                {
                    if (!workAllows.Contains(current))
                    {
                        b.workDisables |= current;
                    }
                }
            }
            else if (workDisables.Count > 0)
            {
                foreach (var tag in workDisables)
                {
                    b.workDisables |= tag;
                }
            }
            else
            {
                b.workDisables = WorkTags.None;
            }

            if (requiredWorkTags.Count > 0)
            {
                foreach (var tag in requiredWorkTags)
                {
                    b.requiredWorkTags |= tag;
                }
            }
            else
            {
                b.requiredWorkTags = WorkTags.None;
            }

            var skillDefs = new Dictionary<SkillDef, int>();
            foreach (var skillGain in skillGains)
            {
                var named = DefDatabase<SkillDef>.GetNamed(skillGain.key, false);
                if (named == null)
                {
                    Log.Error(
                        string.Concat("Tribal Raiders: Unable to find SkillDef of [", skillGain.key,
                            "] for Backstory.Title [", b.title, "]"));
                    continue;
                }

                skillDefs.Add(named, skillGain.value);
            }

            b.skillGainsResolved = skillDefs;
            var fTraitList = forcedTraits.ToDictionary(i => i.key, i => i.value);
            if (fTraitList.Count > 0)
            {
                b.forcedTraits = new List<TraitEntry>();
                foreach (var trait in fTraitList)
                {
                    b.forcedTraits.Add(new TraitEntry(TraitDef.Named(trait.Key), trait.Value));
                }
            }

            var dTraitList = disallowedTraits.ToDictionary(i => i.key, i => i.value);
            if (dTraitList.Count > 0)
            {
                b.disallowedTraits = new List<TraitEntry>();
                foreach (var trait in dTraitList)
                {
                    b.disallowedTraits.Add(new TraitEntry(TraitDef.Named(trait.Key), trait.Value));
                }
            }

            b.ResolveReferences();
            b.PostLoad();
            b.identifier = this.UniqueSaveKey();
            var foundError = false;
            foreach (var s in b.ConfigErrors(false))
            {
                Log.Error("Backstory Error (" + b.identifier + "): " + s);
                foundError = true;
            }

            if (!foundError)
            {
                BackstoryDatabase.allBackstories.Add(b.identifier, b);
            }
        }

        private static bool SetGlobalBodyType(Backstory b, string s)
        {
            if (!TryGetBodyTypeDef(s, out var def))
            {
                return false;
            }

            typeof(Backstory).GetField("bodyTypeGlobal", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(b, def.ToString());
            typeof(Backstory).GetField("bodyTypeGlobalResolved", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(b, def);
            return true;
        }

        private static bool SetMaleBodyType(Backstory b, string s)
        {
            if (!TryGetBodyTypeDef(s, out var def))
            {
                return false;
            }

            typeof(Backstory).GetField("bodyTypeMale", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(b, def.ToString());
            typeof(Backstory).GetField("bodyTypeMaleResolved", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(b, def);
            return true;
        }

        private static bool SetFemaleBodyType(Backstory b, string s)
        {
            if (!TryGetBodyTypeDef(s, out var def))
            {
                return false;
            }

            typeof(Backstory).GetField("bodyTypeFemale", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(b, def.ToString());
            typeof(Backstory).GetField("bodyTypeFemaleResolved", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(b, def);
            return true;
        }

        private static bool TryGetBodyTypeDef(string s, out BodyTypeDef def)
        {
            if (string.IsNullOrEmpty(s))
            {
                def = null;
                return false;
            }

            def = DefDatabase<BodyTypeDef>.GetNamed(s, false);
            if (def == null)
            {
                return false;
            }

            return true;
        }
    }
}
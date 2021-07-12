using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace TribalRaiders_Code
{
    public class Controller : Mod
    {
        public static Settings Settings;

        public Controller(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("net.rainbeau.rimworld.mod.tribalraiders");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Settings = GetSettings<Settings>();
        }

        public override string SettingsCategory()
        {
            return "TribalRaiders.Title".Translate();
        }

        public override void DoSettingsWindowContents(Rect canvas)
        {
            Settings.DoWindowContents(canvas);
        }
    }
}
using Harmony;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DebugModeSetting
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            //helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            var harmony = HarmonyInstance.Create(helper.ModRegistry.ModID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }


        /*********
        ** Private methods
        *********/

        [HarmonyPatch(typeof(OptionsPage), MethodType.Constructor, new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) })]
        static class OptionsPage_ctor_Patch
        {
            static void Postfix(ref List<OptionsElement> ___options)
            {
                ___options.Insert(9, new OptionsCheckbox("Debug Mode", 33, -1, -1));
                //___options.Add(new OptionsCheckbox("Debug Mode", 33, -1, -1));
            }
        }

        [HarmonyPatch(typeof(Options), "changeCheckBoxOption", new Type[] { typeof(int), typeof(bool) })]
        static class Options_changeCheckBoxOption_Patch
        {
            static void Postfix(int which, bool value)
            {
                if (which == 33)
                {
                    OptionHolder.enableDebugMode = value;
                    Program.releaseBuild = !value;
                }
            }
        }

        [HarmonyPatch(typeof(Options), "setCheckBoxToProperValue", new Type[] { typeof(OptionsCheckbox) })]
        static class Options_setCheckBoxToProperValue_Patch
        {
            static void Postfix(OptionsCheckbox checkbox)
            {
                if (checkbox.whichOption == 33)
                {
                    checkbox.isChecked = OptionHolder.enableDebugMode;
                }
            }
        }

        public static class OptionHolder
        {
            public static bool enableDebugMode = false;
        }
    }
}

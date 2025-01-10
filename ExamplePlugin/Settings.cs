using BadAssCompany;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using EmotesAPI;
using ExamplePlugin;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExamplePlugin
{
    public static class Settings
    {
        public static ConfigEntry<bool> chikaProp;
        public static ConfigEntry<bool> moneyProp;
        public static ConfigEntry<bool> desertlightProp;
        public static ConfigEntry<bool> BimBamBomProp;
        public static ConfigEntry<bool> SummermogusProp;
        public static ConfigEntry<bool> FloatLightProp;
        public static ConfigEntry<bool> Amogus;
        public static ConfigEntry<bool> BluntAnimatorProp;
        public static ConfigEntry<bool> neverseeProp;
        public static ConfigEntry<bool> ImaMysteryProp;
        internal static void Setup()
        {
            chikaProp = BadAssEmotesPlugin.instance.Config.Bind<bool>("Settings", "Chika prop", true, "Allows Chika to have a prop");
            moneyProp = BadAssEmotesPlugin.instance.Config.Bind<bool>("Settings", "Make it Rain prop", true, "Allows Make it Rain to have a prop");
            desertlightProp = BadAssEmotesPlugin.instance.Config.Bind<bool>("Settings", "Rivers in the Desert prop", true, "Allows Rivers in the Desert to have a prop");
            BimBamBomProp = BadAssEmotesPlugin.instance.Config.Bind<bool>("Settings", "Bim Bam Boom prop", true, "Allows Bim Bam Boom to have a prop");
            SummermogusProp = BadAssEmotesPlugin.instance.Config.Bind<bool>("Settings", "Summertime prop", true, "Allows Summertime to have a prop");
            FloatLightProp = BadAssEmotesPlugin.instance.Config.Bind<bool>("Settings", "Float prop", true, "Allows Float to have a prop");
            Amogus = BadAssEmotesPlugin.instance.Config.Bind<bool>("Settings", "Markipiler prop", true, "Allows Markipiler to have a prop");
            BluntAnimatorProp = BadAssEmotesPlugin.instance.Config.Bind<bool>("Settings", "Ralsei Dies prop", true, "Allows Ralsei Dies to have a prop");
            neverseeProp = BadAssEmotesPlugin.instance.Config.Bind<bool>("Settings", "Last Surprise prop", true, "Allows Last Surprise to have a prop");
            ImaMysteryProp = BadAssEmotesPlugin.instance.Config.Bind<bool>("Settings", "Im a Mystery prop", true, "Allows Im a Mystery to have a prop");


            if (Chainloader.PluginInfos.ContainsKey("ainavt.lc.lethalconfig"))
            {
                LethalConfigSupport.LethalConfig();
            }
        }
    }
}

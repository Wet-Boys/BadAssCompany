using System;
using System.Collections.Generic;
using System.Text;
using ExamplePlugin;
using LethalConfig;
using LethalConfig.ConfigItems;

namespace BadAssCompany
{
    internal class LethalConfigSupport
    {
        internal static void LethalConfig()
        {
            LethalConfigManager.SetModDescription("API for importing animations to Lethal Company");
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(Settings.FloatLightProp, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(Settings.moneyProp, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(Settings.chikaProp, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(Settings.desertlightProp, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(Settings.BimBamBomProp, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(Settings.SummermogusProp, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(Settings.Amogus, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(Settings.BluntAnimatorProp, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(Settings.neverseeProp, false));
            LethalConfigManager.AddConfigItem(new BoolCheckBoxConfigItem(Settings.ImaMysteryProp, false));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BeatSaberMarkupLanguage.Attributes;
using NightscoutCounter.Configuration;
using NightscoutCounter.Utilities;

namespace NightscoutCounter.UI
{
    internal class NightscoutSettingsHandler
    {

        [UIValue("site-url")]
        public string nightscoutURL
        {
            get => PluginConfig.Instance.NightscoutSiteURL;
            set
            {
                if (value == "") value = "https://example.herokuapp.com";
                PluginConfig.Instance.NightscoutSiteURL = value;
            }
        }

        [UIValue("site-token")]
        public string nightscoutToken
        {
            get => PluginConfig.Instance.NightscoutSiteToken;
            set => PluginConfig.Instance.NightscoutSiteToken = value;
        }

        [UIValue("display-options")]
        private List<object> displayOptions = new object[] { "mg/dL (US)", "mmol/L (Other)"}.ToList();

        [UIValue("display-choice")]
        public string displayChoice
        {
            get => PluginConfig.Instance.DisplayType;
            set
            {
                PluginConfig.Instance.DisplayType = value;
                // since we're changing the data type, it's important to refresh the data manually.
                NightscoutDataService.UpdateBloodSugar();
            }
        }

        [UIValue("arrows-enabled")]
        public bool arrowsEnabled
        {
            get => PluginConfig.Instance.ArrowsEnabled;
            set => PluginConfig.Instance.ArrowsEnabled = value;
        }

        [UIValue("high-threshold")]
        public int highThreshold
        {
            get => PluginConfig.Instance.HighThreshold;
            set => PluginConfig.Instance.HighThreshold = value;
        }

        [UIValue("low-threshold")]
        public int lowThreshold
        {
            get => PluginConfig.Instance.LowThreshold;
            set => PluginConfig.Instance.LowThreshold = value;
        }

        [UIValue("normal-glucose-color")]
        public Color normalGlucoseColor
        {
            get => PluginConfig.Instance.ColorFromConfigOption(PluginConfig.Instance.NormalGlucoseColor);
            set => PluginConfig.Instance.NormalGlucoseColor = ColorUtility.ToHtmlStringRGB(value);
        }

        [UIValue("high-glucose-color")]
        public Color highGlucoseColor
        {
            get => PluginConfig.Instance.ColorFromConfigOption(PluginConfig.Instance.HighGlucoseColor);
            set => PluginConfig.Instance.HighGlucoseColor = ColorUtility.ToHtmlStringRGB(value);
        }

        [UIValue("low-glucose-color")]
        public Color lowGlucoseColor
        {
            get => PluginConfig.Instance.ColorFromConfigOption(PluginConfig.Instance.LowGlucoseColor);
            set => PluginConfig.Instance.LowGlucoseColor = ColorUtility.ToHtmlStringRGB(value);
        }
    }
}

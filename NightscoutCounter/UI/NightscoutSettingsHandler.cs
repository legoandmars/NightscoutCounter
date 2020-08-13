using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BeatSaberMarkupLanguage.Attributes;
using NightscoutCounter.Configuration;

namespace NightscoutCounter.UI
{
    class NightscoutSettingsHandler : MonoBehaviour
    {
        private Config _config = Config.Instance;

        [UIValue("site-url")]
        public string nightscoutURL
        {
            get => _config.NightscoutSiteURL;
            set
            {
                if (value == "") value = "https://example.herokuapp.com";
                _config.NightscoutSiteURL = value;
            }
        }
        [UIValue("arrows-enabled")]
        public bool arrowsEnabled
        {
            get => _config.ArrowsEnabled;
            set => _config.ArrowsEnabled = value;
        }


        [UIValue("high-threshold")]
        public int highThreshold
        {
            get => _config.HighThreshold;
            set =>  _config.HighThreshold = value;
        }

        [UIValue("low-threshold")]
        public int lowThreshold
        {
            get => _config.LowThreshold;
            set => _config.LowThreshold = value;
        }
        [UIValue("normal-glucose-color")]
        public Color normalGlucoseColor
        {
            get => _config.colorFromConfigOption(_config.NormalGlucoseColor);
            set => _config.NormalGlucoseColor = ColorUtility.ToHtmlStringRGB(value);
        }
        [UIValue("high-glucose-color")]
        public Color highGlucoseColor
        {
            get => _config.colorFromConfigOption(_config.HighGlucoseColor);
            set => _config.HighGlucoseColor = ColorUtility.ToHtmlStringRGB(value);
        }

        [UIValue("low-glucose-color")]
        public Color lowGlucoseColor
        {
            get => _config.colorFromConfigOption(_config.LowGlucoseColor);
            set => _config.LowGlucoseColor = ColorUtility.ToHtmlStringRGB(value);
        }
    }
}

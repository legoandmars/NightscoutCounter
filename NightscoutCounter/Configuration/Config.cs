using System.Collections.Generic;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;

namespace NightscoutCounter.Configuration
{
    public class Config
    {
        internal static Config Instance { get; set; }

        [NonNullable]
        public virtual int HighThreshold { get; set; } = 180;
        [NonNullable]
        public virtual int LowThreshold { get; set; } = 70;

        [NonNullable]
        public virtual string NormalGlucoseColor { get; set; } = ColorUtility.ToHtmlStringRGB(Color.white);
        [NonNullable]
        public virtual string HighGlucoseColor { get; set; } = ColorUtility.ToHtmlStringRGB(Color.yellow);
        [NonNullable]
        public virtual string LowGlucoseColor { get; set; } = ColorUtility.ToHtmlStringRGB(Color.red);

        [NonNullable]
        public virtual string NightscoutSiteURL { get; set; } = "https://example.herokuapp.com";

        [NonNullable]
        public virtual bool ArrowsEnabled { get; set; } = true;

        public Color colorFromConfigOption(string configOption)
        {
            Color outputColor;
            if (ColorUtility.TryParseHtmlString("#" + configOption, out outputColor))
            {
                return outputColor;
            }
            else return Color.white;
        }
    }
}

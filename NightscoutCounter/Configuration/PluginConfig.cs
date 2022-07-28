using UnityEngine;

namespace NightscoutCounter.Configuration
{
    public class PluginConfig
    {
		public static PluginConfig Instance { get; set; }
        public virtual int HighThreshold { get; set; } = 180;
        public virtual int LowThreshold { get; set; } = 70;

        public virtual string NormalGlucoseColor { get; set; } = ColorUtility.ToHtmlStringRGB(Color.white);
        public virtual string HighGlucoseColor { get; set; } = ColorUtility.ToHtmlStringRGB(Color.yellow);
        public virtual string LowGlucoseColor { get; set; } = ColorUtility.ToHtmlStringRGB(Color.red);

        public virtual string NightscoutSiteURL { get; set; } = "https://example.herokuapp.com";

        public virtual string NightscoutSiteToken { get; set; } = "";

        public virtual string DisplayType { get; set; } = "mg/dL (US)";
        public virtual bool ArrowsEnabled { get; set; } = true;

        public Color ColorFromConfigOption(string configOption)
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

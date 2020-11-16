using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine;
using NightscoutCounter.Configuration;
using NightscoutCounter.Data;

namespace NightscoutCounter.Utilities
{
    static class NightscoutDataService
    {
        private static readonly HttpClient client = new HttpClient();

        public static event EventHandler<NightscoutData> BGEvent = delegate { };

        public static NightscoutData cachedData;
        private static string ArrowValues(string directionInput)
        {
            var exportString = "";
            switch (directionInput.ToUpper())
            {
                case "NONE":
                    exportString = "";
                    break;
                case "DOUBLEUP":
                    exportString = "↑↑";
                    break;
                case "SINGLEUP":
                    exportString = "↑";
                    break;
                case "FORTYFIVEUP":
                    exportString = "↗";
                    break;
                case "FLAT":
                    exportString = "→";
                    break;
                case "FORTYFIVEDOWN":
                    exportString = "↘";
                    break;
                case "SINGLEDOWN":
                    exportString = "↓";
                    break;
                case "DOUBLEDOWN":
                    exportString = "↓↓";
                    break;
                case "NOT COMPUTABLE":
                    exportString = "";
                    break;
                case "RATE OUT OF RANGE":
                    exportString = "";
                    break;
            }
            return exportString;
        }
        private static Color ColorFromBG(float sgv)
        {
            if(sgv <= PluginConfig.Instance.LowThreshold)
            {
                return PluginConfig.Instance.ColorFromConfigOption(PluginConfig.Instance.LowGlucoseColor);
            }else if(sgv >= PluginConfig.Instance.HighThreshold)
            {
                return PluginConfig.Instance.ColorFromConfigOption(PluginConfig.Instance.HighGlucoseColor);
            }
            return PluginConfig.Instance.ColorFromConfigOption(PluginConfig.Instance.NormalGlucoseColor);
        }

        public static async Task<NightscoutData> GetBloodSugar()
        {
            string siteURL = PluginConfig.Instance.NightscoutSiteURL;
            // do some string manipulation to make sure the URL is valid
            if (siteURL == "https://example.herokuapp.com") return null;
            if (siteURL.Substring(siteURL.Length - 1) != "/") siteURL += "/";
            if (siteURL.Substring(0, 7) == "http://") siteURL = "https://" + siteURL.Substring(7);
            if (siteURL.Substring(0, 8) != "https://") siteURL = "https://" + siteURL;

            string responseString = await client.GetStringAsync(siteURL + "api/v1/entries.json?count=1");
            NightscoutData nightscoutData = JsonConvert.DeserializeObject<NightscoutData>(responseString.Substring(1, responseString.Length - 2));
            nightscoutData.directionSymbol = ArrowValues(nightscoutData.direction);
            nightscoutData.color = ColorFromBG(nightscoutData.sgv);
            nightscoutData.type = "mg/dl";
            if(PluginConfig.Instance.DisplayType == "mmol/L (Other)")
            {
                nightscoutData.type = "mmol/L";
                nightscoutData.sgv = (float) Math.Round(nightscoutData.sgv * 0.55f) / 10;
            }
            return nightscoutData;
        }

        public static async void UpdateBloodSugar()
        {
            NightscoutData data = await GetBloodSugar();
            cachedData = data;
            BGEvent.Invoke(null, cachedData);
        }
        public static void UpdateBloodSugarTask()
        {
            var timer = new Timer((e) =>
            {
                UpdateBloodSugar();
            }, null, 5000, 60000);
        }
    }
}

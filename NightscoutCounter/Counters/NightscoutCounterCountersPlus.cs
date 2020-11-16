using CountersPlus.Counters.Custom;
using TMPro;
using UnityEngine;
using NightscoutCounter.Configuration;
using NightscoutCounter.Data;
using NightscoutCounter.Utilities;

namespace NightscoutCounter.Counters
{
    class NightscoutCounterCountersPlus : BasicCustomCounter
    {
        TMP_Text bgText;
        public override void CounterInit()
        {
            Logger.log.Debug("Creating nightscout counter...");
            bgText = CanvasUtility.CreateTextFromSettings(Settings);
            bgText.text = "--- mg/dl";
            bgText.color = Color.white;
            UpdateText(null, NightscoutDataService.cachedData);

            NightscoutDataService.BGEvent += UpdateText;
        }

        public async void UpdateText(object sender = null, NightscoutData data = null) {
            if (data == null) data = await NightscoutDataService.GetBloodSugar(); // try one more time
            if (data == null) return;
            bgText.text = $"{data.sgv} {data.type}{(PluginConfig.Instance.ArrowsEnabled ? data.directionSymbol : "")}";
            bgText.color = data.color;
        }

        public override void CounterDestroy()
        {
            NightscoutDataService.BGEvent -= UpdateText;
        }
    }
}

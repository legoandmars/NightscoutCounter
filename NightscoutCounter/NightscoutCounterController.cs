using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BeatSaberMarkupLanguage;
using System.Net.Http;
using Newtonsoft.Json;
using NightscoutCounter.Configuration;

namespace NightscoutCounter
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    /// 
    public class NightscoutData
    {
        public float sgv;
        public string direction;
    }
    public class NightscoutCounterController : MonoBehaviour
    {
        private Config _config = Config.Instance;
        private static readonly HttpClient client = new HttpClient();

        public static NightscoutCounterController instance { get; private set; }
        private TextMeshProUGUI _counter;

        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            try
            {
                Logger.log?.Warn("Attempting to Initialize Nightscout Counter");
                Init();
            }
            catch (Exception ex)
            {
                Logger.log?.Error("Nightscout Counter Done screwed up on initialization"); // -Kyle1413
                Logger.log?.Error(ex.ToString());
            }
        }

        private void Init()
        {
            Logger.log.Debug("Creating nightscout counter...");
            gameObject.transform.localScale = Vector3.zero;
            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            CanvasScaler cs = gameObject.AddComponent<CanvasScaler>();
            cs.scaleFactor = 10.0f;
            cs.dynamicPixelsPerUnit = 10f;
            gameObject.AddComponent<GraphicRaycaster>();
            gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
            gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);

            _counter = BeatSaberUI.CreateText(canvas.transform as RectTransform, $"--- mg/dl", Vector2.zero);
            _counter.alignment = TextAlignmentOptions.Center;
            _counter.fontSize = 3f;
            _counter.color = Color.white;
            _counter.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
            _counter.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
            _counter.enableWordWrapping = false;
            _counter.transform.localPosition = new Vector3(0, -5f, 0f);
            BloodSugarUpdater();
        }
        public void BloodSugarUpdater()
        {
            GetBloodSugar();
            InvokeRepeating("GetBloodSugar", 60f, 60f);
        }
        private static string arrowValues(string directionInput)
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

        public async void GetBloodSugar()
        {
            string siteURL = _config.NightscoutSiteURL;
            // do some string manipulation to make sure the URL is valid
            if (siteURL == "https://example.herokuapp.com") return;
            if (siteURL.Substring(siteURL.Length - 1) != "/") siteURL += "/";
            if (siteURL.Substring(0, 7) == "http://") siteURL = "https://" + siteURL.Substring(7);
            if (siteURL.Substring(0, 8) != "https://") siteURL = "https://"+ siteURL;
            
            string responseString = await client.GetStringAsync(siteURL+"api/v1/entries.json?count=1");
            NightscoutData nightscoutData = JsonConvert.DeserializeObject<NightscoutData>(responseString.Substring(1, responseString.Length-2));
            _counter.text = nightscoutData.sgv + " mg/dl";
            if(_config.ArrowsEnabled)
            {
                _counter.text += arrowValues(nightscoutData.direction);
            }
            if(nightscoutData.sgv <= _config.LowThreshold)
            {
                _counter.color = _config.colorFromConfigOption(_config.LowGlucoseColor);
            }
            else if(nightscoutData.sgv >= _config.HighThreshold)
            {
                _counter.color = _config.colorFromConfigOption(_config.HighGlucoseColor);
            }
            else
            {
                _counter.color = _config.colorFromConfigOption(_config.NormalGlucoseColor);
            }
        }

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Logger.log?.Debug($"{name}: OnDestroy()");
            instance = null; // This MonoBehaviour is being destroyed, so set the static instance property to null.

        }
        #endregion
    }
}

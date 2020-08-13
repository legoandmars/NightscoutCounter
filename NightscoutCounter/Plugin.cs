using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using UnityEngine.SceneManagement;
using IPA.Loader;
using UnityEngine;
using BS_Utils;
using CountersPlus.Custom;
using Conf = IPA.Config.Config;

using IPALogger = IPA.Logging.Logger;
using NightscoutCounter.UI;
using NightscoutCounter.Configuration;

namespace NightscoutCounter
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin instance { get; private set; }
        internal static string Name => "NightscoutCounter";

        private NightscoutCounterController _counter;

        internal static BS_Utils.Utilities.Config config = new BS_Utils.Utilities.Config("NightscoutCounter");

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(Conf conf, IPALogger logger)
        {
            instance = this;
            Logger.log = logger;
            Logger.log.Debug("Logger initialized.");
            Configuration.Config.Instance = conf.Generated<Configuration.Config>();
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Logger.log.Debug("OnApplicationStart");
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            Console.WriteLine("Nightscout counter is up and running!");
            if (PluginManager.EnabledPlugins.Any(x => x.Id == "Counters+"))
            {
                Console.WriteLine("Counters+ installed");
                AddCustomCounter();
            }
            else
            {
                Console.WriteLine("Counters+ not installed.");
            }
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Logger.log.Debug("OnApplicationQuit");

        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "GameCore")
            {
                _counter = new GameObject("Nightscout Counter").AddComponent<NightscoutCounterController>();
            }
        }
        private void AddCustomCounter()
        {
            CustomCounter counter = new CustomCounter
            {
                SectionName = "nightscout",
                Name = "Nightscout",
                BSIPAMod = PluginManager.EnabledPlugins.First(x => x.Name == Name),
                Counter = "Nightscout Counter",
                Description = "Reads blood sugar from a nightscout site.",
                Icon_ResourceName = "NightscoutCounter.Images.nightscout-counter.png",
                CustomSettingsResource = "NightscoutCounter.UI.NightscoutSettings.bsml",
                CustomSettingsHandler = typeof(NightscoutSettingsHandler)
            };
            CustomConfigModel defaults = new CustomConfigModel(counter.Name);
            defaults.Enabled = true;
            defaults.Position = CountersPlus.Config.ICounterPositions.AboveMultiplier;
            CustomCounterCreator.Create(counter);
        }

    }
}

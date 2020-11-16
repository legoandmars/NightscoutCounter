using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using NightscoutCounter.Configuration;
using NightscoutCounter.Utilities;

namespace NightscoutCounter
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin instance { get; private set; }
        internal static string Name => "NightscoutCounter";

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger, Config config)
        {
            instance = this;
            Logger.log = logger;
            Logger.log.Debug("Logger initialized.");
            PluginConfig.Instance = config.Generated<PluginConfig>();
        }

        [OnStart]
        public void OnApplicationStart()
        {
            NightscoutDataService.UpdateBloodSugarTask();
            Logger.log.Info("Nightscout counter is up and running!");
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Logger.log.Debug("OnApplicationQuit");
        }
    }
}

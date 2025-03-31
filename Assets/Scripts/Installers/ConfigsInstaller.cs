using TaranaGame.Configs;
using UnityEngine;
using Zenject;

namespace TaranaGame.Installers
{
    [CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "Installers/ConfigsInstaller")]
    public class ConfigsInstaller : ScriptableObjectInstaller<ConfigsInstaller>
    {
        [SerializeField] private GameSettingsConfig _gameSettingsConfig;
        [SerializeField] private TarakansConfig _tarakansConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_gameSettingsConfig).AsCached();
            Container.BindInstance(_tarakansConfig).AsCached();
        }
    }
}
using TaranaGame.Logic;
using Zenject;

namespace TaranaGame.Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSettings>().AsSingle();
        }
    }
}
using TaranaGame.Logic;
using Zenject;

namespace TaranaGame.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        }
    }
}
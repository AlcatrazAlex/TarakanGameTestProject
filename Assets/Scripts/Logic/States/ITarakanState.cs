using TaranaGame.Tarakan;

namespace TaranaGame.Logic.States
{
    public interface ITarakanState
    {
        void Enter(TarakanController controller);
        void Update(TarakanController controller);
        void Exit();
    }
}
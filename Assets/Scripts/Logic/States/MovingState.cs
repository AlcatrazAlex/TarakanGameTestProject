using TaranaGame.Tarakan;
using UnityEngine;

namespace TaranaGame.Logic.States
{
    public class MovingState : ITarakanState
    {
        private Vector2 _targetCorner;
    
        public void Enter(TarakanController controller)
        {
            controller.ResetAcceleration();
            _targetCorner = controller.GetOppositeCorner();
            controller.SetTarget(_targetCorner);
        }

        public void Update(TarakanController controller)
        {
            if(controller.IsMouseInDangerZone())
                controller.SetState(new EscapingState());
        }

        public void Exit() { }
    }
}
using TaranaGame.Tarakan;
using UnityEngine;

namespace TaranaGame.Logic.States
{
    public class EscapingState : ITarakanState
    {
        public float EscapeTimer { get; set; }
        
        private Vector2 _escapeDirection;
        private const float EscapeDelay = 0.6f;
        private const float EscapeDistance = 9f;

        public void Enter(TarakanController controller)
        {
            Vector2 mousePosition = controller.GetMousePosition();
            _escapeDirection = ((Vector2)controller.transform.position - mousePosition).normalized;
            controller.SetTarget((Vector2)controller.transform.position + _escapeDirection * EscapeDistance);
        }

        public void Update(TarakanController controller)
        {
            EscapeTimer += Time.deltaTime;
        
            if(!controller.IsMouseInDangerZone() && EscapeTimer > EscapeDelay)
                controller.SetState(new MovingState());
        }

        public void Exit() { }
    }
}
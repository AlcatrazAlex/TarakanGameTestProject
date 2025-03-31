using TaranaGame.Tarakan;
using TaranaGame.UI;
using UnityEngine;

namespace TaranaGame.Logic.States
{
    public class EscapingState : ITarakanState
    {
        public float EscapeTimer { get; set; }
        
        private Vector2 _escapeDirection;
        private const float EscapeDelay = 0.6f;
        private const float EscapeDistance = 9f;
        private const float EscapeAngle = 45f;
        private const float ScreenMargin = 0.1f;
        
        public void Enter(TarakanController controller)
        {
            var mousePosition = controller.GetMousePosition();
            var currentPosition = controller.transform.position;
            
            var baseDirection = ((Vector2)currentPosition - mousePosition).normalized;
            _escapeDirection = GetEscapeDirection(baseDirection, currentPosition);
            
            controller.SetTarget((Vector2)currentPosition + _escapeDirection * EscapeDistance);
        }

        private Vector2 GetEscapeDirection(Vector2 baseDir, Vector2 currentPos)
        {
            var perpendicularLeft = Quaternion.Euler(0, 0, EscapeAngle) * baseDir;
            var perpendicularRight = Quaternion.Euler(0, 0, -EscapeAngle) * baseDir;
            
            var viewportPos = Camera.main.WorldToViewportPoint(currentPos);
            if(viewportPos.x < ScreenMargin) 
                return perpendicularRight;
            if(viewportPos.x > 1 - ScreenMargin) 
                return perpendicularLeft;
            if(viewportPos.y < ScreenMargin) 
                return Vector2.up;
            if(viewportPos.y > 1 - ScreenMargin) 
                return Vector2.down;
            
            return Random.value > 0.5f ? perpendicularLeft : perpendicularRight;
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
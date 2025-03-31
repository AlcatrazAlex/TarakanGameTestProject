using System;
using TaranaGame.Logic;
using TaranaGame.Logic.States;
using TaranaGame.UI;
using UnityEngine;
using Zenject;

namespace TaranaGame.Tarakan
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TarakanController : MonoBehaviour
    {
        [SerializeField] private Transform _viewRoot;
        
        private ITarakanState _currentState;
        private Rigidbody2D _rb;
        private TarakanViewBase _view;
        private GameSettings _gameSettings;
        private Camera _gameCamera;

        private Vector2 _targetPosition;
        private Vector2 _mousePosition;
        private float _currentSpeed;
        private string _id;
        
        public TarakanViewBase View => _view;
        public ITarakanState CurrentState => _currentState;
        public string Id => _id;
        
        public Action OnPointReached;
        
        
        #region SetUp 

        [Inject]
        public void Init(GameSettings settings)
        {
            _rb = GetComponent<Rigidbody2D>();
            _gameSettings = settings;
            _gameCamera = Camera.main;
        }

        public void SetId(string id)
        {
            _id = id;
        }
        #endregion
        
        #region Mouse

        public void UpdateMousePosition(Vector2 position) => _mousePosition = position;

        public bool IsMouseInDangerZone()
        {
            float distance = Vector2.Distance(transform.position, _mousePosition);
            return distance < _gameSettings.GetOption(OptionType.CursorSize);
        }
        
        public Vector2 GetMousePosition() => _mousePosition;

        #endregion

        #region MainLoop&States

        public void StartTrip()
        {
            SetState(new MovingState());
        }
        
        public void Update()
        {
            _currentState?.Update(this);
        }
        
        public void SetView(TarakanViewBase view)
        {
            _view = view;
            _view.transform.SetParent(_viewRoot);
            _view.transform.localPosition = Vector3.zero;
        }
        

        public void SetState(ITarakanState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter(this);
        }

        public void FixedUpdate()
        {
            UpdateMovement();
            CheckBounds();
        }

        private void UpdateMovement()
        {
            if(_currentState is IdleState)
                return;
            
            var dir = (_targetPosition - (Vector2)transform.position).normalized;
            var baseSpeed = _gameSettings.GetOption(OptionType.Speed);
            var acceleration = _gameSettings.GetOption(OptionType.Acceleration);

            if(_currentState is EscapingState)
            {
                baseSpeed *= _gameSettings.EscapeSpeedMultiplier;
                acceleration *= _gameSettings.EscapeAccelerationMultiplier;
            }

            if(_view != null)
                _view.LookAt(_targetPosition);
            
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, baseSpeed, acceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = dir * _currentSpeed;
            
            var dist = (GetOppositeCorner() - new Vector2(transform.position.x, transform.position.y)).sqrMagnitude;
            if (dist <= _gameSettings.FinishDistance * _gameSettings.FinishDistance)
                FinishTrip();
        }

        private void FinishTrip()
        {
            SetState(new IdleState());
            OnPointReached?.Invoke();
            OnPointReached = null;
        }


        #endregion

        #region Service
        private void CheckBounds()
        {
            Vector3 viewportPos = _gameCamera.WorldToViewportPoint(transform.position);
            viewportPos.x = Mathf.Clamp01(viewportPos.x);
            viewportPos.y = Mathf.Clamp01(viewportPos.y);
            transform.position = _gameCamera.ViewportToWorldPoint(viewportPos);
        }

        public void SetTarget(Vector2 target)
        {
            _targetPosition = target;
        }

        public void ResetAcceleration() => _currentSpeed = 0;
        
        public Vector2 GetOppositeCorner() => 
            _gameCamera.ViewportToWorldPoint(new Vector2(1f, 0f));
        #endregion
    }
}
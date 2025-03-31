using System;
using UnityEngine;
using UnityEngine.UI;

namespace TaranaGame.UI
{
    public class GameEndedScreen : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;

        public event Action OnRestartButtonPressed; 
        
        private void Awake()
        {
            _restartButton.onClick.AddListener(() =>
            {
                OnRestartButtonPressed?.Invoke();
            });
        }
    }
}
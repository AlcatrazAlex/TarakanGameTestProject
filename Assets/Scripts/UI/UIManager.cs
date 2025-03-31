using System.Collections.Generic;
using TaranaGame.Configs;
using TaranaGame.Logic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TaranaGame.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _optionsButton;
        [Space]
        [SerializeField] private OptionsPanelView _optionsPanel;
        [Space]
        [SerializeField] private GameEndedScreen _finishPopup;
        [Space]
        [SerializeField] private RectTransform _mouseArea;
        
        private GameSettings _gameSettings;
        private GameManager _gameManager;
        private GameSettingsConfig _gameSettingsConfig;
        
        private List<IPresenter> _presenters = new List<IPresenter>();
        
        [Inject]
        private void Init(GameSettings gameSettings, GameManager gameManager, GameSettingsConfig gameSettingsConfig)
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
            
            _gameSettings = gameSettings;
            _gameManager = gameManager;
            _gameSettingsConfig = gameSettingsConfig;
            
            _gameManager.OnGameEnded += GameManagerOnOnGameEnded;
            _gameManager.OnGameStarted += GameManagerOnOnGameStarted;

            _finishPopup.gameObject.SetActive(false);
            _finishPopup.OnRestartButtonPressed += FinishPopupOnOnRestartButtonPressed;
            _optionsPanel.gameObject.SetActive(false);
            
            _optionsPanel.Setup(_gameSettingsConfig);
            var optionsPanelPresenter = new OptionsPanelPresenter(_gameSettings, _optionsPanel);
            _presenters.Add(optionsPanelPresenter);

            var mouseAreaPresenter = new MouseAreaPresenter(_mouseArea, gameSettings);
            _presenters.Add(mouseAreaPresenter);
        }

        private void FinishPopupOnOnRestartButtonPressed()
        {
            _startButton.interactable = true;
            _finishPopup.gameObject.SetActive(false);
        }

        private void GameManagerOnOnGameEnded()
        {
            _finishPopup.gameObject.SetActive(true);
        }

        private void GameManagerOnOnGameStarted()
        {
            _startButton.interactable = false;
        }

        private void OnOptionsButtonClicked()
        {
            _optionsPanel.gameObject.SetActive(!_optionsPanel.gameObject.activeInHierarchy);
        }
        
        private void OnStartButtonClicked()
        {
            _gameManager.StartGame();
      
        }

        private void Update()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Update();
            }
        }
    }
}

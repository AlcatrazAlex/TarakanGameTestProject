using TaranaGame.Logic;
using UnityEngine;

namespace TaranaGame.UI
{
    public class MouseAreaPresenter : IPresenter
    {
        private RectTransform _mouseArea; 
        private GameSettings _gameSettings;
        private Vector2 _startSize;
        public MouseAreaPresenter(RectTransform mouseArea, GameSettings gameSettings)
        {
            _mouseArea = mouseArea;
            _gameSettings = gameSettings;
            _startSize = _mouseArea.sizeDelta;
        }

        public void Update()
        {
            _mouseArea.position = Input.mousePosition;
            var size = _gameSettings.GetOption(OptionType.CursorSize);
            _mouseArea.sizeDelta = new Vector2(_startSize.x * size, _startSize.y * size);
        }
    }
}
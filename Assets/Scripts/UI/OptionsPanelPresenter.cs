
using TaranaGame.Logic;

namespace TaranaGame.UI
{
    public class OptionsPanelPresenter : IPresenter
    {
        private GameSettings _gameSettings;
        private OptionsPanelView _optionsPanelView;
        
        public OptionsPanelPresenter(GameSettings gameSettings, OptionsPanelView optionsPanelView)
        {
            _optionsPanelView = optionsPanelView;
            _gameSettings = gameSettings;
            
            _optionsPanelView.OnValueChanged += OptionsPanelViewOnOnValueChanged;
        }

        private void OptionsPanelViewOnOnValueChanged(OptionType type, float value)
        {
            _gameSettings.SetOption(type, value);
        }
        
        public void Update()
        {
        }
    }
}
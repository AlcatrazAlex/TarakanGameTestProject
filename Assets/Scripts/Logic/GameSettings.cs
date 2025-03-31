using System.Collections.Generic;
using TaranaGame.Configs;
using TaranaGame.UI;

namespace TaranaGame.Logic
{
    public class GameSettings
    {
        private Dictionary<OptionType, float> _values = new Dictionary<OptionType, float>();

        public float EscapeAccelerationMultiplier { get; set; }
        public float EscapeSpeedMultiplier { get; set; }
        public float FinishDistance { get; set; }

        public void InitGameSettingsFromConfig(GameSettingsConfig gameSettingsConfig)
        {
            foreach (var value in gameSettingsConfig.Options)
                SetOption(value.OptionType, value.StartValue);
            
            EscapeAccelerationMultiplier = gameSettingsConfig.EscapeAccelerationMultiplier;
            EscapeSpeedMultiplier = gameSettingsConfig.EscapeSpeedMultiplier;
            FinishDistance = gameSettingsConfig.EscapeSpeedMultiplier;
        }
        
        public void SetOption(OptionType type, float value)
        {
            _values[type] = value;
        }

        public float GetOption(OptionType type)
        {
            if(_values.TryGetValue(type, out var option))
                return option;

            return -1;
        }
    }
}
using System.Linq;
using TaranaGame.UI;
using UnityEngine;

namespace TaranaGame.Configs
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Configs/GameSettings")]
    public class GameSettingsConfig : ScriptableObject
    {
        [SerializeField] private float _finishDistance;
        [SerializeField] private float _escapeAccelerationMultiplier = 2f;
        [SerializeField] private float _escapeSpeedMultiplier = 2f;
        [SerializeField] private OptionData[] _options;
        
        public float FinishDistance => _finishDistance;
        public float EscapeAccelerationMultiplier => _escapeAccelerationMultiplier;
        public float EscapeSpeedMultiplier => _escapeSpeedMultiplier;

        public OptionData[] Options => _options;
        public OptionData GetOption(OptionType optionType) => _options.FirstOrDefault(o => o.OptionType == optionType);
    }

    [System.Serializable]
    public class OptionData
    {
        public OptionType OptionType;
        public string OptionName;
        public float StartValue;
        public float MinValue;
        public float MaxValue;
    }
}
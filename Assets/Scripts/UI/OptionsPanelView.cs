using System;
using TaranaGame.Configs;
using TaranaGame.Pooling;
using UnityEngine;

namespace TaranaGame.UI
{
    public class OptionsPanelView : MonoBehaviour
    {
        [SerializeField] private RectTransform _spawnRoot;
        [SerializeField] private OptionsDataLineView _item;
        
        private SimplePool<OptionsDataLineView> _pool;
        
        public event Action<OptionType, float> OnValueChanged;

        public void Setup(GameSettingsConfig gameSettings)
        {
            _pool = new SimplePool<OptionsDataLineView>(_spawnRoot, _item);
            foreach (var option in gameSettings.Options)
            {
                var itm = _pool.Get();
                itm.Setup(option.OptionName, option.OptionType, option.StartValue, option.MinValue, option.MaxValue);
                itm.OnValueChanged += OnItemValueChanged;
            }
        }

        private void OnItemValueChanged(OptionType type, float value)
        {
            OnValueChanged?.Invoke(type, value);
        }
    }
}
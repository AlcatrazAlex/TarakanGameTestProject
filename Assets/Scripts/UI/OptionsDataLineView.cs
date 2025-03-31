using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TaranaGame.UI
{
    public class OptionsDataLineView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueName;
        [SerializeField] private Slider _slider;

        private string _name;
        private OptionType _optionType;
        
        public Action<OptionType, float> OnValueChanged;

        public void Setup(string nameOf, OptionType optionType, float value, float minValue, float maxValue)
        {
            _name = nameOf;
            _optionType = optionType;
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
            _slider.value = value;
            UpdateText();
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void UpdateText()
        {
            _valueName.text = $"{_name} ({_slider.value:F2}/{_slider.maxValue:F2})";
        }

        private void OnSliderValueChanged(float value)
        {
            UpdateText();
            OnValueChanged?.Invoke(_optionType, value);
        }
    }
}
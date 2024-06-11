using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BGMSoundInputSlider : MonoBehaviour // ¿ø°­
{
    [SerializeField]private Slider slider;
    [SerializeField]private TMP_InputField inputField;
    
    private void Start()
    {
        slider.value = GameManager.Instance.audioValue;
        inputField.text = GameManager.Instance.audioValue.ToString();

        slider.onValueChanged.AddListener(OnSliderValueChanged);
        inputField.onEndEdit.AddListener(OnInputFieldValueChanged);
    }

    private void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            slider.value = result;
        }
        GameManager.Instance.audioValue = result / 100;
    }

    private void OnSliderValueChanged(float value)
    {
        inputField.text = value.ToString("F1");
        GameManager.Instance.audioValue = value / 100;
    }
}

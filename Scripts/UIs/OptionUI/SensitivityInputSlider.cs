using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityInputSlider : MonoBehaviour // 원강
{
    [SerializeField]private Slider slider;
    [SerializeField]private TMP_InputField inputField;

    private void Start()
    {
        slider.value = GameManager.Instance.GamelookSensitivity;
        inputField.text = GameManager.Instance.GamelookSensitivity.ToString();

        slider.onValueChanged.AddListener(OnSliderValueChanged);
        inputField.onEndEdit.AddListener(OnInputFieldValueChanged);
    }

    private void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            slider.value = result;
        }
        GameManager.Instance.GamelookSensitivity = result / 100f;
    }

    private void OnSliderValueChanged(float value)
    {
        inputField.text = value.ToString("F1");
        GameManager.Instance.GamelookSensitivity = value / 100f;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float regenRate;
    public Image uiBar;
    public Image uiBg;
    public Color color;

    private void Start()
    {
        curValue = startValue;
        color = uiBar.color;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();

        if (GetPercentage() < 1f)
        {
            uiBg.color = color;
            uiBar.color = color;
        }
        else
        {
            uiBg.color = Color.clear;
            uiBar.color = Color.clear;
        }
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Substract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}

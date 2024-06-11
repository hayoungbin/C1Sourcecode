using System;
using UnityEngine;

public interface IDamagable
{
    void TakeDown();
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UIConditions uiCondition;
    public GameObject jumpscare;

    Condition stamina { get { return uiCondition.stamina; } }
        
    public event Action onTakeDamage;

    private void Update()
    {
        stamina.Add(stamina.regenRate * Time.deltaTime);
    }


    public bool UseStamina(float amount)
    {
        if(stamina.curValue - amount < 0)
        {
            return false;
        }
        stamina.Substract(amount);
        return true;
    }

    public void TakeDown()
    {
        jumpscare.SetActive(true);
        JumpscareManager.Instance.Jumpscare();
    }
}

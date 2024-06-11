using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConditions : MonoBehaviour
{
    public Condition stamina;
    //public Condition health;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}

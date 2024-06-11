using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Equip
{
    public Light flashlight;

    public bool canTurnOff;

    private void Awake()
    {
        flashlight = GetComponentInChildren<Light>();
    }

    private void Start()
    {
        if (!TutorialManager.Instance.tutorialstart && !TutorialManager.Instance.tutorialstartClear)
        {
            StartCoroutine(TutorialManager.Instance.OnCompleteToDo("start"));
            StartCoroutine(TutorialManager.Instance.OnTutorialMassage("마침 손전등이 있어서 다행이야"));
            TutorialManager.Instance.tutorialstartClear = true;
        }
        CharacterManager.Instance.Player.movement.flashlight = this.gameObject;
    }

    public override void UseFlashlight()
    {
        if (canTurnOff)
        {
            canTurnOff = false;
            flashlight.enabled = false;
        }
        else
        {
            canTurnOff = true;
            flashlight.enabled = true;
        }
    }
}

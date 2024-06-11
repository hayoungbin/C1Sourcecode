using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareManager : MonoBehaviour
{
    public static JumpscareManager Instance;

    public bool isDead;

    public AudioClip catchPlayer;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        isDead = false;
    }

    private void Start()
    {
        
    }

    public void Jumpscare()
    {
        isDead = true;
        ChangePlayerMovementControll();

        // »ç¿îµå
        SoundManager.instance.SFXPlay("CatchPlayer", catchPlayer);
        CharacterManager.Instance.Player.controller.enabled = false;
        CharacterManager.Instance.Player.movement.enabled = false;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(GameOver());
    }

    public void ChangePlayerMovementControll()
    {
        CharacterManager.Instance.Player.controller.enabled = !isDead;
        CharacterManager.Instance.Player.movement.enabled = !isDead;

        if(Cursor.lockState == CursorLockMode.Locked )
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(isDead == false) { return; }
        isDead = false;
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(6f);
        GameManager.Instance.GameOver();
    }
}

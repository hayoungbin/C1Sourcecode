using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject gameClear;
    public GameObject crossHair;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}

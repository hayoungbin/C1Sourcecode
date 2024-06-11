using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Pad : MonoBehaviour, IInteractable
{
    public ObjectsSO objectsSO;
    public GameObject padUI;

    public string GetInteractPrompt()
    {
        string str = $"{objectsSO._name}\n{objectsSO.description}";
        return str;
    }

    public void ObjectInteract()
    {
        if (GameManager.Instance.isClearSwitch)
        {            
            Instantiate(padUI, GameObject.Find("Canvas").transform);
            Cursor.lockState = CursorLockMode.None;            
            Cursor.visible = true; // TODO : 임시로 마우스 토글 기능 만듦 나중에 수정할 수 있으면 수정
        }
        else
        {
            StartCoroutine(GameManager.Instance.OnSystemText("먹통이야"));
        }
    }
}

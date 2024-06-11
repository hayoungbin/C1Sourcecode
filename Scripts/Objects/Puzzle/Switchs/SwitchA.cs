using TMPro;
using UnityEngine;
public class SwitchA : Switchs, IInteractable
{
    public AudioClip setDown;

    private void Start()
    {
        hintText.text = GameManager.Instance.passward2.ToString();
    }
    public string GetInteractPrompt()
    {
        string str = $"{objectsSO._name}\n{objectsSO.description}";
        return str;
    }

    public void ObjectInteract()
    {
        if (GameManager.Instance.is1stSwitch == true)
        {
            OnSwitch();
        }
        else
        {
            StartCoroutine(GameManager.Instance.OnSystemText("아직 작동하지 않는다."));
            Debug.Log("1번 스위치가 켜지지 않았음");
        }
    }

    protected override void OnSwitch()
    {
        base.OnSwitch();
        if (GameManager.Instance.isClearSwitch == true)
        {
            StartCoroutine(GameManager.Instance.OnSystemText("이러고 있을 시간 없어.", Color.yellow));
        }
        else if (GameManager.Instance.randam == 1)
        {
            SoundManager.instance.SFXPlay("setDown", setDown);
            GameManager.Instance.is2ndSwitch = true;
            objSwitch.transform.localRotation = Quaternion.Euler(-1, 0, 0);
            GameManager.Instance.ClearPuzzle();
        }
        else
        {
            GameManager.Instance.ClearPuzzle();
        }
    }
}

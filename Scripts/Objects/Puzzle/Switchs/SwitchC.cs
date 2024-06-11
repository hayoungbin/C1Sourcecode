using UnityEngine;

public class SwitchC : Switchs , IInteractable
{
    public AudioClip setDown;
    private void Start()
    {
        hintText.text = GameManager.Instance.passward1.ToString();
    }
    public string GetInteractPrompt()
    {
        string str = $"{objectsSO._name}\n{objectsSO.description}";
        return str;
    }

    public void ObjectInteract()
    {
        OnSwitch();
    }

    protected override void OnSwitch()
    {
        base.OnSwitch();
        if (GameManager.Instance.isClearSwitch == true)
        {
            StartCoroutine(GameManager.Instance.OnSystemText("이러고 있을 시간 없어.", Color.yellow));
        }
        else if (GameManager.Instance.is1stSwitch == false)
        {
            SoundManager.instance.SFXPlay("setDown", setDown);
            GameManager.Instance.is1stSwitch = true;
            objSwitch.transform.localRotation = Quaternion.Euler(-1, 0, 0);
            StartCoroutine(GameManager.Instance.OnSystemText("무언가가 작동한 것 같다."));
            Debug.Log("첫번째차단기");
            DataManager.Instance.SaveData(QuestState.FirstSwitch);
        }
        else if (GameManager.Instance.is1stSwitch == true)
        {
            StartCoroutine(GameManager.Instance.OnSystemText("이미 작동중이야"));
        }
    }
}
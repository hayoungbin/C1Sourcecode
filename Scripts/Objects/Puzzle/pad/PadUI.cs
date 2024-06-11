using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PadUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI number;
    private string curPasswad;
    private int?[] input = new int?[4];

    public AudioClip clickButton;
    public AudioClip clearClip;

    private void Start()
    {
        for (int i = 0; i < input.Length; i++)
        {
            input[i] = null;
        }
        UpdateNum();
    }
    private void UpdateNum()
    {
        number.text = $"{input[0]}  {input[1]}  {input[2]}  {input[3]}";
        curPasswad = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString();
    }
    public void CommitBtn()
    {
        if (curPasswad == GameManager.Instance.passward)
        {
            SoundManager.instance.SFXPlay("ClearClip", clearClip);
            Debug.Log("클리어!!!");
            GameManager.Instance.isClearGame = true;
            CharacterManager.Instance.Player.movement.ToggleCursor();
            GameManager.Instance.GameEnd();
            Destroy(GameObject.Find("PadUI(Clone)"));
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; // TODO : 임시로 마우스 토글 기능 만듦 나중에 수정할 수 있으면 수정
        }
        else
        {
            Debug.Log("실패!!!!!!!");
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = null;
            }
            UpdateNum();
        }

    }
    public void BackBtn()
    {
        Destroy(GameObject.Find("PadUI(Clone)"));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // TODO : 임시로 마우스 토글 기능 만듦 나중에 수정할 수 있으면 수정
    }
    public void NumBtn1()
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == null)
            {
                input[i] = 1;
                SoundManager.instance.SFXPlay("ClickButton", clickButton);
                UpdateNum();
                return;
            }
        }
        if (input[3] != null)
        {
            return;
        }
    }
    public void NumBtn2()
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == null)
            {
                input[i] = 2;
                SoundManager.instance.SFXPlay("ClickButton", clickButton);
                UpdateNum();
                return;
            }
        }
        if (input[3] != null)
        {
            return;
        }
    }
    public void NumBtn3()
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == null)
            {
                input[i] = 3;
                SoundManager.instance.SFXPlay("ClickButton", clickButton);
                UpdateNum();
                return;
            }
        }
        if (input[3] != null)
        {
            return;
        }
    }
    public void NumBtn4()
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == null)
            {
                input[i] = 4;
                SoundManager.instance.SFXPlay("ClickButton", clickButton);
                UpdateNum();
                return;
            }
        }
        if (input[3] != null)
        {
            return;
        }
    }
    public void NumBtn5()
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == null)
            {
                input[i] = 5;
                SoundManager.instance.SFXPlay("ClickButton", clickButton);
                UpdateNum();
                return;
            }
        }
        if (input[3] != null)
        {
            return;
        }
    }
    public void NumBtn6()
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == null)
            {
                input[i] = 6;
                SoundManager.instance.SFXPlay("ClickButton", clickButton);
                UpdateNum();
                return;
            }
        }
        if (input[3] != null)
        {
            return;
        }
    }
    public void NumBtn7()
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == null)
            {
                input[i] = 7;
                SoundManager.instance.SFXPlay("ClickButton", clickButton);
                UpdateNum();
                return;
            }
        }
        if (input[3] != null)
        {
            return;
        }
    }
    public void NumBtn8()
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == null)
            {
                input[i] = 8;
                SoundManager.instance.SFXPlay("ClickButton", clickButton);
                UpdateNum();
                return;
            }
        }
        if (input[3] != null)
        {
            return;
        }
    }
    public void NumBtn9()
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == null)
            {
                input[i] = 9;
                SoundManager.instance.SFXPlay("ClickButton", clickButton);
                UpdateNum();
                return;
            }
        }
        if (input[3] != null)
        {
            return;
        }
    }
    public void NumBtn0()
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == null)
            {
                input[i] = 0;
                SoundManager.instance.SFXPlay("ClickButton", clickButton);
                UpdateNum();
                return;
            }
        }
        if (input[3] != null)
        {
            return;
        }
    }
}

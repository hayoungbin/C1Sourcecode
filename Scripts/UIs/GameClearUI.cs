using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearUI : MonoBehaviour
{
    public Image bg;
    public Image background;
    public TextMeshProUGUI text;

    private void Awake()
    {
        background.color = new Color(255, 255, 255, 0);
        text.color = new Color(0, 0, 0, 0);
    }

    private void Start()
    {
        UIManager.Instance.crossHair.SetActive(false);
        StartCoroutine(BackgroundCoroutine());
        StartCoroutine(TextCoroutine());
    }

    IEnumerator BackgroundCoroutine()
    {
        float fadeCnt = 0;
        while (fadeCnt < 1.0f)
        {
            fadeCnt += 0.01f;
            yield return new WaitForSeconds(0.01f);
            background.color = new Color(230, 230, 230, fadeCnt);
        }
    }

    IEnumerator TextCoroutine()
    {
        float fadeCnt = 0;
        while (fadeCnt < 1.0f)
        {
            fadeCnt += 0.01f;
            yield return new WaitForSeconds(0.01f);
            text.color = new Color(0, 0, 0, fadeCnt);
        }
    }

    public void GameClear()
    {
        SceneManager.LoadScene("StartScene");
    }
}

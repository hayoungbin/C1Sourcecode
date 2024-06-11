using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    public GameObject gameOver;
    public Image back;
    public TextMeshProUGUI text;
    public Color color;

    public GameObject jumpscare;

    public float fadeSpeed = 1.5f;

    public bool fadeInOnStart = true;
    public bool fadeOutOnExit = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
    }

    public void StartFadeIn()
    {
        gameOver.SetActive(true);

        if (fadeInOnStart)
        {
            color.a = 0f;
            back.color = color;
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        while (color.a <= 1f)
        {
            color.a += Time.deltaTime * fadeSpeed;
            back.color = color;
        }
        yield return null;
    }

    IEnumerator FadeOut()
    {
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            back.color = color;
        }
        yield return null;
    }

    public void ReStart()
    {
        StartCoroutine(FadeOut());
        gameOver.SetActive(false);

        // player data 불러오기
        DataManager.Instance.LoadPlayerDataFromJson();
        // 게임 재시작
        GameManager.Instance.GameReStart();

        jumpscare.SetActive(false);
    }
}

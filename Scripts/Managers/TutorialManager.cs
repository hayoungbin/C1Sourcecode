using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager _instance;

    [SerializeField] private GameObject tutorialMassage;
    [SerializeField] private GameObject toDoSlot;

    private Canvas canvas;
    [SerializeField]private GameObject slots;
    private TextMeshProUGUI tutorialText;
    private List<ToDoListData> toDoListData;

    public bool tutorialstart = true;
    public bool tutorialstartClear = false;
    public bool tutoriallock = true;
    public bool tutoriallockClear = false;

    public static TutorialManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("TutorialManager").AddComponent<TutorialManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        tutorialText = tutorialMassage.GetComponentInChildren<TextMeshProUGUI>();
        toDoListData = new List<ToDoListData>();
    }
    private void Start()
    {
        if (tutorialstart)
        {
            StartCoroutine(OnTutorialMassage("여긴 어디지? 너무 어둡다... 뭔가 빛을 비출 만한 게 없나?", "조명이 될 만한 것을 찾자", "start"));
            tutorialstart = false;
        }
    }

    public void ClearLock()
    {
        StartCoroutine(OnTutorialMassage("열었으니 됐지 뭐"));
        StartCoroutine(OnCompleteToDo("lock"));
        tutoriallockClear = true;
    }
    public void UpdateToDo()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Slots");
        for (int i = 0; i < obj.Length; i++)
        {
            Destroy(obj[i]);
        }
        TextMeshProUGUI text;
        for(int i = 0; i < toDoListData.Count; i++)
        {
            GameObject go = Instantiate(toDoSlot, slots.transform);
            text = go.GetComponentInChildren<TextMeshProUGUI>();
            text.text = toDoListData[i]._text;
        }
    }
    public IEnumerator OnTutorialMassage(string massage, string text, string title)
    {
        tutorialText.text = massage;
        if (GameObject.Find("TutorialMassage(Clone)") != null)
        {
            Destroy(GameObject.Find("TutorialMassage(Clone)"));
        }
        GameObject go = Instantiate(tutorialMassage, canvas.transform);
        yield return new WaitForSeconds(3);
        Destroy(go);
        ToDoListData start = new ToDoListData(text, title);
        toDoListData.Add(start);
        UpdateToDo();
        tutorialText.text = null;
    }
    public IEnumerator OnTutorialMassage(string massage)
    {
        tutorialText.text = massage;
        GameObject go = Instantiate(tutorialMassage, canvas.transform);
        yield return new WaitForSeconds(3);
        Destroy(go);
        tutorialText.text = null;
    }
    public IEnumerator OnCompleteToDo(string title)
    {
        for (int i = 0; i < toDoListData.Count; i++)
        {
            if (toDoListData[i]._title == title)
            {
                toDoListData[i]._text = $"<s>{toDoListData[i]._text}</s>";
                break;
            }
        }
        UpdateToDo();

        yield return new WaitForSeconds(1);

        for (int i = 0; i < toDoListData.Count; i++)
        {
            if (toDoListData[i]._title == title)
            {
                toDoListData.Remove(toDoListData[i]);
                break;
            }
        }
        UpdateToDo();
    }
}

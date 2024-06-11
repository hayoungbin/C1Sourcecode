using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum QuestState
{
    // state : 자물쇠를 부순 후
    First_Quest,
    // state : 차단기를 한 개 내린 후
    FirstSwitch,
    // state : 차단기를 모두 내린 후
    SwitchClear
}

[System.Serializable]
public class QuestLine
{
    public bool is1st;
    public bool is12st;
    public bool isClear;

    public void QuestData(QuestState questState)
    {
        QuestState state = questState;

        switch (state)
        {
            case QuestState.First_Quest:
                break;
            case QuestState.FirstSwitch:
                is1st = true;
                is12st = false;
                isClear = false;
                break;
            case QuestState.SwitchClear:
                is1st = true;
                is12st = true;
                isClear = true;
                break;
        }
    }
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private GameObject systemTextObj;
    private TextMeshProUGUI systemText;
    public GameObject hintMemo;
    [HideInInspector] public TextMeshProUGUI hintMassage;

    public bool is1stSwitch = false;
    public bool is2ndSwitch = false;
    public bool isClearSwitch = false;
    public bool isClearGame = false;
    public int randam = 0;
    public int? passward1;
    public int? passward2;
    public int? passward3;
    public int? passward4;
    public string passward = "1";

    [HideInInspector]public bool canDestroy = false; // 원강, 자물쇠 파괴 관련
    [HideInInspector]public bool isOpening = false; // 원강, 빅door 문 관련
    [HideInInspector]public bool isOpne = false; // 원강, 빅 door 문 관련 변수 

    [HideInInspector]public float GamelookSensitivity = 0.1f; //원강, 옵션 감도 저장 할 곳
    [HideInInspector]public float audioValue = 0.1f; //원강, 옵션 브금설정 저장 할 곳
    // 코드보다 -> 인스펙터 나중순서

    // 수현, 몬스터 스폰 위치
    public NPC npc;
    public GameObject Enemy;
    public Transform[] monsterSpawnPoints;
    public Transform monsterSpawnPosition;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
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
        
        hintMassage = hintMemo.GetComponentInChildren<TextMeshProUGUI>();

        // TODO : randam에 저장된 값을 받아오는 기능 필요
        if (randam == 0) // TODO : rarandam에 저장된 값이 null일 경우로 조건을 변경
        {
            randam = Random.Range(1, 3);
        }
        else
        {
            // TODO : null이 아닐 경우 randam값을 불러오기
        }

        passward1 = Random.Range(0, 10);
        passward2 = Random.Range(0, 10);
        passward3 = Random.Range(0, 10);
        passward4 = Random.Range(0, 10);

        passward = $"{passward1}" + $"{passward2}" + $"{passward3}" + $"{passward4}";

        systemText = systemTextObj.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void MonsterData(GameObject enemy)
    {
        // 몬스터 재스폰 임의 위치 가져옴
        Enemy = enemy;
        npc = enemy.GetComponent<NPC>();
        monsterSpawnPoints = npc.points;

        int i = Random.Range(0, npc.points.Length);
        monsterSpawnPosition = npc.points[i];
    }

    public void QuestInit()
    {
        is1stSwitch = CharacterManager.Instance.Player.questLine.is1st;
        is2ndSwitch = CharacterManager.Instance.Player.questLine.is12st;
        isClearSwitch = CharacterManager.Instance.Player.questLine.isClear;
    }

    public void ClearPuzzle()
    {
        if(is1stSwitch && is2ndSwitch)
        {
            StartCoroutine(OnSystemText("좋아, 드디어 나갈 수 있겠군"));
            isClearSwitch = true;
            Debug.Log("완료!!!");
            // TODO : 퍼즐이 완료되면 작동할 기능을 구현
            DataManager.Instance.SaveData(QuestState.SwitchClear);
        }
        else if (is1stSwitch && !is2ndSwitch)
        {
            StartCoroutine(OnSystemText("무언가가 잘 못 된 것 같다...", Color.red));
            Debug.Log("실패!!!");
            // TODO : 그 외에 퍼즐이 틀렸을 때 작동할 기능을 구현
        }
    }

    public void SpawnMonster()
    {
        Enemy.SetActive(true);
        Enemy.transform.position = monsterSpawnPosition.position;
    }

    public void GameOver()
    {
        // TODO : 게임 패배 구현
        GameOverManager.instance.StartFadeIn();
    }

    public void GameEnd()
    {
        // TODO : 게임 클리어 구현
        Destroy(Enemy);
        UIManager.Instance.gameClear.SetActive(true);
    }

    public void GameReStart()
    {
        QuestInit();

        SpawnMonster();
        SpawnPlayer();

        // 플레이어 움직임 해금
        JumpscareManager.Instance.ChangePlayerMovementControll();
    }

    private void SpawnPlayer()
    {
        CharacterManager.Instance.Player.gameObject.transform.position = CharacterManager.Instance.Player.playerData.position;
        CharacterManager.Instance.Player.EquipItemLsit = CharacterManager.Instance.Player.playerData.equipItems;
        CharacterManager.Instance.Player.ItemLsit = CharacterManager.Instance.Player.playerData.itemList;
    }

    public IEnumerator OnSystemText(string massage)
    {
        systemText.text = massage;
        systemText.color = Color.white;
        if (GameObject.Find("SystemText(Clone)") != null)
        {
            Destroy(GameObject.Find("SystemText(Clone)"));
        }
        GameObject text = Instantiate(systemTextObj, GameObject.Find("Canvas").transform);
        yield return new WaitForSeconds(2);
        Destroy(text);
    }

    public IEnumerator OnSystemText(string massage ,Color color)
    {
        systemText.text = massage;
        systemText.color = color;
        if (GameObject.Find("SystemText(Clone)") != null)
        {
            Destroy(GameObject.Find("SystemText(Clone)"));
        }
        GameObject text = Instantiate(systemTextObj, GameObject.Find("Canvas").transform);
        yield return new WaitForSeconds(2);
        Destroy(text);
        systemText.color = Color.white;
    }
}

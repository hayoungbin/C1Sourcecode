# 게임개발 숙련주차 팀 과제 : 3D 퍼즐 플랫폼 게임
---

게임개발 숙련주차 C1조 '1조라도 안보이면' 입니다.

저희가 만든 게임의 제목은 'IT :In The Abandoned'이며 장르는 3D공포게임입니다.

저희는 이번 숙련주차의 과제로 3D공포게임을 만들기로 결정했는데, 그 이유를 설명드리자면

저희 팀원 중에서 개인과제를 공포 분위기로 만드신 분이 계셨는데, 거기서 영감을 받아

공포게임도 일종의 퍼즐게임 아닌가? 라는 생각으로 만들게 되었습니다.

---

## 팀원 역할 배분

---

하영빈 : 게임 진행에 관련된 퍼즐과 튜토리얼 메세지들 구현

김수현 : 플레이어에 관련된 내용들과 데이터 저장을 구현

김진영 : 맵 에셋을 다듬고 구성하는 작업과 사운드 시스템을 구현

황원강 : 아이템 오브젝트들에 관한 내용과 인벤토리 시스템을 구현

우승수 : 추격자에 관한 내용과 시작씬을 구성

---

## 필수 구현사항들

---

아래는 발제에서 제시된 필수 구현사항들입니다.

---

### 1.퍼즐 디자인

---

저희 게임의 목표는 알맞은 차단기들을 내리고 키패드를 활성화시켜 암호를 입력하고 건물의 밖으로 나가는 것 입니다.

![image](https://github.com/hayoungbin/C13DTeamProject/assets/167050593/c436065d-8215-416e-a549-38210dc136b6)

위와 같은 차단기 오브젝트들이 맵에 3개 존재하고, 차단기마다 번호가 적혀있습니다.


![image](https://github.com/hayoungbin/C13DTeamProject/assets/167050593/5a571e6b-1376-406d-915c-58234262bb85)

쪽지 아이템으로 내려야하는 차단기와 비밀번호를 유추할 수 있습니다.


![image](https://github.com/hayoungbin/C13DTeamProject/assets/167050593/0bb28e9d-bbc4-4bf2-bf25-b0a1ff7f4d20)

현관문 옆의 키패드입니다. 알맞은 차단기를 두개 내려야 작동하며, 아직 에셋이 없어서 큐브입니다...


![image](https://github.com/hayoungbin/C13DTeamProject/assets/167050593/2aa5e3d4-e9f6-4b67-99cc-361407cf8cad)

키패드를 활성화시켜서 비밀번호를 입력하면


![image](https://github.com/hayoungbin/C13DTeamProject/assets/167050593/018be8f7-d3d4-4461-9d33-24946b122f52)

게임을 클리어하게 됩니다.



---

### 2.플레이어 캐릭터 및 컨트롤

---

플레이어의 움직임은 Input시스템의 Send Massages 형식을 사용해서 구현해보았습니다

![image](https://github.com/hayoungbin/C13DTeamProject/assets/167050593/5b526b34-f36b-4488-9534-e2e9e0816fa8)

![image](https://github.com/hayoungbin/C13DTeamProject/assets/167050593/da59923f-048e-4160-b2ee-4a82053d0023)

플레이어의 이동을 구현하는 요소는 아래와 같이 이루어져 있습니다.


인풋값을 받아오고 플레이어의 행동을 정의하는 PlayerInputController.cs

```cs
<C#>

public class PlayerInputController : Controller
{
    private Vector2 curMovementInput;
    private Vector2 mouseDelta;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputValue value)
    {
        curMovementInput = value.Get<Vector2>();
        CallMoveEvent(curMovementInput);
    }

    public void OnLook(InputValue value)
    {
        mouseDelta = value.Get<Vector2>();
        CallLookEvent(mouseDelta);
    }

    public void OnRun(InputValue value)
    {
        float runValue = value.Get<float>();
        CallRunEvent(runValue);
    }

    public void OnInventory(InputValue value)
    {
        CallInventoryEvent();
    }

    public void OnInteract(InputValue vlaue)
    {
        CallInteracEvent();
    }

    public void OnMenu(InputValue vlaue)
    {
        CallMenuEvent();
    }
}
```

플레이어의 움직임을 실제로 구현하는 Movement.cs

```cs
<C#>
public class Movement : MonoBehaviour
{
    [Header("# Movement")]
    public float speed;
    public float moveSpeed;
    public float runSpeed;
    public float useStamina;

    [Header("# Look")]
    public Transform cameraContainer;
    public Camera cam;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot; // 카메라 회전
    [HideInInspector]public float lookSensitivity;
    private Vector2 mouseDelta;

    [HideInInspector]
    public bool isRun = false;
    [HideInInspector]
    public bool canLook = true;

    private Rigidbody movementRigidbody;
    private FootStepsSound footStepsSound;

    public GameObject flashlight = null;
    public GameObject menuObject;
    public GameObject optionObject;

    Vector2 movementDir = Vector2.zero;

    private void Awake()
    {
        speed = moveSpeed;
        cam = UnityEngine.Camera.main;
        movementRigidbody = GetComponent<Rigidbody>();
        footStepsSound = GetComponent<FootStepsSound>();
    }

    private void Start()
    {
        CharacterManager.Instance.Player.controller.OnMoveEvent += Move;
        CharacterManager.Instance.Player.controller.OnInventoryEvent += UseInventory;
        CharacterManager.Instance.Player.controller.OnRunEvent += Run;
        CharacterManager.Instance.Player.controller.OnLookEvent += Camera;

        CharacterManager.Instance.Player.controller.OnMenuEvent += OpneMenu;
        CharacterManager.Instance.Player.controller.OnMenuEvent += ToggleCursor;

    }

    private void FixedUpdate()
    {
        if (isRun)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                footStepsSound.SetRunning(true);
                speed = runSpeed;                
            }
            else
            {
                footStepsSound.SetRunning(false);
                speed = moveSpeed;                
            }
        }
        else
        {
            footStepsSound.SetRunning(false);
            speed = moveSpeed;            
        }

        ApplyMovement(movementDir);
    }

    private void LateUpdate()
    {
        if(canLook)
        {
            CameraLook();
        }
    }

    private void Move(Vector2 direction)
    {
        movementDir = direction;
    }

    private void ApplyMovement(Vector2 direction)
    {
        Vector3 dir = transform.forward * direction.y + transform.right * direction.x;
        dir *= speed;
        dir.y = movementRigidbody.velocity.y;

        movementRigidbody.velocity = dir;
    }

    private void Run(float value)
    {
        if(isRun == false)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }
    }

    private void UseInventory()
    {
        ToggleCursor();
    }

    public void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    private void Camera(Vector2 vector)
    {
        mouseDelta = vector;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * GameManager.Instance.GamelookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cam.transform.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        if(flashlight != null)
        {
            flashlight.transform.localEulerAngles = new Vector3(camCurXRot - 90, 180, 0);
        }

        transform.eulerAngles += new Vector3(0, mouseDelta.x * GameManager.Instance.GamelookSensitivity, 0);
    }

    private void OpneMenu()
    {
        optionObject.SetActive(false);

        menuObject.SetActive(!menuObject.activeSelf);
    }
}
```

---

### 3.퍼즐 해결 시스템

---

저희 게임의 클리어 목표인 차단기와 비밀번호는 GameManager.cs에서 생성되는 랜덤한 변수에 의해 결정됩니다.

해당 내용은 다음과같은 코드로 이루어져 있습니다.

```cs
<C#>
public class GameManager : MonoBehaviour
{
    public int randam = 0;
    public int? passward1;
    public int? passward2;
    public int? passward3;
    public int? passward4;
    public string passward = "1";

    private void Awake()
    {
        if (randam == 0)
        {
            randam = Random.Range(1, 3); // 차단기 변수
        }

        passward1 = Random.Range(0, 10);
        passward2 = Random.Range(0, 10);
        passward3 = Random.Range(0, 10);
        passward4 = Random.Range(0, 10);

        passward = $"{passward1}" + $"{passward2}" + $"{passward3}" + $"{passward4}"; // 최종 암호 변수

        systemText = systemTextObj.GetComponentInChildren<TextMeshProUGUI>();
    }
}
```

그리고 힌트를 한 글자씩 나눠서 주기 위해 최종암호를 4개로 나눠서 따로 변수로 지정해 두었으며 힌트는 아래와 같은 코드로 쪽지에 나타나게 됩니다.

```cs
<C#>
public class Hints : MonoBehaviour
{
    public void SetHints(int num)
    {
        switch (num)
        {
            case 1:
                SetHint1();
                break;
            case 2:
                SetHint2();
                break;
            default:
                break;
        }
    }

    private void SetHint1()
    {
        string str1;
        string str2;

        if (GameManager.Instance.randam == 1)
        {
            str1 = "2층 휴게실";
            str2 = "지하 끝 방";
        }
        else
        {
            str1 = "지하 끝 방";
            str2 = "2층 휴게실";
        }

        GameManager.Instance.hintMassage.text = "야간 순찰 당번에게\n" +
            "\n" +
            "퇴근하기 전에 1층 현관 옆의 관리실에 있는 차단기를 내리는 걸 잊지마.\n" +
            "그리고 1층, 2층, 지하순으로 둘러봐주고, 차단기가 잘 작동하는지도 확인해주고\n" +
            $"{str1}에 있는 차단기도 확실히 내려줘\n" +
            $"아, 그리고 {str2}에 있는 차단기는 왠만하면 건드리지마\n" +
            "왜인지는 나도 몰라, 그냥 여기 주인장이 그러던데\n" +
            "무슨 일이 있어도 절대로 건드리지 말라고 그러더라\n" +
            "그럴거면 그냥 없애는게 낫지 않나 싶기도 한데...";
    }
    private void SetHint2()
    {
        GameManager.Instance.hintMassage.text = $"X X X {GameManager.Instance.passward4}";
    }
}
```

---

### 4.장애물 및 트랩

---

저희 게임에서는 공포게임인 만큼 추격자가 장애물과 트랩의 역할을 합니다.

![image](https://github.com/hayoungbin/C13DTeamProject/assets/167050593/07461ac3-cadc-4584-88a3-50053ba1c158)

추격자는 NPC.cs 사용해 맵을 배회하고, 플레이어를 추격합니다.

```cs
<C#>
public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Fleeing
}

public class NPC : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public GameObject hints2;

    [Header("AI")]
    private AIState aiState;
    public float detectDistance;
    public float safeDistance;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    public Transform[] points;//AI 이동하는 지점

    public AudioClip clip;
    private float playerDistance;
    private bool hasBgm = false;
    private bool fitem = false;

    public float fieldOfView = 120f; //몬스터 좌우 시야각
    public float verticalFieldOfView = 90f;//몬스터 상하 시야각

    private int destPoint = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        GameManager.Instance.MonsterData(this.gameObject);
    }

    private void Start()
    {
        SetState(AIState.Wandering);
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        animator.SetBool("Moving", aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
                PassiveUpdate();
                break;
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
            case AIState.Fleeing:
                FleeingUpdate();
                break;
        }
    }

    private void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
            case AIState.Fleeing:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            GotoNextPoint();
            //Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if (playerDistance < detectDistance && IsPlayerInFieldOfView())
        {
            if (!fitem)
            {
                DropItem();
                //Instantiate(dropOn ,transform.position + Vector3.up * 2, Quaternion.identity);
                fitem = true;
            }
            Debug.Log("Player detected!");
            SetState(AIState.Attacking);
        }
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;

    }

    void AttackingUpdate()
    {
        if (playerDistance > detectDistance)
        {
            SoundManager.instance.bgSound.Pause();
            hasBgm = false;
            agent.isStopped = false;
            SetState(AIState.Wandering);
            GotoNextPoint();
        }
        else if (playerDistance > attackDistance || !IsPlayerInFieldOfView())
        {
            if (!hasBgm)
            {

                SoundManager.instance.BgSoundPlay(clip);
                hasBgm = true;
            }
            agent.isStopped = false;
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
            {
                agent.SetDestination(CharacterManager.Instance.Player.transform.position);
            }
            else
            {
                SetState(AIState.Fleeing);
            }
        }
        else
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate)
            {
                animator.speed = 1;
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;
                CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakeDown();
                gameObject.SetActive(false);
            }
        }
    }

    void FleeingUpdate()
    {
        if (agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(GetFleeLocation());
        }
        else
        {
            SetState(AIState.Wandering);
            GotoNextPoint();
        }
    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle)
        {
            return;
        }
        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    bool IsPlayerInFieldOfView()
    {

        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;

        if (Mathf.Abs(directionToPlayer.y) > 2)
        {
            return false;
        }
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        //float verticalAngle = Vector3.Angle(transform.up, directionToPlayer);



        // x축과 y축 각도가 각각의 시야각 범위 내에 있는지 확인
        //bool isInHorizontalFOV = angle < fieldOfView * 0.5f;
        //bool isInVerticalFOV = verticalAngle < verticalFieldOfView * 0.5f;


        return angle < fieldOfView * 0.5f;
        //return isInHorizontalFOV && isInVerticalFOV;
    }

    Vector3 GetFleeLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (GetDestinationAngle(hit.position) > 90 || playerDistance < safeDistance)
        {

            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - CharacterManager.Instance.Player.transform.position, transform.position + targetPos);
    }

    IEnumerator DamageFlash()
    {
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f);
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = Color.white;
    }
    void GotoNextPoint()
    {
        // 설정된 목적지가 없는 경우 반환합니다
        if (points.Length == 0)
            return;
        // 배열에서 랜덤한 인덱스를 선택합니다.
        int randomIndex = Random.Range(0, points.Length);

        // 에이전트가 랜덤으로 선택된 목적지로 이동하도록 설정합니다.
        agent.destination = points[randomIndex].position;

        SetState(AIState.Wandering);
        //// 에이전트가 현재 선택된 목적지로 이동하도록 설정합니다.
        //agent.destination = points[destPoint].position;

        //// 배열의 다음 목적지로 선택하고, 필요하다면 처음부터 다시 시작합니다.
        //destPoint = (destPoint + 1) % points.Length;
    }
    void Die()
    {
        //for (int x = 0; x < dropOnDeath.Length; x++)
        //{
        //    Instantiate(dropOnDeath[x].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        //}

        Destroy(gameObject);
    }
    //public void TakePhysicalDamage(int damageAmount)
    //{
    //    health -= damageAmount;
    //    if (health <= 0)
    //        Die();

    //    StartCoroutine(DamageFlash());
    //}
    void DropItem()
    {
        if (hints2 != null)
        {
            Instantiate(hints2, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Item prefab is not assigned in the inspector.");
        }
    }
}
```

---

### 5.목표 지점

---

목표 지점은 따로 구현하지 않았지만, 현관의 키패드가 최종 퍼즐과 목표 지점의 역할을 겸합니다.

---

### 6.게임 진행 상태 및 저장

---

게임의 저장은 json을 이용해 데이터를 직렬화하여 저장하였고 이 작업은 DataManager.cs에서 이루어집니다.

```cs
<C#>
public class DataManager : MonoBehaviour
{
    private static DataManager _instance;

    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("DataManager").AddComponent<DataManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // 데이터 .json 저장
    public void SavePlayerDataToJson()
    {
        Debug.Log("저장완료");
        string jsonData = JsonUtility.ToJson(CharacterManager.Instance.Player.playerData);
        string path = Path.Combine(Application.dataPath, "playerData.json");
        File.WriteAllText(path, jsonData);
    }

    // json 데이터 불러오기
    public void LoadPlayerDataFromJson()
    {
        string path = Path.Combine(Application.dataPath, "playerData.json");
        string jsonData = File.ReadAllText(path);
        CharacterManager.Instance.Player.playerData = JsonUtility.FromJson<PlayerData>(jsonData); // 역직렬화
    }

    public void SaveData(QuestState state)
    {
        CharacterManager.Instance.Player.playerData.quest.QuestData(state);
        CharacterManager.Instance.Player.playerData.equipItems = CharacterManager.Instance.Player.EquipItemLsit; //장착 아이템 저장
        CharacterManager.Instance.Player.playerData.itemList = CharacterManager.Instance.Player.ItemLsit; //인벤 아이템 저장

        SavePlayerDataToJson();
    }

    public void SavePosData(Vector3 pos)
    {
        CharacterManager.Instance.Player.playerData.position = pos;
        CharacterManager.Instance.Player.playerData.equipItems = CharacterManager.Instance.Player.EquipItemLsit; //장착 아이템 저장
        CharacterManager.Instance.Player.playerData.itemList = CharacterManager.Instance.Player.ItemLsit; //인벤 아이템 저장

        Debug.Log("위치 저장");
        SavePlayerDataToJson();
    }
}
```

---

### 7.사운드 및 음악

---



---

## 그 외 저희가 구현한 사항들

---



---

이상입니다.

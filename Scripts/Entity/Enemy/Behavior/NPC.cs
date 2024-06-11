using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
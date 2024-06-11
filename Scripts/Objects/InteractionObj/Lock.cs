using UnityEngine;

public class Lock : MonoBehaviour, IInteractable // 원강
{
    private static readonly int isDestruction = Animator.StringToHash("isDestruction");

    private Animator animator;

    public AudioClip destroyLock;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public string GetInteractPrompt()
    {
        if (GameManager.Instance.canDestroy)
        {
            string str = "E";
            return str;
        }
        else
        {
            string str = "자물쇠를 풀어야 할 것 같다. ";
            return str;
        }
    }
    
    public void ObjectInteract()
    {
        if (GameManager.Instance.canDestroy)
        {
            if (!TutorialManager.Instance.tutoriallockClear && !TutorialManager.Instance.tutoriallock)
            {
                TutorialManager.Instance.ClearLock();
            }
            animator.SetTrigger(isDestruction);
            Destroy(gameObject,3f);
            SoundManager.instance.SFXPlay("DestroyLock", destroyLock);
        }
        else
        {
            if (TutorialManager.Instance.tutoriallock)
            {
                StartCoroutine(TutorialManager.Instance.OnTutorialMassage("자물쇠를 열어야겠는데...", "문을 열 방법을 찾자", "lock"));
                TutorialManager.Instance.tutoriallock = false;
            }
        }
    }
}
using UnityEngine;

public class CharacterAnimationController : AnimationController
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");

    private readonly float magnituteThreshold = 0.5f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        controller.OnMoveEvent += Move;
        controller.OnRunEvent += Run;
    }

    private void Move(Vector2 obj)
    {
        animator.SetBool(IsRunning, false);
        animator.SetBool(IsWalking, obj.magnitude > magnituteThreshold);
    }

    private void Run(float value)
    {
        if(animator.GetBool(IsRunning) == false)
        {
            animator.SetBool(IsRunning, true);
            animator.SetBool(IsWalking, false);
        }
        else
        {
            animator.SetBool(IsRunning, false);
            animator.SetBool(IsWalking, true);
        }
    }
}

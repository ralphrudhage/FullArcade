using UnityEngine;

public class Jujimufu : Player
{
    [SerializeField] private Animator barbellAnimator;
    [SerializeField] private AudioClip backflipSound;
    [SerializeField] private AudioClip maskSound;
    [SerializeField] private AudioClip deadliftSound;
    [SerializeField] AnimatorOverrideController horseController;

    private RuntimeAnimatorController defaultController;
    private bool usingHorseController;
    private bool isDeadlift;

    private void Start()
    {
        defaultController = animator.runtimeAnimatorController;
    }

    protected override void Update()
    {
        base.Update();
        HandleInputActions();
    }

    private void HandleInputActions()
    {
        if (InputActions.Up.triggered)
        {
            SoundManager.Instance.PlaySound(deadliftSound);
            isDeadlift = true;
            barbellAnimator.SetTrigger("lift");
            animator.SetTrigger("deadlift");
            spriteRenderer.sortingOrder = 10;
        }

        if (InputActions.Down.triggered)
        {
            if (isDeadlift)
            {
                isDeadlift = false;
                barbellAnimator.SetTrigger("down");
                animator.SetTrigger("down");
                spriteRenderer.sortingOrder = 1;
            }
            else
            {
                FaceFront();
            }
        }

        if (InputActions.Button1.triggered)
        {
            SoundManager.Instance.PlaySound(backflipSound);
            animator.SetTrigger("backflip");
        }

        if (InputActions.Button2.triggered)
        {
            ToggleMask();
        }

        if (InputActions.Button3.triggered)
        {
            Debug.Log("Button3");
        }

        if (InputActions.Button4.triggered)
        {
            Debug.Log("Button4");
        }

        if (InputActions.Button5.triggered)
        {
            Debug.Log("Button5");
        }
    }

    private void FaceFront()
    {
        animator.SetBool("walking", false);
        animator.SetTrigger("idle_front");
    }
    
    private void ToggleMask()
    {
        SoundManager.Instance.PlaySound(maskSound);
        var trigger = usingHorseController ? "maskoff" : "maskon";
        animator.SetTrigger(trigger);
    }
    
    public void MaskOn()
    {
        animator.runtimeAnimatorController = horseController;
        usingHorseController = true;
    }

    public void MaskOff()
    {
        animator.runtimeAnimatorController = defaultController;
        usingHorseController = false;
    }
}
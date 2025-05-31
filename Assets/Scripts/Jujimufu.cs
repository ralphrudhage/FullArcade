using UnityEngine;

public class Jujimufu : Player
{
    [SerializeField] AnimatorOverrideController horseController;
    
    private RuntimeAnimatorController defaultController;
    private bool usingHorseController;

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
            Debug.Log("Up");
        }

        if (InputActions.Down.triggered)
        {
            FaceFront();
        }

        if (InputActions.Button1.triggered)
        {
            Debug.Log("Button1");
        }

        if (InputActions.Button2.triggered)
        {
            usingHorseController = !usingHorseController;
            animator.runtimeAnimatorController = usingHorseController ? horseController : defaultController;
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
}
using UnityEngine;

public class Jujimufu : Player
{
    protected override void Update()
    {
        base.Update();
        HandleMovement();
        HandleInputActions();
    }

    private void HandleMovement()
    {
        float moveInput = GetMoveInput();
        isMoving = moveInput != 0;

        if (isMoving)
        {
            if (!animator.GetBool("walking"))
            {
                animator.SetTrigger("walk");
                animator.SetBool("walking", true);
            }

            transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0f, 0f);
            FaceRight(moveInput > 0);
        }
        else
        {
            animator.SetBool("walking", false);
        }
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
            Debug.Log("Button2");
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
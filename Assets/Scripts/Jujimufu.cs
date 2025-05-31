using UnityEngine;

public class Jujimufu : Player
{
    protected override void Update()
    {
        base.Update();

        HandleMovement();
        HandleStepSound();
    }

    private void HandleMovement()
    {
        float moveInput = InputActions.Right.IsPressed() ? 1 : InputActions.Left.IsPressed() ? -1 : 0;
        isMoving = moveInput != 0;
        
        if (moveInput != 0)
        {
            transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0f, 0f);
        }
    }
    
    private void HandleStepSound()
    {
        if (!isMoving) return;

        stepTimer += Time.deltaTime;
        if (stepTimer >= stepInterval)
        {
            stepTimer = 0f;
            SoundManager.Instance.PlaySound(walkSound);
        }
    }
}
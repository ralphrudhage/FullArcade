using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Grizzly : Player
{
    [SerializeField] private Image powerImage;
    [SerializeField] private AudioClip facePalm;
    [SerializeField] private AudioClip initShrug;
    [SerializeField] private AudioClip shrugSound;
    [SerializeField] Animator zercherAnimator;
    [SerializeField] Animator sledpullAnimator;
    [SerializeField] GameObject sledpull;
    
    private bool facingRight;
    private bool isWalking;
    private bool isPulling;
    private bool playerEnabled = true;
    
    protected override void Update()
    {
        base.Update();
        HandleInputActions();
        if (!playerEnabled) return;

        HandleMovement();
        HandleStepSound();
    }

    private void HandleMovement()
    {
        if (!playerEnabled || isPulling) return;

        float moveInput = InputActions.Right.IsPressed() ? 1 :
            InputActions.Left.IsPressed() ? -1 : 0;

        bool wasMoving = isMoving;
        isMoving = moveInput != 0;

        if (isMoving)
        {
            transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0f, 0f);

            if (!isWalking)
            {
                animator.SetTrigger("walk");
                isWalking = true;
            }

            if (moveInput > 0)
                FaceRight();
            else
                FaceLeft();
        }
        else
        {
            if (wasMoving)
            {
                SideIdle();
            }
        }
    }

    private void HandleInputActions()
    {
        if (InputActions.Up.triggered && !isPulling)
        {
            SledPullInit();
        }

        if (InputActions.Down.triggered)
        {
            ResetSledPull();
            FaceFront();
        }

        if (InputActions.Button1.triggered)
        {
            InitShrugging();
        }

        if (InputActions.Button2.triggered)
        {
            ShrugOrFacePalm();
        }
    }

    private void HandleStepSound()
    {
        if (!isWalking || !isMoving || walkSound == null) return;

        stepTimer += Time.deltaTime;
        if (stepTimer >= stepInterval)
        {
            stepTimer = 0f;
            SoundManager.Instance.PlaySound(walkSound);
        }
    }

    private void SideIdle()
    {
        if (!isWalking) return;

        animator.SetTrigger("idleside");
        isWalking = false;
        stepTimer = 0f;
    }

    private void FaceLeft()
    {
        if (!playerEnabled) return;

        if (facingRight)
        {
            FlipDirection();
            facingRight = false;
        }
    }

    private void FaceFront()
    {
        if (!playerEnabled)
        {
            playerEnabled = true;
            spriteRenderer.enabled = true;
            zercherAnimator.SetTrigger("empty");
            SoundManager.Instance.PlaySound(initShrug);
        }

        isWalking = false;
        animator.SetTrigger("idlefront");
    }

    private void FaceRight()
    {
        if (!playerEnabled) return;

        if (!facingRight)
        {
            FlipDirection();
            facingRight = true;
        }
    }
    
    private void InitShrugging()
    {
        SoundManager.Instance.PlaySound(initShrug);
        spriteRenderer.enabled = false;
        playerEnabled = false;
        zercherAnimator.SetTrigger("init");
    }

    private void ShrugOrFacePalm()
    {
        if (playerEnabled)
        {
            FacePalm();
        }
        else
        {
            powerImage.fillAmount -= 0.05f;
            SoundManager.Instance.PlaySound(shrugSound);
            zercherAnimator.SetTrigger("shrug");
        }
    }

    private void FacePalm()
    {
        SoundManager.Instance.PlaySound(facePalm);
        animator.SetTrigger("facepalm");
    }

    private void SledPullInit()
    {
        SoundManager.Instance.PlaySound(initShrug);
        spriteRenderer.enabled = false;
        playerEnabled = false;
        sledpullAnimator.SetTrigger("init");
    }

    public void SledPull()
    {
        if (isPulling) return;
        powerImage.fillAmount -= 0.05f;
        SoundManager.Instance.PlaySound(shrugSound);
        isPulling = true;
        sledpullAnimator.SetTrigger("pull");
    }

    public void MoveSled()
    {
        float offset = -0.2f;
        Vector3 targetPosition = transform.position + new Vector3(offset, 0f, 0f);
        Vector3 sledTargetPosition = sledpull.transform.position + new Vector3(offset, 0f, 0f);

        StartCoroutine(SmoothMove(transform, targetPosition, 0.25f));
        StartCoroutine(SmoothMove(sledpull.transform, sledTargetPosition, 0.25f));
        StartCoroutine(DelaySledPull());
    }

    private IEnumerator SmoothMove(Transform obj, Vector3 target, float duration)
    {
        Vector3 start = obj.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            obj.position = Vector3.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        obj.position = target; // snap to final position in case of rounding errors
    }

    private IEnumerator DelaySledPull()
    {
        yield return new WaitForSeconds(0.5f);
        isPulling = false;
    }

    private void ResetSledPull()
    {
        isPulling = false;
        sledpullAnimator.SetTrigger("empty");
    }
}
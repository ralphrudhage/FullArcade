using System;
using UnityEngine;

public class Jujimufu : Player
{
    [SerializeField] private Animator barbellAnimator;
    [SerializeField] private AudioClip backflipSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip maskSound;
    [SerializeField] private AudioClip deadliftSound;
    [SerializeField] AnimatorOverrideController horseController;

    private ArcadeManager arcadeManager;
    private PowerRack powerRack;
    private CameraFollow cameraFollow;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb;
    private RuntimeAnimatorController defaultController;
    private bool usingHorseController;
    private bool isDeadlift;
    private float jumpForce = 6f;
    private bool isBenchpress;
    private int currentScore;

    private void Start()
    {
        arcadeManager = FindAnyObjectByType<ArcadeManager>();
        powerRack = FindAnyObjectByType<PowerRack>();
        cameraFollow = FindAnyObjectByType<CameraFollow>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        defaultController = animator.runtimeAnimatorController;
        moveSpeed = 4f;
        StartCoroutine(DelayedAction(8f, () => scoreText.text = "SCORE: 0"));
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
            if (isBenchpress)
            {
                powerRack.Press(false);
            }
            else
            {
                isDeadlift = true;
                barbellAnimator.SetTrigger("lift");
                animator.SetTrigger("deadlift");
                spriteRenderer.sortingOrder = 10;
                arcadeManager.SpawnFloatingText("+1", GameUtils.lightYellow, transform.position + new Vector3(0f, 1f, 0f));
                currentScore += 1;
                scoreText.text = "SCORE: " + currentScore;
            }
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
            else if (isBenchpress)
            {
                powerRack.Press(true);
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
            SoundManager.Instance.PlaySound(jumpSound);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // animator.SetTrigger("jump");
            isGrounded = false;
        }

        if (InputActions.Button4.triggered)
        {
            cameraFollow.MoveDown();
            boxCollider2D.isTrigger = true;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("powerrack"))
        {
            boxCollider2D.isTrigger = false;
            isBenchpress = true;
            spriteRenderer.enabled = false;
        }
    }
}
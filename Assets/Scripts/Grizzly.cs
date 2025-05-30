using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Grizzly : MonoBehaviour
{
    [SerializeField] private Image powerImage;
    [SerializeField] private AudioClip facePalm;
    [SerializeField] private AudioClip initShrug;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip shrugSound;
    [SerializeField] Animator zercherAnimator;
    [SerializeField] Animator sledpullAnimator;
    [SerializeField] GameObject sledpull;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool facingRight;
    private bool isWalking;
    private bool isPulling;
    private bool isMoving;

    private bool playerEnabled = true;

    [SerializeField] private float moveSpeed = 2f;

    private float stepTimer;
    private const float stepInterval = 0.2f;

    private PlayerControls controls;
    private PlayerControls.ArcadeActions InputActions { get; set; }

    public bool IsButton1Held => InputActions.Button1.IsPressed();
    public bool IsButton2Held => InputActions.Button2.IsPressed();

    public bool IsUp => InputActions.Up.IsPressed();
    public bool IsDown => InputActions.Down.IsPressed();
    public bool IsLeft => InputActions.Left.IsPressed();
    public bool IsRight => InputActions.Right.IsPressed();


    private void Awake()
    {
        controls = new PlayerControls();
        InputActions = controls.Arcade;
        controls.Enable();
    }

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if (!playerEnabled) return;

        HandleMovement();
        HandleInputActions();
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
            if (wasMoving) // just stopped moving
            {
                SideIdle(); // resets `isWalking` and `stepTimer`
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
            Shrug();
        }
    }


    private void Walk()
    {
        if (!playerEnabled) return;

        if (!isWalking)
        {
            animator.SetTrigger("walk");
            isWalking = true;
            stepTimer = 0f; // reset timer when starting
        }

        // Play step sound on interval
        stepTimer += Time.deltaTime;
        if (stepTimer >= stepInterval)
        {
            stepTimer = 0f;
            if (walkSound != null)
            {
                audioSource.PlayOneShot(walkSound);
            }
        }
    }

    private void HandleStepSound()
    {
        if (!isWalking || !isMoving || walkSound == null) return;

        stepTimer += Time.deltaTime;
        if (stepTimer >= stepInterval)
        {
            stepTimer = 0f;
            audioSource.PlayOneShot(walkSound);
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
            audioSource.PlayOneShot(initShrug);
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

    private void FlipDirection()
    {
        Vector3 flipped = transform.localScale;
        flipped.x *= -1;
        transform.localScale = flipped;
    }

    private void InitShrugging()
    {
        audioSource.PlayOneShot(initShrug);
        spriteRenderer.enabled = false;
        playerEnabled = false;
        zercherAnimator.SetTrigger("init");
    }

    private void Shrug()
    {
        if (playerEnabled)
        {
            FacePalm();
        }
        else
        {
            powerImage.fillAmount -= 0.05f;
            audioSource.PlayOneShot(shrugSound);
            zercherAnimator.SetTrigger("shrug");
        }
    }

    private void FacePalm()
    {
        audioSource.PlayOneShot(facePalm);
        animator.SetTrigger("facepalm");
    }

    private void SledPullInit()
    {
        audioSource.PlayOneShot(initShrug);
        spriteRenderer.enabled = false;
        playerEnabled = false;
        sledpullAnimator.SetTrigger("init");
    }

    public void SledPull()
    {
        if (isPulling) return;
        powerImage.fillAmount -= 0.05f;
        audioSource.PlayOneShot(shrugSound);
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
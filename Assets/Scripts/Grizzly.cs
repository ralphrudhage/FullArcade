using UnityEngine;

public class Grizzly : MonoBehaviour
{
    [SerializeField] private AudioClip facePalm;
    [SerializeField] private AudioClip initShrug;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip shrugSound;
    [SerializeField] Animator zercherAnimator;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool facingRight = false;
    private bool isWalking = false;

    private bool playerEnabled = true;

    [SerializeField] private float moveSpeed = 2f;

    private float stepTimer = 0f;
    private float stepInterval = 0.4f; // Adjust based on speed / step rhythm

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!playerEnabled) return;

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0f, 0f);
            Walk();

            if (moveInput > 0)
                FaceRight();
            else
                FaceLeft();
        }
        else
        {
            SideIdle();
        }
    }

    public void Walk()
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


    public void SideIdle()
    {
        if (!playerEnabled) return;

        if (isWalking)
        {
            animator.SetTrigger("idleside");
            isWalking = false;
            stepTimer = 0f;
        }
    }

    public void FaceLeft()
    {
        if (!playerEnabled) return;

        if (facingRight)
        {
            FlipDirection();
            facingRight = false;
        }
    }

    public void FaceFront()
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

    public void FaceRight()
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

    public void InitShrugging()
    {
        audioSource.PlayOneShot(initShrug);
        spriteRenderer.enabled = false;
        playerEnabled = false;
        zercherAnimator.SetTrigger("init");
    }

    public void Shrug()
    {
        if (playerEnabled)
        {
            FacePalm();
        }
        else
        {
            audioSource.PlayOneShot(shrugSound);
            zercherAnimator.SetTrigger("shrug");
        }
    }

    public void FacePalm()
    {
        audioSource.PlayOneShot(facePalm);
        animator.SetTrigger("facepalm");
    }
}
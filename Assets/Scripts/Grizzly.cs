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
    private bool facingRight = false;
    private bool isWalking = false;
    private bool isPulling = false;

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
            powerImage.fillAmount -= 0.05f;
            audioSource.PlayOneShot(shrugSound);
            zercherAnimator.SetTrigger("shrug");
        }
    }

    public void FacePalm()
    {
        audioSource.PlayOneShot(facePalm);
        animator.SetTrigger("facepalm");
    }

    public void SledPullInit()
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

    public void ResetSledPull()
    {
        isPulling = false;
        sledpullAnimator.SetTrigger("empty");
    }
}
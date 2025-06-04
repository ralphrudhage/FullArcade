using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jujimufu : Player
{
    [SerializeField] private Image xpBar;
    [SerializeField] private List<Text> levelText;
    [SerializeField] private List<Text> deadsText;
    [SerializeField] private List<Text> benchText;
    [SerializeField] private List<Text> flipsText;
    [SerializeField] private GameObject xpObj;
    [SerializeField] private Animator barbellAnimator;
    [SerializeField] private AudioClip backflipSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip maskSound;
    [SerializeField] private AudioClip portalSound;
    [SerializeField] private AudioClip benchSound;
    [SerializeField] private AudioClip levelSound;
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
    private int currentLevel = 1;
    private int currentDeads;
    private int currentFlips;
    private int currentBench;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetText(levelText, "");
        SetText(deadsText, "");
        SetText(benchText, "");
        SetText(flipsText, "");
        xpObj.SetActive(false);
        xpBar.fillAmount = 0f;
    }

    private void Start()
    {
        arcadeManager = FindAnyObjectByType<ArcadeManager>();
        powerRack = FindAnyObjectByType<PowerRack>();
        cameraFollow = FindAnyObjectByType<CameraFollow>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        defaultController = animator.runtimeAnimatorController;
        moveSpeed = 4f;
        StartCoroutine(DelayedAction(8f, () =>
        {
            xpObj.SetActive(true);
            RefreshTexts();
        }));
    }

    private void RefreshTexts()
    {
        SetText(levelText, "LEVEL: " + currentLevel);
        if (currentDeads >= 10)
        {
            deadsText[1].color = GameUtils.nightBlue;
            SetText(deadsText, "DEADS: 10 / 10");
        }
        else
        {
            SetText(deadsText, "DEADS: " + currentDeads + " / 10");
        }

        if (currentBench >= 10)
        {
            benchText[1].color = GameUtils.nightBlue;
            SetText(benchText, "BENCH: 10 / 10");
        }
        else
        {
            SetText(benchText, "BENCH: " + currentBench + " / 10");
        }

        if (currentFlips >= 10)
        {
            flipsText[1].color = GameUtils.nightBlue;
            SetText(flipsText, "FLIPS: 10 / 10");
        }
        else
        {
            SetText(flipsText, "FLIPS: " + currentFlips + " / 10");
        }
    }

    private void IncreaseXp()
    {
        xpBar.fillAmount += 0.25f;
        if (xpBar.fillAmount >= 1f)
        {
            StartCoroutine(DelayedAction(0.5f, LevelUp));
        }
    }

    private void LevelUp()
    {
        SoundManager.Instance.PlaySound(levelSound);
        arcadeManager.SpawnFloatingText("LEVEL UP!", GameUtils.lightBlue, transform.position + new Vector3(0f, 1f, 0f));
        currentLevel++;
        RefreshTexts();
        xpBar.fillAmount = 0f;
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
            if (isBenchpress)
            {
                SoundManager.Instance.PlaySound(benchSound);
                powerRack.Press(false);
                currentBench++;
                arcadeManager.SpawnFloatingText("+1", GameUtils.lightYellow, powerRack.gameObject.transform.position + new Vector3(0f, 1f, 0f));
                if (currentBench < 11) IncreaseXp();
                RefreshTexts();
            }
            else
            {
                SoundManager.Instance.PlaySound(deadliftSound);
                isDeadlift = true;
                barbellAnimator.SetTrigger("lift");
                animator.SetTrigger("deadlift");
                spriteRenderer.sortingOrder = 10;
                arcadeManager.SpawnFloatingText("+1", GameUtils.lightYellow, transform.position + new Vector3(0f, 1f, 0f));
                currentDeads += 1;
                if (currentDeads < 11) IncreaseXp();
                RefreshTexts();
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
            currentFlips++;
            arcadeManager.SpawnFloatingText("+1", GameUtils.lightYellow, transform.position + new Vector3(0f, 1f, 0f));
            if (currentFlips < 11) IncreaseXp();
            RefreshTexts();
        }

        if (InputActions.Button2.triggered)
        {
            ToggleMask();
        }

        if (InputActions.Button3.triggered)
        {
            SoundManager.Instance.PlaySound(jumpSound);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        if (InputActions.Button4.triggered)
        {
            if (isBenchpress)
            {
                powerRack.Reset();
                isBenchpress = false;
                spriteRenderer.sortingOrder = 3;
                spriteRenderer.enabled = true;
            }
            else
            {
                SoundManager.Instance.PlaySound(portalSound);
                cameraFollow.MoveDown();
                boxCollider2D.isTrigger = true;
            }
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

    private void SetText(List<Text> uiText, string value)
    {
        foreach (var text in uiText)
        {
            text.text = value;
        }
    }

    private void SetColor(Text uiText, Color value)
    {
        uiText.color = value;
    }
}
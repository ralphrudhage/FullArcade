using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] protected AudioClip walkSound;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    private ArcadeManager arcadeManager;
    private PlayerControls controls;
    protected PlayerControls.ArcadeActions InputActions { get; set; }

    protected float stepTimer;
    protected const float moveSpeed = 2f;
    protected const float stepInterval = 0.2f;
    protected bool isMoving;
    protected bool facingRight;

    private void Awake()
    {
        controls = new PlayerControls();
        InputActions = controls.Arcade;
        controls.Enable();
    }

    protected virtual void OnEnable()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        arcadeManager = FindAnyObjectByType<ArcadeManager>();
    }

    protected virtual void Update()
    {
        VisualControls();
        HandleStepSound();
    }

    protected int GetMoveInput()
    {
        if (InputActions.Right.IsPressed()) return 1;
        if (InputActions.Left.IsPressed()) return -1;
        return 0;
    }

    protected void FaceRight(bool shouldFaceRight)
    {
        if (facingRight != shouldFaceRight)
        {
            facingRight = shouldFaceRight;
            FlipDirection();
        }
    }

    private void VisualControls()
    {
        if (InputActions.Up.IsPressed())
            arcadeManager?.SetJoystickDirection("Up");
        else if (InputActions.Down.IsPressed())
            arcadeManager?.SetJoystickDirection("Down");
        else if (InputActions.Left.IsPressed())
            arcadeManager?.SetJoystickDirection("Left");
        else if (InputActions.Right.IsPressed())
            arcadeManager?.SetJoystickDirection("Right");
        else
            arcadeManager?.SetJoystickDirection("");

        arcadeManager?.SetButtonState(1, InputActions.Button1.IsPressed());
        arcadeManager?.SetButtonState(2, InputActions.Button2.IsPressed());
    }

    protected void FlipDirection()
    {
        var flipped = transform.localScale;
        flipped.x *= -1;
        transform.localScale = flipped;
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
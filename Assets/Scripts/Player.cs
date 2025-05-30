using UnityEngine;

public class Player : MonoBehaviour
{
    private ArcadeManager arcadeManager;
    private PlayerControls controls;
    protected PlayerControls.ArcadeActions InputActions { get; set; }

    private void Awake()
    {
        controls = new PlayerControls();
        InputActions = controls.Arcade;
        controls.Enable();
    }

    protected virtual void OnEnable()
    {
        arcadeManager = FindAnyObjectByType<ArcadeManager>();
    }

    protected virtual void Update()
    {
        VisualControls();
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
}
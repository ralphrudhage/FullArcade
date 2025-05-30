using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ArcadeManager : BaseManager
{
    [SerializeField] private List<Sprite> joystickSprites;
    [SerializeField] private List<Sprite> buttonSprites;
    [SerializeField] private Image joystick;
    [SerializeField] private Image button1;
    [SerializeField] private Image button2;

    private bool isUpHeld, isDownHeld, isRightHeld, isLeftHeld;
    private bool isButton1Pressed, isButton2Pressed;

    private PlayerControls controls;
    private Grizzly grizzly;
    private bool sledPull;

    private void Awake()
    {
        controls = new PlayerControls();

        // Directional input
        BindDirectionalInput(controls.Arcade.Up, value => isUpHeld = value);
        BindDirectionalInput(controls.Arcade.Down, value => isDownHeld = value);
        BindDirectionalInput(controls.Arcade.Right, value => isRightHeld = value);
        BindDirectionalInput(controls.Arcade.Left, value => isLeftHeld = value);

        // Button visuals
        BindButtonInput(controls.Arcade.Button1, value => isButton1Pressed = value);
        BindButtonInput(controls.Arcade.Button2, value => isButton2Pressed = value);

        // Button actions (trigger once on press)
        controls.Arcade.Button1.started += ctx => OnButton1Press();
        controls.Arcade.Button2.started += ctx => OnButton2Press();
    }


    private void BindButtonInput(InputAction action, System.Action<bool> setter)
    {
        action.performed += ctx => setter(true);
        action.canceled += ctx => setter(false);
    }

    private void OnEnable()
    {
        grizzly = FindAnyObjectByType<Grizzly>();
    }

    private void Start()
    {
        controls.Arcade.Enable();
    }

    private void BindDirectionalInput(InputAction action, System.Action<bool> setter)
    {
        action.performed += ctx => { setter(true); };
        action.canceled += ctx => setter(false);
    }

    private void Update()
    {
        UpdateJoystickVisual();
        UpdateButtonVisuals();
        HandleDirectionalInput();
    }

    private void UpdateJoystickVisual()
    {
        joystick.sprite = joystickSprites[
            isUpHeld ? 1 : isDownHeld ? 3 : isRightHeld ? 2 : isLeftHeld ? 4 : 0
        ];
    }

    private void UpdateButtonVisuals()
    {
        button1.sprite = buttonSprites[isButton1Pressed ? 1 : 0];
        button2.sprite = buttonSprites[isButton2Pressed ? 1 : 0];
    }

    private void HandleDirectionalInput()
    {
        bool moved = false;
        
        if (isUpHeld)
        {
            HandleUp();
            moved = true;
        }
        else if (isDownHeld)
        {
            HandleDown();
            moved = true;
        }
        else if (isRightHeld)
        {
            HandleRight();
            moved = true;
        }
        else if (isLeftHeld)
        {
            HandleLeft();
            moved = true;
        }

        if (!moved)
        {
            grizzly.SideIdle();
        }
    }
    
    private void HandleUp() {
        if (!sledPull)
        {
            sledPull = true;
            grizzly.SledPullInit();
        }
    }

    private void HandleDown()
    {
        if (sledPull)
        {
            grizzly.ResetSledPull();
            sledPull = false;
            
        }
        grizzly.FaceFront();
    }

    private void HandleLeft()
    {
        if (sledPull)
        {
            grizzly.SledPull();
        }
        else
        {
            grizzly.FaceLeft();
            grizzly.Walk();
        }

    }

    private void HandleRight()
    {
        grizzly.FaceRight();
        grizzly.Walk();
    }
    
    private void OnButton1Press()
    {
        isButton1Pressed = true;
        grizzly.InitShrugging();
    }

    private void OnButton2Press()
    {
        isButton2Pressed = true;
        grizzly.Shrug();
    }
}
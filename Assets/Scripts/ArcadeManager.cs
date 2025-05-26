using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ArcadeManager : BaseManager
{
    [SerializeField] private List<Sprite> joystickSprites;
    [SerializeField] private List<Sprite> greenShoeButtonSprites;
    [SerializeField] private List<Sprite> blueShoeButtonSprites;
    [SerializeField] private Image joystick;
    [SerializeField] private Image greenShoeButton;
    [SerializeField] private Image blueShoeButton;
    [SerializeField] private Image redShoeButton;
    [SerializeField] private Image whiteShoeButton;
    [SerializeField] private Animator showArcadeAnimator;
    [SerializeField] private Animator transitionAnimator;

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
        BindButtonInput(controls.Arcade.Green, value => isButton1Pressed = value);
        BindButtonInput(controls.Arcade.Blue, value => isButton2Pressed = value);

        // Button actions (trigger once on press)
        controls.Arcade.Green.started += ctx => OnGreenButtonPress();
        controls.Arcade.Blue.started += ctx => OnBlueButtonPress();
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
            isUpHeld && isRightHeld ? 2 :
            isUpHeld && isLeftHeld ? 8 :
            isDownHeld && isRightHeld ? 4 :
            isDownHeld && isLeftHeld ? 6 :
            isUpHeld ? 1 :
            isRightHeld ? 3 :
            isDownHeld ? 5 :
            isLeftHeld ? 7 : 0
        ];
    }

    private void UpdateButtonVisuals()
    {
        greenShoeButton.sprite = greenShoeButtonSprites[isButton1Pressed ? 1 : 0];
        blueShoeButton.sprite = blueShoeButtonSprites[isButton2Pressed ? 1 : 0];
    }

    private void HandleDirectionalInput()
    {
        bool moved = false;

        if (isUpHeld && isRightHeld)
        {
            HandleUpRight();
            moved = true;
        }
        else if (isUpHeld && isLeftHeld)
        {
            HandleUpLeft();
            moved = true;
        }
        else if (isDownHeld && isRightHeld)
        {
            HandleDownRight();
            moved = true;
        }
        else if (isDownHeld && isLeftHeld)
        {
            HandleDownLeft();
            moved = true;
        }
        else if (isUpHeld)
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

    private void HandleButtonInput()
    {
        if (isButton1Pressed) OnGreenButtonPress();
        if (isButton2Pressed) OnBlueButtonPress();
    }

    // Direction actions
    private void HandleUpRight() => Debug.Log("Moving Up-Right");
    private void HandleUpLeft() => Debug.Log("Moving Up-Left");
    private void HandleDownRight() => Debug.Log("Moving Down-Right");
    private void HandleDownLeft() => Debug.Log("Moving Down-Left");
    
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

    // Button actions
    private void OnGreenButtonPress()
    {
        isButton1Pressed = true;
        grizzly.InitShrugging();
    }

    private void OnBlueButtonPress()
    {
        isButton2Pressed = true;
        grizzly.Shrug();
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcadeManager : BaseManager
{
    [SerializeField] private List<Sprite> joystickSprites;
    [SerializeField] private List<Sprite> buttonSprites;
    [SerializeField] private Image joystick;
    [SerializeField] private Image button1;
    [SerializeField] private Image button2;
    
    private Grizzly grizzly;
    
    private void OnEnable()
    {
        grizzly = FindAnyObjectByType<Grizzly>();
    }
    

    private void Update()
    {
        if (grizzly == null) return;

        UpdateJoystickVisual();
        UpdateButtonVisuals();
    }

    private void UpdateJoystickVisual()
    {
        joystick.sprite = joystickSprites[
            grizzly.IsUp ? 1 :
            grizzly.IsDown ? 3 :
            grizzly.IsRight ? 2 :
            grizzly.IsLeft ? 4 : 0
        ];
    }

    private void UpdateButtonVisuals()
    {
        button1.sprite = buttonSprites[grizzly.IsButton1Held ? 1 : 0];
        button2.sprite = buttonSprites[grizzly.IsButton2Held ? 1 : 0];
    }
    
}
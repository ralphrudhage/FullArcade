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
    
    public void SetJoystickDirection(string direction)
    {
        int index = direction switch
        {
            "Up" => 1,
            "Right" => 2,
            "Down" => 3,
            "Left" => 4,
            _ => 0
        };

        joystick.sprite = joystickSprites[index];
    }

    public void SetButtonState(int buttonIndex, bool pressed)
    {
        switch (buttonIndex)
        {
            case 1:
                button1.sprite = buttonSprites[pressed ? 1 : 0];
                break;
            case 2:
                button2.sprite = buttonSprites[pressed ? 1 : 0];
                break;
        }
    }
}
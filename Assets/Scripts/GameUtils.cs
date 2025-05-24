using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public static class GameUtils
{
    public static Color32 lightYellow = new(254, 231, 97, 255);
    public static Color32 lightBlue = new(0, 149, 233, 255);
    public static Color32 skyBlue = new(44, 232, 245, 255);
    public static Color32 darkBlue = new(18, 78, 137, 255);
    public static Color32 nightBlue = new(58, 68, 102, 255);
    public static Color32 darkish = new(38, 43, 68, 251);
    public static Color32 darker = new(24, 20, 37, 251);
    public static Color32 lightRed = new(247, 118, 34, 255);
    public static Color32 gray = new(139, 155, 180, 255);
    public static Color32 lightGreen = new(99, 199, 77, 255);
    public static Color32 green = new(62, 137, 72, 255);
    public static Color32 darkGreen = new(38, 92, 66, 255);
    public static Color32 lightCyan = new(192, 203, 220, 255);
    public static Color32 pressedWhite = new(255, 255, 255, 100);
    public static Color32 white = new(255, 255, 255, 255);
    public static Color32 blackish = new(24, 20, 37, 255);
    public static Color32 hoofish = new(234, 212, 170, 255);
    public static Color32 brownie = new(116, 63, 57, 255);




    

    
    
    public static string ToHex(Color32 color)
    {
        return $"#{color.r:X2}{color.g:X2}{color.b:X2}";
    }

    public static Color32 GetRandomColor()
    {
        var randomInt = Random.Range(0, 9);
        return randomInt switch
        {
            0 => lightYellow,
            1 => lightBlue,
            2 => lightGreen,
            3 => lightRed,
            4 => green,
            5 => darkGreen,
            6 => gray,
            7 => lightCyan,
            8 => Color.white,
            _ => lightYellow
        };
    }
    
    public static Color32 RandomTrailColor()
    {
        var randomInt = Random.Range(0, 7);
        return randomInt switch
        {
            0 => lightYellow,
            1 => lightBlue,
            2 => lightRed,
            3 => white,
            4 => lightGreen,
            5 => skyBlue,
            6 => hoofish,
            _ => lightBlue
        };
    }

    public static Color32 GetRandomProgressBarColor()
    {
        var randomInt = Random.Range(0, 4);
        return randomInt switch
        {
            0 => lightRed,
            1 => lightBlue,
            2 => green,
            3 => lightGreen,
            _ => lightRed
        };
    }

    public static string TimeAsString(float timer)
    {
        var minutes = Mathf.FloorToInt(timer / 60);
        var seconds = Mathf.FloorToInt(timer % 60);
        var millis = Mathf.FloorToInt(timer * 1000 % 1000);
        return $"{minutes:00}:{seconds:00}:{millis:000}";
    }

    private static string TimeAsStringInHours(float time)
    {
        var hours = Mathf.FloorToInt(time / 3600);
        var minutes = Mathf.FloorToInt((time % 3600) / 60);
        var seconds = Mathf.FloorToInt(time % 60);
        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }


    public static int GetPlatformSortingOrderByLane(int lane)
    {
        return lane switch
        {
            0 => 22,
            1 => 25,
            2 => 28,
            3 => 31,
            4 => 34,
            5 => 37,
            _ => 22
        };
    }

    public static int GetSortingOrderByLane(int lane)
    {
        return lane switch
        {
            0 => 4,
            1 => 7,
            2 => 10,
            3 => 13,
            4 => 16,
            5 => 19,
            _ => 19
        };
    }

    public static string GetLayerName(int index)
    {
        return index switch
        {
            0 => "groundLane1",
            1 => "groundLane2",
            2 => "groundLane3",
            3 => "groundLane4",
            4 => "groundLane5",
            5 => "groundLane6",
            _ => throw new NotSupportedException("invalid laneIndex")
        };
    }

    public static string GetTriggerLayerName(int index)
    {
        return index switch
        {
            0 => "triggerLane1",
            1 => "triggerLane2",
            2 => "triggerLane3",
            3 => "triggerLane4",
            4 => "triggerLane5",
            5 => "triggerLane6",
            _ => throw new NotSupportedException("invalid laneIndex")
        };
    }

    public static string GetObstacleLayerName(int index)
    {
        return index switch
        {
            0 => "obstacleLayer1",
            1 => "obstacleLayer2",
            2 => "obstacleLayer3",
            3 => "obstacleLayer4",
            4 => "obstacleLayer5",
            5 => "obstacleLayer6",
            _ => throw new NotSupportedException("invalid laneIndex")
        };
    }

    public static string GetHorseLayerName(int index)
    {
        return index switch
        {
            0 => "horseLayer1",
            1 => "horseLayer2",
            2 => "horseLayer3",
            3 => "horseLayer4",
            4 => "horseLayer5",
            5 => "horseLayer6",
            _ => throw new NotSupportedException("invalid laneIndex")
        };
    }

    // Method to set the layer for the GameObject and all its children recursively
    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        // Set the layer of the current object
        obj.layer = newLayer;

        // Iterate through each child and set its layer
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public static bool CalculateChance(float rate)
    {
        return Random.Range(0.0f, 1.0f) <= rate;
    }

    public static void FadeSprite(MonoBehaviour context, SpriteRenderer renderer, float startingAlpha,
        float fadeDuration)
    {
        if (renderer == null || context == null)
        {
            Debug.LogWarning("SpriteRenderer or context is null. Cannot perform fade.");
            return;
        }

        context.StartCoroutine(FadeSpriteCoroutine(renderer, startingAlpha, fadeDuration));
    }

    private static IEnumerator FadeSpriteCoroutine(SpriteRenderer sprite, float startingAlpha, float fadeDuration)
    {
        var elapsedTime = 0f;
        var currentColor = sprite.color;

        currentColor.a = startingAlpha;
        sprite.color = currentColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            var t = Mathf.Clamp01(elapsedTime / fadeDuration);
            currentColor.a = Mathf.Lerp(startingAlpha, 0, t);
            sprite.color = currentColor;

            yield return null;
        }

        currentColor.a = 0;
        sprite.color = currentColor;
    }



    public static Color32 HexToColor32(string hex)
    {
        // Remove the '#' if it's present
        if (hex.StartsWith("#"))
        {
            hex = hex.Substring(1);
        }

        // Parse the R, G, B values
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        // Return the color (RGBA defaults to 255 for opaque colors)
        return new Color32(r, g, b, 255);
    }


}
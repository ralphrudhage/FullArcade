using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SledPuller : MonoBehaviour
{
    [SerializeField] private AudioClip earthMove;
    [SerializeField] List<Sprite> sprites;

    private Image image;
    private Shake shake;
    private int counter;
    private float soundVolume = 1.0f;

    private void OnEnable()
    {
        shake = FindAnyObjectByType<Shake>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(Pull());
    }

    private IEnumerator Pull()
    {
        counter++;
        
        yield return new WaitForSeconds(0.4f);
        image.sprite = sprites[1];
        
        yield return new WaitForSeconds(0.1f);

        if (counter > 4) soundVolume -= 0.2f;
        SoundManager.Instance.PlaySound(earthMove, soundVolume);
        shake.TriggerShake();
        RectTransform rectTransform = image.rectTransform;
        rectTransform.anchoredPosition += new Vector2(-2f, 0f);

        yield return new WaitForSeconds(1f);
        image.sprite = sprites[0];

        if (counter < 8) StartCoroutine(Pull());
    }
}
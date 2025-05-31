using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SledPuller : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;

    private Image image;

    private void OnEnable()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(Pull());
    }

    private IEnumerator Pull()
    {
        yield return new WaitForSeconds(0.4f);
        image.sprite = sprites[1];
        
        yield return new WaitForSeconds(0.1f);
        RectTransform rectTransform = image.rectTransform;
        rectTransform.anchoredPosition += new Vector2(-2f, 0f);

        yield return new WaitForSeconds(1f);
        image.sprite = sprites[0];

        StartCoroutine(Pull());
    }
}
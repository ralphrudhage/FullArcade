using System.Collections.Generic;
using UnityEngine;

public class PowerRack : MonoBehaviour
{
    [SerializeField] private List<Sprite> powerSprites;

    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        spriteRenderer.sprite = powerSprites[1];
    }

    public void Press(bool status)
    {
        spriteRenderer.sprite = status ? powerSprites[2] : powerSprites[1];
    }
}

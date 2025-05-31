using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeWriter : MonoBehaviour
{
    [SerializeField] private List<AudioClip> typeSound;
    [SerializeField] private Text centeredRow;
    [SerializeField] private Text centeredRow2;

    private void OnEnable()
    {
        centeredRow.text = "";
        centeredRow2.text = "";
    }

    public void TextRow1(string message1, string message2, float duration, Color color)
    {
        StopAllCoroutines(); // optional: prevent overlapping messages
        StartCoroutine(TypeWriter(message1, message2, duration, color));
    }

    private IEnumerator TypeWriter(string message1, string message2, float duration, Color color)
    {
        float writeSpeed = 6f;

        // Row 1
        yield return StartCoroutine(TypeOutText(centeredRow, message1, writeSpeed, color));

        // Row 2
        if (!string.IsNullOrEmpty(message2))
        {
            yield return StartCoroutine(TypeOutText(centeredRow2, message2, writeSpeed, color));
        }

        if (duration > 0)
        {
            yield return new WaitForSeconds(duration);
            centeredRow.text = "";
            centeredRow2.text = "";
        }
    }

    private IEnumerator TypeOutText(Text textComponent, string message, float speed, Color color)
    {
        float t = 0f;
        int charIndex = 0;
        int lastCharIndex = -1;

        textComponent.text = "";
        textComponent.color = color;

        while (charIndex < message.Length)
        {
            t += Time.deltaTime * speed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, message.Length);

            // Only update text and play sound when a new character is revealed
            if (charIndex != lastCharIndex)
            {
                textComponent.text = message.Substring(0, charIndex);
                SoundManager.Instance.PlaySound(typeSound[Random.Range(0, typeSound.Count)]);
                lastCharIndex = charIndex;
            }

            yield return null; // wait one frame
        }

        textComponent.text = message;
    }
    
    public void Skip()
    {
        StopAllCoroutines();
        centeredRow.text = "";
        centeredRow2.text = "";
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboCounter : MonoBehaviour
{
    public TextMeshProUGUI comboText; // Reference to the TMP text component
    public AnimationCurve shakeIntensity; // Animation curve for shake effect
    public float shakeDuration = 0.5f; // Duration of the shake animation
    public float fadeDuration = 1f; // Duration of the fade-out effect

    private int comboCount = 0; // Current combo count
    private Vector3 originalPosition; // Original position of the text
    private Coroutine shakeCoroutine;
    private Coroutine fadeCoroutine;

    void Start()
    {
        originalPosition = comboText.rectTransform.localPosition; // Store the original position
        UpdateComboText(); // Initialize the combo text
        comboText.alpha = 0; // Start with the text hidden
    }

    public void IncrementCombo()
    {
        comboCount++;
        UpdateComboText();

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine); // Stop any ongoing shake
        }
        shakeCoroutine = StartCoroutine(ShakeText());

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine); // Stop fade-out if combo continues
        }

        comboText.alpha = 1; // Ensure text is visible
        ChangeTextColor();
    }

    public void ResetCombo()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine); // Stop any ongoing fade
        }
        fadeCoroutine = StartCoroutine(FadeOutAndReset());
    }

    private void UpdateComboText()
    {
        comboText.text = comboCount > 0 ? $"x{comboCount}" : "";
    }

    private IEnumerator ShakeText()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float intensity = shakeIntensity.Evaluate(elapsed / shakeDuration); // Evaluate intensity based on the animation curve
            Vector3 offset = new Vector3(
                Random.Range(-1f, 1f) * intensity,
                Random.Range(-1f, 1f) * intensity,
                0
            );

            comboText.rectTransform.localPosition = originalPosition + offset;

            yield return null;
        }

        comboText.rectTransform.localPosition = originalPosition; // Reset position
    }

    private void ChangeTextColor()
    {
        if (comboCount < 5)
        {
            comboText.color = Color.white;
        }
        else if (comboCount < 10)
        {
            comboText.color = Color.green;
        }
        else if (comboCount < 20)
        {
            comboText.color = Color.blue;
        }
        else
        {
            comboText.color = Color.red;
        }
    }

    private IEnumerator FadeOutAndReset()
    {
        float elapsed = 0f;
        float startAlpha = comboText.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            comboText.alpha = Mathf.Lerp(startAlpha, 0, elapsed / fadeDuration); // Smooth fade-out
            yield return null;
        }

        comboText.alpha = 0; // Ensure it's fully invisible
        comboCount = 0; // Reset the combo count
        UpdateComboText(); // Update the text to reflect the reset
        comboText.color = Color.white; // Reset color to white
    }
}

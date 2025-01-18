using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboCounter : MonoBehaviour
{
    public TextMeshProUGUI comboText; 
    public AnimationCurve shakeIntensity; 
    public float shakeDuration = 0.5f; 
    public float fadeDuration = 1f; 

    private int comboCount = 0; 
    private Vector3 originalPosition; 
    private Coroutine shakeCoroutine;
    private Coroutine fadeCoroutine;

    void Start()
    {
        originalPosition = comboText.rectTransform.localPosition; 
        UpdateComboText(); 
        comboText.alpha = 0; 
    }

    public void IncrementCombo()
    {
        comboCount++;
        UpdateComboText();

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine); 
        }
        shakeCoroutine = StartCoroutine(ShakeText());

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine); 
        }

        comboText.alpha = 1; 
        ChangeTextColor();
    }

    public void ResetCombo()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
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

            float intensity = shakeIntensity.Evaluate(elapsed / shakeDuration); 
            Vector3 offset = new Vector3(
                Random.Range(-1f, 1f) * intensity,
                Random.Range(-1f, 1f) * intensity,
                0
            );

            comboText.rectTransform.localPosition = originalPosition + offset;

            yield return null;
        }

        comboText.rectTransform.localPosition = originalPosition; 
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
            comboText.alpha = Mathf.Lerp(startAlpha, 0, elapsed / fadeDuration);
            yield return null;
        }

        comboText.alpha = 0; 
        comboCount = 0; 
        UpdateComboText(); 
        comboText.color = Color.white; 
    }
}

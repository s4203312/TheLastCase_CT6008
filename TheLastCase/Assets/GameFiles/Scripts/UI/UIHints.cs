using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHints : MonoBehaviour
{
    public static UIHints Instance;

    [SerializeField] private CanvasGroup messageCanvasGroup;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine currentMessageCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        messageCanvasGroup.alpha = 0f;
        messageCanvasGroup.interactable = false;
        messageCanvasGroup.blocksRaycasts = false;
    }

    // Show a single message
    public void ShowMessage(string message, float duration)
    {
        if (currentMessageCoroutine != null)
        {
            StopCoroutine(currentMessageCoroutine);
        }

        currentMessageCoroutine = StartCoroutine(FadeMessageRoutine(message, duration));
    }

    // Show multiple messages in sequence
    public void ShowMessages(List<(string message, float duration)> messages)
    {
        if (currentMessageCoroutine != null)
        {
            StopCoroutine(currentMessageCoroutine);
        }

        currentMessageCoroutine = StartCoroutine(SequentialMessagesRoutine(messages));
    }

    private IEnumerator FadeMessageRoutine(string message, float duration)
    {
        messageText.text = message;
        yield return StartCoroutine(FadeCanvasGroup(messageCanvasGroup, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(FadeCanvasGroup(messageCanvasGroup, 1f, 0f, fadeDuration));
    }

    private IEnumerator SequentialMessagesRoutine(List<(string message, float duration)> messages)
    {
        foreach (var (message, duration) in messages)
        {
            yield return FadeMessageRoutine(message, duration);
        }
    }

    // Cool fade effect on box
    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float from, float to, float duration)
    {
        float elapsed = 0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            canvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        canvasGroup.alpha = to;

        if (to == 0f)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}

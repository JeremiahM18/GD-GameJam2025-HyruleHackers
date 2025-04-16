using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public CanvasGroup toastGroup;
    public TMP_Text toastText;

    private Coroutine hideMessageCoroutine;

    #region Singleton
    private void Awake() {
    if (Instance != null)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
    }
    #endregion

 

    public void showToast(string message, float duration = 2f)
    {
        if (toastText == null || toastGroup == null) return;
        
        toastText.text = message;
        StartCoroutine(ToastSequence(duration));
    }

    private IEnumerator ToastSequence(float duration)
    {
        toastGroup.alpha = 1;
        toastGroup.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        // Fade out
        float fadeTime = 0.5f;
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            toastGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        toastGroup.alpha = 0;
        toastGroup.gameObject.SetActive(false);
    }

}

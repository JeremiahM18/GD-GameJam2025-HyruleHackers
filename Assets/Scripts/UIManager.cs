using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text messageText;

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

    public void ShowMessage(string message, float duration = 2f)
    {
        if(messageText == null)
        {
            return;
        }
        else
        {
            messageText.text = message;
            messageText.transform.parent.gameObject.SetActive(true);
            //Instance.StartCoroutine(HideMessageAfterDelay(duration));
        }
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageText.transform.parent.gameObject.SetActive(false);
    }


}

using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public GameObject DoorMenu;

    AudioSource source;

    #region Singleton

        private void Awake()
    {
        source = GetComponent<AudioSource>();
        if(source == null)
        {
            Debug.LogWarning("No AudioSource founr on MenuScript object.");
        }
    }

    #endregion

    // Temp Start for testing
    private void Start()
    {
        if(DoorMenu != null)
            {
            DoorMenu.SetActive(true);
            }
    }

    private IEnumerator WaitForSoundAndTransition(string sceneName)
    {
        if (source != null && source.clip != null)
        {
            source.Play();
            yield return new WaitForSeconds(source.clip.length); // wait for sound to finish
        }

        if (DoorMenu != null)
        {
            DoorMenu.SetActive(false);
        }

        SceneManager.LoadScene(sceneName);
    }

    public void gotoGame() {
        StartCoroutine(WaitForSoundAndTransition("InnerRoom"));
    }
    public void gotoForest() {
        StartCoroutine(WaitForSoundAndTransition("ForestRoom"));
    }
    public void gotoIce() {
        StartCoroutine(WaitForSoundAndTransition("IceRoom"));
    }
    public void gotoLava() {
        StartCoroutine(WaitForSoundAndTransition("LavaRoom"));
    }
    public void gotoMenu()
    {
        StartCoroutine(WaitForSoundAndTransition("MainMenu"));
    }
    public void gotoGameOver()
    {
        StartCoroutine(WaitForSoundAndTransition("GameOver"));
    }

    public void HideDoorMenu()
    {
        if(DoorMenu != null)
        {
            DoorMenu.SetActive(false);
        }
    }
}

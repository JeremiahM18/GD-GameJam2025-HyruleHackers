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
            DoorMenu.SetActive(false);
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

    public void onClickNo()
    {
        if (DoorMenu != null)
        {
            DoorMenu.SetActive(false);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }

        Debug.Log("Ok, goodbye door menu");
    }

    public void gotoGame() {
        GameManager.instance.lastExit = "ForestToInner";
        StartCoroutine(WaitForSoundAndTransition("InnerRoom"));
    }
    public void gotoForest() {
        GameManager.instance.lastExit = "InnerToForest";
        StartCoroutine(WaitForSoundAndTransition("ForestRoom"));
    }
    public void gotoIce() {
        GameManager.instance.lastExit = "InnerToIce";
        StartCoroutine(WaitForSoundAndTransition("IceRoom"));
    }
    public void gotoLava() {
        GameManager.instance.lastExit = "InnerToLava";
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

    public void goToInnerRoom(string fromRoom)
    {
        GameManager.instance.lastExit = fromRoom + "ToInner";
        Debug.Log("Going to InnerRoom from " + fromRoom);
        SceneManager.LoadScene("InnerRoom");
    }

    public void HideDoorMenu()
    {
        if(DoorMenu != null)
        {
            DoorMenu.SetActive(false);
        }
    }
}

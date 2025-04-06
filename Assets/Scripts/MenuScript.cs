using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    private IEnumerator WaitForSoundAndTransition(string sceneName) {
        AudioSource source = GetComponent<AudioSource>();
        source.Play();
        yield return new WaitForSeconds(source.clip.length); // wait for sound to finish
        SceneManager.LoadScene(sceneName); // Load next scene
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
}

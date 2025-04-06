using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool hasFireGem = false;
    public bool hasIceGem = false;
    public bool hasForestGem = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Keep it between scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

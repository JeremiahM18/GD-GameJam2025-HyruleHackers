using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool hasFireGem = false;
    public bool hasIceGem = false;
    public bool hasForestGem = false;

    #region Singleton
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
    #endregion


    private void Start()
    {
        hasFireGem = PlayerPrefs.GetInt("FireGem", 0) == 1;
        hasIceGem = PlayerPrefs.GetInt("IceGem", 0) == 1;
        hasForestGem = PlayerPrefs.GetInt("ForestGem", 0) == 1;
    }
    public void SetGemState(string type, bool value)
    {
        switch (type)
        {
            case "fire":
                hasFireGem = value;
                PlayerPrefs.SetInt("FireGem", value ? 1 : 0);
                break;

            case "ice":
                hasIceGem = value;
                PlayerPrefs.SetInt("IceGem", value ? 1 : 0);
                break;

            case "forest":
                hasForestGem = value;
                PlayerPrefs.SetInt("ForestGem", value ? 1 : 0);
                break;
        }
    }

    public bool GetGemState(string type)
    {
        return type switch
        {
            "fire" => hasFireGem,
            "ice" => hasIceGem,
            "forest" => hasForestGem,
            _ => false
        };
    }

    public void QuitGame()
    {
        Debug.Log("Quit the game");
        Application.Quit();
    }
}


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

    public void SetGemState(string type, bool value)
    {
        switch (type)
        {
            case "fire": hasFireGem = value; break;
            case "ice": hasIceGem = value; break;
            case "forest": hasForestGem= value; break;
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
}


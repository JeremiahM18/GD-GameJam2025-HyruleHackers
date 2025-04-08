using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
   public static GameSaveManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SaveData(PlayerController player)
    {
        // PlayerPrefs.SetInt("LifeCount", player.lifeCount);
        // PlayerPrefs.SetInt("KeyCount", player.keyCount);
        PlayerPrefs.SetInt("Coins", player.GetCoinCount());

        PlayerPrefs.SetInt("FireGem", player.HasFireGem() ? 1 : 0);
        PlayerPrefs.SetInt("IceGem", player.HasIceGem() ? 1 : 0);
        PlayerPrefs.SetInt("ForestGem", player.HasForestGem() ? 1 : 0);
    }

    public void LoadData(PlayerController player)
    {
        // player.lifeCount = PlayerPrefs.GetInt("LifeCount", 3);
        // player.lifeCount = PlayerPrefs.GetInt("KeyCount", 1);
        player.SetCoinCount(PlayerPrefs.GetInt("Coins", 0));

        player.SetGemState("fire", PlayerPrefs.GetInt("FireGem", 0) == 1);
        player.SetGemState("ice", PlayerPrefs.GetInt("IceGem", 0) == 1);
        player.SetGemState("forest", PlayerPrefs.GetInt("ForestGem", 0) == 1);
    }
}

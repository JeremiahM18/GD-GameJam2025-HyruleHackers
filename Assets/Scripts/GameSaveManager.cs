using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;

    // for win
    public GameObject pedestal;
    public GameObject confettiPrefab;

    #region Singleton
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
    #endregion

    public void SaveData()
    {
        // PlayerPrefs.SetInt("LifeCount", player.lifeCount);
        // PlayerPrefs.SetInt("KeyCount", player.keyCount);
        //PlayerPrefs.SetInt("Coins", player.GetCoinCount());

        //PlayerPrefs.SetInt("FireGem", player.HasFireGem() ? 1 : 0);
        //PlayerPrefs.SetInt("IceGem", player.HasIceGem() ? 1 : 0);
        //PlayerPrefs.SetInt("ForestGem", player.HasForestGem() ? 1 : 0);

        PlayerPrefs.SetInt("FireGem", GameManager.instance.hasFireGem ? 1 : 0);
        PlayerPrefs.SetInt("IceGem", GameManager.instance.hasIceGem ? 1 : 0);
        PlayerPrefs.SetInt("ForestGem", GameManager.instance.hasForestGem ? 1 : 0);

    }

    public void LoadData()
    {
        // player.lifeCount = PlayerPrefs.GetInt("LifeCount", 3);
        // player.lifeCount = PlayerPrefs.GetInt("KeyCount", 1);
        //player.SetCoinCount(PlayerPrefs.GetInt("Coins", 0));

        //player.SetGemState("fire", PlayerPrefs.GetInt("FireGem", 0) == 1);
        //player.SetGemState("ice", PlayerPrefs.GetInt("IceGem", 0) == 1);
        //player.SetGemState("forest", PlayerPrefs.GetInt("ForestGem", 0) == 1);
        GameManager.instance.SetGemState("fire", PlayerPrefs.GetInt("FireGem", 0) == 1);
        GameManager.instance.SetGemState("ice", PlayerPrefs.GetInt("IceGem", 0) == 1);
        GameManager.instance.SetGemState("forest", PlayerPrefs.GetInt("ForestGem", 0) == 1);

        //if (player.HasFireGem() && player.HasIceGem() && player.HasForestGem())
        //{
        //    TriggerWinCondition();
        //}
        if (HasAllTriangles())
        {
            TriggerWinCondition();
        }

    }

    public static bool HasAllTriangles()
    {
        return PlayerPrefs.GetInt("FireGem", 0) == 1 &&
               PlayerPrefs.GetInt("IceGem", 0) == 1 &&
               PlayerPrefs.GetInt("ForestGem", 0) == 1;
    }

    public void TriggerWinCondition()
    { 
        ChangePedestalImage();
        TriggerConfetti();

    }

    public void TriggerConfetti()
    {
        Instantiate(confettiPrefab, pedestal.transform.position, Quaternion.identity);
    }

    private void ChangePedestalImage()
    {
        SpriteRenderer pedestalSpriteRenderer = pedestal.GetComponent<SpriteRenderer>();
        Sprite newImage = Resources.Load<Sprite>("altar 3");  
        pedestalSpriteRenderer.sprite = newImage;

        //player.SetGemState("fire", PlayerPrefs.GetInt("FireGem", 0) == 1);
        //player.SetGemState("ice", PlayerPrefs.GetInt("IceGem", 0) == 1);
        //player.SetGemState("forest", PlayerPrefs.GetInt("ForestGem", 0) == 1);

        GameManager.instance.SetGemState("fire", PlayerPrefs.GetInt("FireGem", 0) == 1);
        GameManager.instance.SetGemState("ice", PlayerPrefs.GetInt("IceGem", 0) == 1);
        GameManager.instance.SetGemState("forest", PlayerPrefs.GetInt("ForestGem", 0) == 1);


        //player.SetGemState("fire", PlayerPrefs.GetInt("FireGem", 0) == 1);
        //player.SetGemState("ice", PlayerPrefs.GetInt("IceGem", 0) == 1);
        //player.SetGemState("forest", PlayerPrefs.GetInt("ForestGem", 0) == 1);

        GameManager.instance.SetGemState("fire", PlayerPrefs.GetInt("FireGem", 0) == 1);
        GameManager.instance.SetGemState("ice", PlayerPrefs.GetInt("IceGem", 0) == 1);
        GameManager.instance.SetGemState("forest", PlayerPrefs.GetInt("ForestGem", 0) == 1);
    }
}

    


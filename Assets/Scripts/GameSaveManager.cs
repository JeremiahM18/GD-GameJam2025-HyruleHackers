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
        // Currently inside GameManager SetGemState.
        // Add logic to save other data like coins, health, items etc.
    }

    public void LoadData()
    {
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
        if (pedestal == null) return;

    SpriteRenderer pedestalSpriteRenderer = pedestal.GetComponent<SpriteRenderer>();
        if (pedestalSpriteRenderer != null)
        {
            Sprite newImage = Resources.Load<Sprite>("altar 3");
            if (newImage != null)
            {
                pedestalSpriteRenderer.sprite = newImage;
            } else
            {
                Debug.LogWarning("Could not load sprite");
            }
        }
    }
}

    


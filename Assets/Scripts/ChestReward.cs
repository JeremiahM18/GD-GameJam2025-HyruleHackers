using UnityEngine;

public class ChestReward : MonoBehaviour
{
    public enum RewardType {FireGem, IceGem, ForestGem, Key, Sword}
    public RewardType reward;

    public GameObject rewardSprite;
    private bool opened = false;
    private bool waitForEnemies = false;

    private void Start()
    {
        // Hide chest until enemies are defeated
        if (ShouldWaitForEnemies())
        {
            waitForEnemies = true;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (waitForEnemies)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if(enemies.Length == 0)
            {
                gameObject.SetActive(true);
                waitForEnemies=false;
            }
        }
    }

    private bool ShouldWaitForEnemies()
    {
        return reward == RewardType.FireGem;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (opened || !other.CompareTag("Player"))
        {
            return;
        }
        else
        {
            opened = true;

            Animator anim = GetComponent<Animator>();
            if (anim != null) {
                anim.SetTrigger("Open");
            }

            // Enable gem
            if (rewardSprite != null)
            {
                rewardSprite.SetActive(true);
            }

            var player = other.GetComponent<PlayerController>();

            switch (reward)
            {
                case RewardType.FireGem:
                    GameManager.instance.SetGemState("fire", true);
                    UIManager.Instance.ShowMessage("You found the Fire Gem!");
                    break;

                case RewardType.IceGem:
                    GameManager.instance.SetGemState("ice", true);
                    UIManager.Instance.ShowMessage("You found the Ice Gem!");
                    break;

                case RewardType.ForestGem:
                    GameManager.instance.SetGemState("forest", true);
                    UIManager.Instance.ShowMessage("You found the Forest Gem!");
                    break;

                case RewardType.Key:
                    player?.AddKey();
                    UIManager.Instance.ShowMessage("You found a Key!");
                    break;
                case RewardType.Sword:
                    player?.UnlockSword();
                    UIManager.Instance.ShowMessage("You found the Sword!");
                    break;
            }

            GameSaveManager.instance.SaveData();
        }
    }

}

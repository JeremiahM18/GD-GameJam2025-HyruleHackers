using UnityEngine;

public class ChestReward : MonoBehaviour
{
    public enum RewardType {FireGem, IceGem, ForestGem, Key}
    public RewardType reward;

    public GameObject rewardSprite;
    private bool opened = false;

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
                    other.GetComponent<PlayerController>().AddKey();
                    UIManager.Instance.ShowMessage("You found a Key!");
                    break;
            }

            GameSaveManager.instance.SaveData();
        }
    }

}

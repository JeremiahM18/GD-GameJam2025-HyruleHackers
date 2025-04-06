using UnityEngine;

public class Coins : MonoBehaviour
{
    public int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null && pc.AddCoin(coinValue))
            {
                Destroy(gameObject);
            }
        }
    }
}

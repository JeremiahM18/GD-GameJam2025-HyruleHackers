using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public int health = 2;

    private Vector2 direction;
    private float changeDirectionTimer = 2f;

    private void Start()
    {
        ChangeDirection();
    }

    private void Update()
    {
        changeDirectionTimer += Time.deltaTime;
        if (changeDirectionTimer > 2f)
        {
            ChangeDirection();
            changeDirectionTimer = 0f;
        }

        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void ChangeDirection()
    {
        int randomDir = Random.Range(0, 4);
        direction = randomDir switch
        {
            0 => Vector2.up,
            1 => Vector2.down,
            2 => Vector2.left,
            3 => Vector2.right,
            _ => Vector2.zero,
        };
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 
        }
    }
}

using System.Collections;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public int health = 2;

    private Vector2 direction;
    private float changeDirectionTimer = 2f;

    // Enemy Animator
    private Animator animator;

    // Coroutine
    private IEnumerator Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
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

        if (animator != null)
        {
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
            animator.SetBool("IsMoving", true);
        }

        transform.Translate(direction * moveSpeed * Time.deltaTime);

        //Lock Z Axis
        Vector3 fixedPosition = transform.position;
        fixedPosition.z = 0f;
        transform.position = fixedPosition;
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
            StartCoroutine(Die());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // player lose health and maybe damage sound?
        }
    }
}

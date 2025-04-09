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
    private Rigidbody2D rb2d;

    // Cooldown
    private float timeSinceCollision = 0f;
    private float collisionCooldown = 0.2f;

    // Intellegence
    public float aggroRange = 4f;
    private Transform player;
    private bool isChasing = false;

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
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Freeze rotation so enemy stays upright
        rb2d.freezeRotation = true;

        ChangeDirection();
    }

    private void Update()
    {
        changeDirectionTimer += Time.deltaTime;

        // Check if player is in range
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            isChasing = distanceToPlayer <= aggroRange;
        }

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

        //Lock Z Axis
        Vector3 fixedPosition = transform.position;
        fixedPosition.z = 0f;
        transform.position = fixedPosition;
    }

    private void FixedUpdate()
    {
        Vector2 moveDirection;
        if (isChasing && player != null)
        {
            moveDirection = (player.position - transform.position).normalized;
        } else
        {
            moveDirection = direction;
        }

        rb2d.MovePosition(rb2d.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    void ChangeDirection()
    {
        Vector2 oldDirection = direction;

        do
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
        } while (direction == oldDirection);
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
        else
        {
            // Enemy hit a wall or other object - change direction
            if (Time.time - timeSinceCollision > collisionCooldown)
            {

                ChangeDirection();
                timeSinceCollision = Time.time;
            }
        }
    }
}

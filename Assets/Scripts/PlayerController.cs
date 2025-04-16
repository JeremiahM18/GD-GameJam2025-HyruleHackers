/*
Notes
******* Audio Sources[] *******
0 = sword slash
1 = lose health
2 = gain health
3 = death
4 = open chest
5 = level solve

*******
*/

using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public TimerBehavior timerBehavior;
    public GameObject instructionsPanel;
    public GameObject PauseMenu;

    public GameObject[] lifeIcon;
    public GameObject[] keyIcon;

    // Door Menu
    public GameObject doorMenu;
    public GameObject hasKey;
    public GameObject hasForestKey;
    public GameObject hasIceKey;
    public GameObject hasLavaKey;
    public GameObject locked;

    // Sounds
    public AudioClip[] soundClips;
    public AudioSource audioSource;

    public static int lifeCount = 3;

    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float moveX;
    private float moveY;

    private bool isPausedForInstructions = false;
    private bool isRecovering = false;

    // public Transform RespawnPoint;
    private const float recoveryDuration = 1.0f;
    private Vector2 RespawnPoint = new Vector2(-4.0f, 0);

    private static bool hasSword = false;
    private static int keyCount = 0; // forest >=1, ice >= 2, lava >= 3

    // Coin Collect
    private int triangleCoins = 0;
    private const int maxCoins = 99;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (SceneManager.GetActiveScene().name == "IceRoom")
        {
            isPausedForInstructions = true;
            instructionsPanel.SetActive(true); // Show your instructions popup
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        moveX = movementVector.x;
        moveY = movementVector.y;
    }

    public void continueFromInstructions()
    {
        animator.enabled = true;
        isPausedForInstructions = false;
        instructionsPanel.SetActive(false);
        timerBehavior.RestartTimer();

        // Force update animator
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        animator.SetBool("IsMoving", moveX != 0 || moveY !=0);
    }

    void Update()
    {
        // Movement
        if (isPausedForInstructions || doorMenu.activeSelf)
        {
            rb2d.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return;
        }
        else
        {
            Vector2 movement = new Vector2(moveX, moveY);
            rb2d.linearVelocity = movement;

            // Animation movement parameters
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
            animator.SetBool("IsMoving", moveX !=0 || moveY !=0);

            //Sets HasSword parameter if player has sword
            animator.SetBool("HasSword", hasSword);
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.Space) && hasSword)
        {
            //play attack sound and animation
            PlaySound(0);
            animator.SetTrigger("Attack");
        }

        //Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // GameObject.FindGameObjectWithTag("Enemy").GetComponent<BasicEnemy>().SetActive(false);
            GetComponent<PlayerController>().enabled = false;
            PauseMenu.gameObject.SetActive(true);
        }
    }

    #region Collisions + Triggers

    void OnCollisionEnter2D(Collision2D other)
    {
        // Doors
        switch (other.gameObject.tag)
        {
            case "ForestDoor":
                doorMenu.SetActive(true);
                if (keyCount >= 1) hasForestKey.SetActive(true);
                else StartCoroutine(doorLocked());
                break;

            case "IceDoor":
                doorMenu.SetActive(true);
                if (keyCount >= 2) hasIceKey.SetActive(true);
                else StartCoroutine(doorLocked());
                break;

            case "LavaDoor":
                doorMenu.SetActive(true);
                if (keyCount >= 3) hasLavaKey.SetActive(true);
                else StartCoroutine(doorLocked());
                break;

            case "Door":
                doorMenu.SetActive(true);
                hasKey.SetActive(true);
                break;

            case "Enemy":
                loseHealth();
                break;

            case "IcicleWall":
                transform.position = RespawnPoint;
                loseHealth();
                timerBehavior?.RestartTimer();
                break;

            case "Pedestal":
                if (GameSaveManager.HasAllTriangles())
                {
                    GameSaveManager.instance.TriggerConfetti();
                    GameSaveManager.instance.TriggerWinCondition();
                }
                break;
        }
    }


    void OnTriggerEnter2D(Collider2D pickup)
    {
        switch (pickup.tag)
        {
            case "Heart":
                gainHealth();
                break;

            case "Sword":
                pickup.gameObject.SetActive(false);
                hasSword = true;
                UIManager.Instance.showToast("You have the Sword!");
                break;
        }
    }

    #endregion

    #region Health & Recover

    private void loseHealth()
    {
        if (isRecovering || lifeCount <= 0) return;

        PlaySound(1);
        lifeCount--;

        if (lifeCount >= 0 && lifeCount < lifeIcon.Length)
        {
            lifeIcon[lifeCount].SetActive(false);
        }

        if (lifeCount == 0)
        {
            animator.SetTrigger("Death");
            StartCoroutine(Death());
        }
        else
        {
            StartCoroutine(RecoveryFlash());
        }
    }

    private void gainHealth()
    {
        if (lifeCount < lifeIcon.Length)
        {
            PlaySound(2);
            lifeIcon[lifeCount].gameObject.SetActive(true);
            lifeCount++;
        }
    }

    private IEnumerator RecoveryFlash()
    {

        isRecovering = true;
        float elapsed = 0f;

        while (elapsed < recoveryDuration)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        isRecovering = false;
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameOver");
    }

    #endregion

    #region UI & Effects
    public void ContinueFromInstructions()
    {
        animator.enabled = true;
        isPausedForInstructions = false;
        instructionsPanel.SetActive(false);
        timerBehavior.RestartTimer();
    }

    // Occurs whenever player hits a locked door
    IEnumerator doorLocked()
    {
        locked.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        locked.SetActive(false);
        doorMenu.SetActive(false);
    }

    void PlaySound(int soundIndex)
    {
        if (soundIndex >= 0 && soundIndex < soundClips.Length)
        {
            audioSource.clip = soundClips[soundIndex];
            audioSource.Play();  // Play selected sound
        }
    }


    #endregion


    #region Inventory
    // Handle all pick up items
    public void AddKey()
    {
        if (keyCount < keyIcon.Length)
        {
            keyIcon[keyCount].SetActive(true);
            keyCount++;
            PlaySound(4);
            UIManager.Instance.showToast("You found a Key!");
        }
    }
    public void UnlockSword()
    {
        hasSword = true;
        PlaySound(5);
    }

    public int GetCoinCount() => triangleCoins;
    public void SetCoinCount(int amount) => triangleCoins = Mathf.Clamp(amount, 0, maxCoins);

    public bool AddCoin(int amount)
    {
        if (triangleCoins >= maxCoins)
        {
            return false;
        }
        triangleCoins += amount;
        if (triangleCoins > maxCoins)
        {
            triangleCoins = maxCoins;
        }
        return true;
    }

    public int GetKeyCount()
    {
        return keyCount;
    }

    public void SetKeyCount(int value)
    {
        keyCount = value;
    }

    #endregion

}

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
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameSaveManager gameSaveManager;

    public GameObject instructionsPanel;
    private bool isPausedForInstructions = false;

    public TimerBehavior timerBehavior;
    public TMP_Text messageText;
    // public Transform RespawnPoint;
    private Vector2 RespawnPoint;
    private float moveX;
    private float moveY;
    private Animator animator;
    private Animator chestAnim;
    private Rigidbody2D rb2d;
    public float speed = 1.0f;

    public static int lifeCount = 3;
    public GameObject[] lifeIcon;
    public GameObject[] keyIcon;

    // Recover Time
    private bool isRecovering = false;
    public float recoveryDuration = 1.0f;
    private SpriteRenderer spriteRenderer;

    private static bool hasSword = false;
    private static int keyCount = 0; // forest >=1, ice >= 2, lava >= 3
    private static bool hasFireTriangle => GameManager.instance.hasFireGem;
    private static bool hasIceTriangle => GameManager.instance.hasIceGem;
    private static bool hasForestTriangle => GameManager.instance.hasForestGem;

    public GameObject PauseMenu;

    // Door Menu
    public GameObject doorMenu;
    public GameObject hasKey;
    public GameObject hasForestKey;
    public GameObject hasIceKey;
    public GameObject hasLavaKey;
    public GameObject locked;
    public GameObject key;

    // Coin Collect
    private int triangleCoins = 0;
    private const int maxCoins = 99;

    // Sounds
    public AudioClip[] soundClips; 
    public AudioSource audioSource; 


    // Coroutines

    // Occurs whenever player hits a locked door
    IEnumerator doorLocked() {
        locked.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        locked.SetActive(false);
        doorMenu.SetActive(false);
    }

    IEnumerator Death()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  
        if (messageText != null && messageText.transform.parent != null)
        {
            messageText.transform.parent.gameObject.SetActive(false);  
        }
    }

    IEnumerator HideKey()
    {
        yield return new WaitForSeconds(1.5f);
        key.SetActive(false);
    }

    IEnumerator FollowUpMessage(string followUpText, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowMessage(followUpText);
    }

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

        if (messageText != null && messageText.transform.parent != null)
        {
            messageText.transform.parent.gameObject.SetActive(false);
        }

        if (key != null)
        {
            key.SetActive(false);
        }

        // IceRoom Respawn
        RespawnPoint = new Vector2(-4.0f, 0);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnMove(InputValue movementValue) {
        Vector2 movementVector = movementValue.Get<Vector2>();
        moveX = movementVector.x;
        moveY = movementVector.y;
    }
    void Update()
    {
        // Movement
        if (isPausedForInstructions || doorMenu.gameObject.activeSelf == true) {
            rb2d.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return;
        }
        else {
        Vector2 movement = new Vector2(moveX, moveY);
        rb2d.linearVelocity = movement;
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.Space) && hasSword == true) {
            //play attack sound and animation
            PlaySound(0);
            animator.SetTrigger("Attack");
        }

        // Animation movement parameters
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

        //animator.SetBool("IsMoving", moveX != 0 || moveY != 0);
        if (moveX != 0 || moveY != 0)
        {
            animator.SetBool("IsMoving", true); // Force the transition when moving
        }
        else
        {
            animator.SetBool("IsMoving", false); // Transition back to idle when not moving
        }

        //Sets HasSword parameter if player has sword
        animator.SetBool("HasSword", hasSword);

        //Pause
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // GameObject.FindGameObjectWithTag("Enemy").GetComponent<BasicEnemy>().SetActive(false);
            GetComponent<PlayerController>().enabled = false;
            PauseMenu.gameObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (isPausedForInstructions || doorMenu.gameObject.activeSelf == false)
        {
            Vector2 movement = new Vector2(moveX, moveY);
            rb2d.linearVelocity = movement * speed;
        }
    }

    // Handle all pick up items
    void OnTriggerEnter2D (Collider2D pickup) {
        // Fire Triangle
        if (pickup.gameObject.CompareTag("FireTriangle"))
        {
            SetGemState("fire", true);
            pickup.gameObject.SetActive(false);
            ShowMessage("The shard of Strength!");
        }
        // Ice Triangle
        else if (pickup.gameObject.CompareTag("IceTriangle")) 
        {
            SetGemState("ice", true);
            pickup.gameObject.SetActive(false);
            ShowMessage("The shard of Intellect!");
            StartCoroutine(FollowUpMessage("You're almost there!", 2f));
        }
        // Forest Triangle
        else if (pickup.gameObject.CompareTag("ForestTriangle")) {
            SetGemState("forest", true);
            PlaySound(5);
            pickup.gameObject.SetActive(false);
            ShowMessage("The shard of Creativity!");
            StartCoroutine(FollowUpMessage("One more left!", 2f));
        }

        // Heart
         else if (pickup.gameObject.CompareTag("Heart")) {
            gainHealth();
        }

        // Sword
        else if (pickup.gameObject.CompareTag("Sword"))
        {
            pickup.gameObject.SetActive(false);
            hasSword = true;
        }
            
    }

    void OnCollisionEnter2D (Collision2D other) 
    {
        // Doors
        if (other.gameObject.CompareTag("ForestDoor")) {
                doorMenu.SetActive(true);
                if (keyCount >= 1) {
                    // hasKey = GameObject.Find("HasForestKey");
                    hasForestKey.SetActive(true);
                } else {
                    StartCoroutine(doorLocked());
                }
        }
        else if (other.gameObject.CompareTag("IceDoor")) {
            doorMenu.SetActive(true);
                if (keyCount >= 2) {
                    hasIceKey.SetActive(true);
                } else {
                    StartCoroutine(doorLocked());
                }
        }
        else if (other.gameObject.CompareTag("LavaDoor")) {
            doorMenu.SetActive(true);
                if (keyCount >= 3) {
                    hasLavaKey.SetActive(true);
                } else {
                    StartCoroutine(doorLocked());
                }
        }
        else if (other.gameObject.CompareTag("Door")) {
            doorMenu.SetActive(true);
            hasKey.SetActive(true);
        }

        // Lose life when hit enemy
        else if (other.gameObject.CompareTag("Enemy")) {
            loseHealth();
        }

        else if (other.gameObject.CompareTag("IcicleWall"))
        {
            // transform.position = RespawnPoint.position;
            transform.position = RespawnPoint;
            loseHealth();

            if (timerBehavior != null)
            {
                timerBehavior.RestartTimer();
            }

            else if (other.gameObject.CompareTag("Pedestal") && GameSaveManager.HasAllTriangles())
            {
                GameSaveManager.instance.TriggerConfetti();
                GameSaveManager.instance.TriggerWinCondition();
            }
        }


        if (other.gameObject.CompareTag("Chest"))
        {
            chestAnim = other.gameObject.GetComponent<Animator>();
            if (chestAnim != null)
            {
                chestAnim.SetTrigger("Open");
                 if (keyCount < keyIcon.Length)
                 {
                    keyIcon[keyCount].gameObject.SetActive(true);
                 }
                PlaySound(4);
                key.SetActive(true);
                keyCount = keyCount + 1;


                ShowMessage("Yay! You found a key!");


                StartCoroutine(HideKey());
            }
        }

        if (other.gameObject.CompareTag("Pedestal") && GameSaveManager.HasAllTriangles())
        {
            GameSaveManager.instance.TriggerConfetti();
            GameSaveManager.instance.TriggerWinCondition();
        }

                StartCoroutine(HideKey());

                ShowMessage("Yay! You found a key!");

                StartCoroutine(HideKey());
            }
      

    

    // Instructions Ice
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "IceRoom")
        {
            instructionsPanel.SetActive(true);
        }
    }

    public void ContinueFromInstructions()
    {
        animator.enabled = true;
        instructionsPanel.SetActive(false);
        isPausedForInstructions = false;
        if (timerBehavior != null) timerBehavior.isPaused = true;

        if (timerBehavior != null)
        {
            timerBehavior.isPaused = false;
            timerBehavior.RestartTimer();
        }
    }

    void ShowMessage(string message)
    {
        if (messageText != null && messageText.transform.parent != null)
        {
            messageText.text = message;
            messageText.transform.parent.gameObject.SetActive(true);


            StartCoroutine(HideMessageAfterDelay(2f));

        }
    }

    void PlaySound(int soundIndex)
    {
        if (soundIndex >= 0 && soundIndex < soundClips.Length)
        {
            audioSource.clip = soundClips[soundIndex];
            audioSource.Play();  // Play selected sound
        }
    }

        void loseHealth() {

        if (isRecovering || lifeCount <= 0)
        {
            return;
        }

        PlaySound(1);

        lifeCount--;

        if(lifeCount >= 0 && lifeCount < lifeIcon.Length)
        {
            lifeIcon[lifeCount].gameObject.SetActive(false);
        }

        if(lifeCount == 0) {

            animator.SetTrigger("Death");
            PlaySound(3);
            // fade screen to black
            StartCoroutine(Death());
            }
        else
        {
            StartCoroutine(RecoveryFlash());
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

    void gainHealth()
    {
        if (lifeCount < lifeIcon.Length)
        {
            PlaySound(2);
            lifeIcon[lifeCount].gameObject.SetActive(true);
            lifeCount++;
        }
    }

    #region Coins
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

    public void AddKey()
    {
        if (keyCount < keyIcon.Length)
        {
            keyIcon[keyCount].SetActive(true);
            keyCount++;
            PlaySound(4);
            key.SetActive(true);
            ShowMessage("You found a key!");
            StartCoroutine(HideKey());
        }
    }

    public int GetCoinCount() => triangleCoins;

    public void SetCoinCount(int amount)
    {
        triangleCoins = Mathf.Clamp(amount, 0, maxCoins);
    }

    #endregion

    #region Gem States

    public bool HasFireGem() => hasFireTriangle;
    public bool HasIceGem() => hasIceTriangle;

    public bool HasForestGem() => hasForestTriangle;

    public void SetGemState(string type, bool state)
    {

        GameManager.instance.SetGemState(type, state);
        //switch (type)
        //{
        //    case "fire": hasFireTriangle = state; break;
        //    case "ice": hasIceTriangle = state; break;
        //    case "forest": hasForestTriangle = state; break;
        //}
    
    }
    #endregion
}

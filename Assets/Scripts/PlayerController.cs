/*
Notes
******* Audio Sources[] *******
0 = sword slash
1 = lose health
2 = gain health
3 = death

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
    public GameObject instructionsPanel; 
    private bool isPausedForInstructions = false;

    public TimerBehavior timerBehavior;
    public TMP_Text messageText;
    public Transform RespawnPoint;
    private float moveX;
    private float moveY;
    private Animator animator;
    private Animator chestAnim;
    private Rigidbody2D rb2d;
    public float speed = 4.5f;

    public int lifeCount = 3;
    public GameObject[] lifeIcon;
    public GameObject[] keyIcon;

    private bool hasSword = false;
    private int keyCount = 1; // forest >=1, ice >= 2, lava >= 3
    private bool hasFireTriangle = false;
    private bool hasIceTriangle = false;
    private bool hasForestTriangle = false;

    // Door Menu
    public GameObject doorMenu;
    public GameObject hasKey;
    public GameObject locked;
    public GameObject key;

    // Coin Collect
    private int triangleCoins = 0;
    private const int maxCoins = 99;

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

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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
        if (Input.GetButtonDown("Fire1") && hasSword == true) {
            //play attack sound and animation

            animator.SetTrigger("Attack");
        }

        // Animation movement parameters
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

        animator.SetBool("IsMoving", moveX != 0 || moveY != 0);

        //Sets HasSword parameter if player has sword
        animator.SetBool("HasSword", hasSword);
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
        if (pickup.gameObject.CompareTag("FireTriangle")) {
            hasFireTriangle = true;
        }
        // Ice Triangle
        else if (pickup.gameObject.CompareTag("IceTriangle")) {
            hasIceTriangle = true;
            pickup.gameObject.SetActive(false);
        }
        // Forest Triangle
        else if (pickup.gameObject.CompareTag("ForestTriangle")) {
            hasForestTriangle = true;
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

    void OnCollisionEnter2D (Collision2D other) {
        // Doors
        if (other.gameObject.CompareTag("ForestDoor")) {
                doorMenu.SetActive(true);
                if (keyCount >= 1) {
                    // hasKey = GameObject.Find("HasForestKey");
                    hasKey.SetActive(true);
                } else {
                    StartCoroutine(doorLocked());
                }
        }
        else if (other.gameObject.CompareTag("IceDoor")) {
            doorMenu.SetActive(true);
                if (keyCount >= 2) {
                    hasKey.SetActive(true);
                } else {
                    StartCoroutine(doorLocked());
                }
        }
        else if (other.gameObject.CompareTag("LavaDoor")) {
            doorMenu.SetActive(true);
                if (keyCount >= 3) {
                    hasKey.SetActive(true);
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
            transform.position = RespawnPoint.position;
            loseHealth();

            if (timerBehavior != null)
            {
                timerBehavior.RestartTimer();
            }
        }

        if (other.gameObject.CompareTag("Chest"))
        {
            chestAnim = other.gameObject.GetComponent<Animator>();
            if (chestAnim != null)
            {
                chestAnim.SetTrigger("Open");
                // if (keyCount < keyIcon.Length)
                // {
                    keyIcon[keyCount].gameObject.SetActive(true);
                // }
                key.SetActive(true);
                keyCount = keyCount + 1;

                ShowMessage("Yay! You found a key!");

                StartCoroutine(HideKey());
            }
        }
    }

    public void ContinueFromInstructions()
    {
        animator.enabled = true;
        isPausedForInstructions = false;
        instructionsPanel.SetActive(false);
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


            StartCoroutine(HideMessageAfterDelay(3f));

        }
    }
            void loseHealth() {
        // play hurt sound and animation
        lifeIcon[lifeCount - 1].gameObject.SetActive(false);
        lifeCount = lifeCount - 1;
            if (lifeCount == 0) {
            animator.SetTrigger("Death");
            // play death sound 
            // fade screen to black
            StartCoroutine(Death());
            }
    }

    void gainHealth() {
        // play sound + maybe animation?
        // add a life icon
        lifeCount = lifeCount + 1;
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
        switch (type)
        {
            case "fire": hasFireTriangle = state; break;
            case "ice": hasIceTriangle = state; break;
            case "forest": hasForestTriangle = state; break;
        }
    }
    #endregion
}

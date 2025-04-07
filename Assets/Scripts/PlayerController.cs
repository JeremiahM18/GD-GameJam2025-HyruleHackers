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

public class PlayerController : MonoBehaviour
{
    public Transform RespawnPoint;
    private float moveX;
    private float moveY;
    private Rigidbody2D rb2d;
    public float speed = 4.5f;

    public int lifeCount = 4;

    private bool hasSword = false;
    private int keyCount = 1; // forest >=1, ice >= 2, lava >= 3
    private bool hasFireTriangle = false;
    private bool hasIceTriangle = false;
    private bool hasForestTriangle = false;

    // Door Menu
    public GameObject doorMenu;
    public GameObject hasKey;
    public GameObject locked;

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

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue movementValue) {
        Vector2 movementVector = movementValue.Get<Vector2>();
        moveX = movementVector.x;
        moveY = movementVector.y;
    }
    void Update()
    {
        // Movement
        if (doorMenu.gameObject.activeSelf == true) {
            rb2d.linearVelocity = Vector2.zero;
        }
        else {
        Vector2 movement = new Vector2(moveX, moveY);
        rb2d.linearVelocity = movement;
        }

        // Attack
        if (Input.GetButtonDown("Fire1") && hasSword == true) {
            //play attack sound and animation
        }
    }

    // Handle all pick up items
    void OnTriggerEnter2D (Collider2D pickup) {
        // Keys
        if (pickup.gameObject.CompareTag("Key")) {
            pickup.gameObject.SetActive(false);
            keyCount = keyCount + 1;
        }
        // Fire Triangle
        else if (pickup.gameObject.CompareTag("Firetriangle")) {
            hasFireTriangle = true;
        }
        // Ice Triangle
        else if (pickup.gameObject.CompareTag("IceTriangle")) {
            hasIceTriangle = true;
        }
        // Forest Triangle
        else if (pickup.gameObject.CompareTag("ForestTriangle")) {
            hasForestTriangle = true;
        }

        // Heart
         else if (pickup.gameObject.CompareTag("Heart")) {
            gainHealth();
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
        }
    }

    void loseHealth() {
        // play hurt sound and animation
        // remove a life icon
        lifeCount = lifeCount - 1;
            if (lifeCount == 0) {
                // play death sound and animation
                // fade screen to black
                SceneManager.LoadScene("GameOver");
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

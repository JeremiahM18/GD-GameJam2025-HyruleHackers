using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveX;
    private float moveY;
    private Rigidbody2D rb2d;
    public float speed = 4.5f;

    public int lifeCount = 4;

    private bool hasSword = false;
    private bool hasFireKey = false;
    private bool hasIceKey = false;
    private bool hasForestKey = false;
    private bool hasFireTriangle = false;
    private bool hasIceTriangle = false;
    private bool hasForestTriangle = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void OnMove(InputValue movementValue) {
        Vector2 movementVector = movementValue.Get<Vector2>();
        moveX = movementVector.x;
        moveY = movementVector.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        Vector2 movement = new Vector2(moveX, moveY);
        rb2d.linearVelocity = movement;

        // Attack
        if (Input.GetButtonDown("Fire1") && hasSword == true) {
            //play attack sound animation
        }
    }

    // Handle all pick up items
    void onTriggerEnter2D (Collision2D pickup) {
        // Fire Key
        if (pickup.gameObject.CompareTag("FireKey")) {
            hasFireKey = true;
        }
        // Ice Key
        else if (pickup.gameObject.CompareTag("IceKey")) {
            hasIceKey = true;
        }
        // Forest Key
        else if (pickup.gameObject.CompareTag("ForestKey")) {
            hasForestKey = true;
        }
        // Fire Triangle
        else if (pickup.gameObject.CompareTag("FireKey")) {
            hasFireTriangle = true;
        }
        // Ice Triangle
        else if (pickup.gameObject.CompareTag("IceKey")) {
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
        if (other.gameObject.CompareTag("ForestDoor") && hasForestKey == true) {
            SceneManager.LoadScene("ForestRoom");
            // Replace with a pop-up UI scene that asks to enter room
            // Yes button will load scene
        }
        else if (other.gameObject.CompareTag("LavaDoor") && hasFireKey == true) {
            SceneManager.LoadScene("LavaRoom");
        }
        else if (other.gameObject.CompareTag("IceDoor") && hasIceKey == true) {
            SceneManager.LoadScene("IceRoom");
        }
        else if (other.gameObject.CompareTag("Door")) {
            SceneManager.LoadScene("InnerRoom");
        }

        // Lose life when hit enemy
        else if (other.gameObject.CompareTag("Enemy")) {
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
}

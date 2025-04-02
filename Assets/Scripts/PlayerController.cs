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

    void OnCollisionEnter2D (Collision2D other) {
        // Doors
        if (other.gameObject.CompareTag("ForrestDoor")) {
            SceneManager.LoadScene("ForrestRoom");
        }
        else if (other.gameObject.CompareTag("LavaRoom")) {
            SceneManager.LoadScene("LavaRoom");
        }
        else if (other.gameObject.CompareTag("IceDoor")) {
            SceneManager.LoadScene("IceRoom");
        }
        else if (other.gameObject.CompareTag("Door")) {
            SceneManager.LoadScene("InnerRoom");
        }

        // Lose life when hit enemy
        else if (other.gameObject.CompareTag("Enemy")) {
            loseHealth();
        }
        // Gain life when interact with
         else if (other.gameObject.CompareTag("Health")) {
            gainHealth();
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

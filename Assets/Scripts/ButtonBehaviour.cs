using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    private Animator anim;
    // private bool isDown = false;
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            anim.SetTrigger("isDown");
        }
    }
}

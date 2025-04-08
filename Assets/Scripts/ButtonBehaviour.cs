using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    private Animator anim;
    private AudioSource source;
    private Collider2D collider;

    public GameObject triangle;

    void Start() {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
    }
    
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            anim.SetBool("isDown", true);
            source.Play();
            triangle.gameObject.SetActive(false);
            collider.enabled = false;
        }
    }
}

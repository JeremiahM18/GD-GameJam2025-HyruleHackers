using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    // Camera boundaries
    //public float minX;
    //public float maxX;
    //public float minY;
    //public float maxY;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, -10);
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos = player.transform.position + offset;

       // float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
       // float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = player.transform.position + offset;
    }

}

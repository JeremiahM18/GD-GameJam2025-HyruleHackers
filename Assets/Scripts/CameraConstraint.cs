using UnityEngine;

public class CameraConstraint : MonoBehaviour
{
    public Transform player; 
    public Vector2 minPosition; 
    public Vector2 maxPosition; 

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

            // Constrain camera within certain bounds
            float clampedX = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            float clampedY = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

            transform.position = new Vector3(clampedX, clampedY, targetPosition.z);
        }
    }
}


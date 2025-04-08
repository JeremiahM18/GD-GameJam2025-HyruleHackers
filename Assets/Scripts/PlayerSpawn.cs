using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject forestSpawn;
    public GameObject iceSpawn;
    public GameObject lavaSpawn;
    public GameObject innerSpawnForest;
    public GameObject innerSpawnIce;
    public GameObject innerSpawnLava;

    private Vector2 playerPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;

        if (player != null) {
            if (SceneManager.GetActiveScene().name == "ForestRoom") {
                playerPos = forestSpawn.transform.position;
            } else if (SceneManager.GetActiveScene().name == "IceRoom") {
                playerPos = iceSpawn.transform.position;
            } else if (SceneManager.GetActiveScene().name == "LavaRoom") {
                playerPos = lavaSpawn.transform.position;
            }
        }
    }
}

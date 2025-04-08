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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null) {
            if (SceneManager.GetActiveScene().name == "ForestRoom") {
                player.transform.position = forestSpawn.transform.position;
            } else if (SceneManager.GetActiveScene().name == "IceRoom") {
                player.transform.position = iceSpawn.transform.position;
            } else if (SceneManager.GetActiveScene().name == "LavaRoom") {
                player.transform.position = lavaSpawn.transform.position;
            } else if (SceneManager.GetActiveScene().name == "InnerRoom" && player.transform.position.x > 3f) {
                player.transform.position = innerSpawnForest.transform.position;
            } else if (SceneManager.GetActiveScene().name == "InnerRoom" && player.transform.position.x < -3f) {
                player.transform.position = innerSpawnIce.transform.position;
            } else if (SceneManager.GetActiveScene().name == "InnerRoom" && player.transform.position.y < -3f) {
                player.transform.position = innerSpawnIce.transform.position;
            } 
        }
    }
}

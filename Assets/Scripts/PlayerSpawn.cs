using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public Transform forestSpawn;
    public Transform iceSpawn;
    public Transform lavaSpawn;

    public Transform innerSpawnForest;
    public Transform innerSpawnIce;
    public Transform innerSpawnLava;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null ) return;

        string scene = SceneManager.GetActiveScene().name;

        switch (scene)
        {
            case "ForestRoom":
                player.transform.position = forestSpawn.position;
                break;

            case "IceRoom":
                player.transform .position = iceSpawn.position; 
                break;

            case "LavaRoom":
                player.transform.position = lavaSpawn.position;
                break;

            case "InnerRoom":
                Vector3 prevPos = player.transform.position;
                if(prevPos.x > 3f)
                {
                    player.transform.position = innerSpawnForest.position;
                } else if (prevPos.x < -3f)
                {
                    player.transform.position = innerSpawnIce.position;
                } else if (prevPos.y < -3f)
                {
                    player.transform.position = innerSpawnLava.position;
                }
                break;
        }
    }
}

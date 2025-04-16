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

    public Transform innerSpawnDefault;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null ) return;

        string scene = SceneManager.GetActiveScene().name;

        switch (scene)
        {
            case "ForestRoom":
                if(GameManager.instance.lastExit == "InnerToForest")
                    player.transform.position = forestSpawn.position;
                break;

            case "IceRoom":
                if(GameManager.instance.lastExit == "InnerToIce")
                    player.transform .position = iceSpawn.position; 
                break;

            case "LavaRoom":
                if(GameManager.instance.lastExit == "InnerToLava")
                    player.transform.position = lavaSpawn.position;
                break;

            case "InnerRoom":
                switch (GameManager.instance.lastExit)
                {
                    case "ForestToInner":
                        player.transform.position = innerSpawnForest.position;
                        break;

                    case "IceToInner":
                        player.transform.position= innerSpawnIce.position;
                        break;

                    case "LavaToInner":
                        player.transform.position = innerSpawnLava.position;
                        break;
                    default:
                        Debug.LogWarning("Unknown lastExit. Using default Inner Room spawn.");
                        if(innerSpawnDefault != null )
                            player.transform.position += innerSpawnDefault.position;
                        break;
                }

                GameManager.instance.lastExit = "";

                break;
        }
    }
}

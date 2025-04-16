using TMPro;
using UnityEngine;

public class TimerBehavior : MonoBehaviour
{
    // public GameObject player;
    private GameObject player;

    public Transform RespawnPoint;
    public TextMeshProUGUI textField;

    private float timer = 120f;    
    private bool hasRespawned = false;
    public bool isPaused = false;

    void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player not found! Tag a player");
        }

        if (textField == null)
        {
            Debug.LogWarning("Timer text field not assigned.");
        }
        UpdateTimerText();
    }

    void Update()
    {
        if (isPaused || player == null) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0 && !hasRespawned)
        {
            timer = 0;
            hasRespawned = true;

            player.transform.position = RespawnPoint.position;
            RestartTimer();
        }

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        if (textField == null) return;


        int minutes = (int)timer / 60;
        int seconds = (int)timer % 60;
        string message = string.Format("Timer: {0:00}:{1:00}", minutes, seconds);
        textField.text = message;
    }

    public void StartTimer(float time)
    {
        timer = time;
        hasRespawned = false;
        UpdateTimerText();
    }

    public void RestartTimer()
    {
        StartTimer(120);
    }

}

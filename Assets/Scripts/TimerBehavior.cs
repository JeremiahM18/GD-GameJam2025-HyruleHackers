using TMPro;
using UnityEngine;

public class TimerBehavior : MonoBehaviour
{
    // public GameObject player;
    private GameObject player;
    public Transform RespawnPoint;
    private float timer;
    public TextMeshProUGUI textField;
    private bool hasRespawned = false;
    public bool isPaused = false;

    void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player");
        if (textField == null)
        {
            Debug.Log("No TextMeshProUGUI component found.");
        }

        timer = 120f;
        UpdateTimerText();
    }

    void Update()
    {
        if (isPaused) return;

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
        int minutes = (int)timer / 60;
        int seconds = (int)timer % 60;
        string message = string.Format("Timer: {0:00}:{1:00}", minutes, seconds);
        textField.text = message;
    }

    public void StartTimer(float time)
    {
        timer = time;
    }

    public void RestartTimer()
    {
        timer = 120f;
        hasRespawned = false;
    }

}

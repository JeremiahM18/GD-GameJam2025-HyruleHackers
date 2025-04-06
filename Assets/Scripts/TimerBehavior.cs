using TMPro;
using UnityEngine;

public class TimerBehavior : MonoBehaviour
{
    public GameObject player;
    public Transform RespawnPoint;
    private float timer;
    public TextMeshProUGUI textField;

    
    void Start()
    { 
        if (textField == null)
        {
            Debug.Log("No TextMeshProUGUI component found.");
        }

        timer = 45f;
        UpdateTimerText();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            timer = 0;
            player.transform.position = RespawnPoint.position;

        }

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int minutes = (int)timer / 60;
        int seconds = (int)timer % 60;
        string message = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        textField.text = message;
    }

    public void StartTimer(float time)
    {
        timer = time; 
    }

    public void RestartTimer()
    {
        timer = 45f;
    }
}

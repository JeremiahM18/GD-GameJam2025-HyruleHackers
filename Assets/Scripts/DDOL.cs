using UnityEngine;

public class DDOL : MonoBehaviour
{
    public static DDOL Instance;
    private void Awake() {
    if (Instance != null)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
    }
}

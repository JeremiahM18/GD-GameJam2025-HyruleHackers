using UnityEditor.Build.Content;
using UnityEngine;

public class GemShrine : MonoBehaviour
{
    public GameObject fireGemSlot;
    public GameObject iceGemSlot;
    public GameObject forestGemSlot;

    public Sprite litSprite;
    private Vector3 baseScale = Vector3.one;
    void Start()
    {
        UpdateShrine();
    }

    private void Update()
    {
        AnimateGlow(fireGemSlot, GameManager.instance.hasFireGem);
        AnimateGlow(iceGemSlot, GameManager.instance.hasIceGem);
        AnimateGlow(forestGemSlot, GameManager.instance.hasForestGem);
    }
    void UpdateShrine()
    {
        if(GameManager.instance.hasFireGem)
            fireGemSlot.GetComponent<SpriteRenderer>().sprite = litSprite;

        if (GameManager.instance.hasIceGem)
            iceGemSlot.GetComponent<SpriteRenderer>().sprite = litSprite;

        if (GameManager.instance.hasForestGem)
            forestGemSlot.GetComponent<SpriteRenderer>().sprite = litSprite;
    }

    void AnimateGlow(GameObject gemslot, bool isActive)
    {
        if (isActive)
        {
            float scale = 1f + Mathf.Sin(Time.deltaTime * 3f) * 0.05f;
            gemslot.transform.localScale = baseScale * scale;
        }
    }
}

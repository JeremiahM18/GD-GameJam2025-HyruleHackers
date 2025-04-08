using UnityEngine;

public class GemShrine : MonoBehaviour
{
    [Header("Gem Slot Visuals")]
    public GameObject fireGemSlot;
    public GameObject iceGemSlot;
    public GameObject forestGemSlot;

    [Header("Alter Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite alter0Gems;
    public Sprite alter1Gems;
    public Sprite alter2Gems;
    public Sprite alter3Gems;

    [Header("Gem Visual")]
    public Sprite litGemSprite;

    private Vector3 baseScale = Vector3.one;

    private void Update()
    {
        UpdateShrine();
        AnimateGlow(fireGemSlot, GameManager.instance.hasFireGem);
        AnimateGlow(iceGemSlot, GameManager.instance.hasIceGem);
        AnimateGlow(forestGemSlot, GameManager.instance.hasForestGem);
    }
    void UpdateShrine()
    {
        int gemCount = 0;

        if (GameManager.instance.hasFireGem)
        {
            fireGemSlot.GetComponent<SpriteRenderer>().sprite = litGemSprite;
            gemCount++;
        }

        if (GameManager.instance.hasIceGem)
        {
            iceGemSlot.GetComponent<SpriteRenderer>().sprite = litGemSprite;
            gemCount++;
        }

        if (GameManager.instance.hasForestGem)
        {
            forestGemSlot.GetComponent<SpriteRenderer>().sprite = litGemSprite;
            gemCount++;
        }

        // Update the pedestal sprite
        switch (gemCount)
        {
            case 0:
                spriteRenderer.sprite = alter0Gems;
                break;
            case 1:
                spriteRenderer.sprite = alter1Gems;
                break;
            case 2:
                spriteRenderer.sprite = alter2Gems;
                break;
            case 3:
                spriteRenderer.sprite = alter3Gems;
                break;
        }
    }

    void AnimateGlow(GameObject gemslot, bool isActive)
    {
        if (isActive)
        {
            float scale = 1f + Mathf.Sin(Time.time * 3f) * 0.05f;
            gemslot.transform.localScale = baseScale * scale;
        }
    }
}

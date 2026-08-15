using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Vida")]
    public Image[] hearts;
    public Sprite fullHeartSprite; // Sprite de corazón lleno
    public Sprite emptyHeartSprite; // Sprite de corazón vacío

    [Header("Recursos")]
    public TMP_Text onionText;
    public TMP_Text coinText;
    public TMP_Text gemText;
    public TMP_Text rescueText;

    [Header("Inventario")]
    public Image[] inventorySlots;

    public void UpdateHealth(int currentHealth)
    {
        if (hearts == null || hearts.Length == 0) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;
            if (i < currentHealth) hearts[i].sprite = fullHeartSprite;
            else hearts[i].sprite = emptyHeartSprite;

            hearts[i].SetAllDirty();
        }
    }

    public void UpdateOnions(int amount)
    {
        onionText.text = "x" + amount;
    }

    public void UpdateCoins(int amount)
    {
        coinText.text = "x" + amount;
    }

    public void UpdateGems(int amount)
    {
        gemText.text = "x" + amount;
    }

    public void UpdateRescues(int current, int total)
    {
        rescueText.text = current + " / " + total;
    }
}

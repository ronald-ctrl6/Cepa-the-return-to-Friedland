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
    public Sprite aceiteSprite; // Sprite de Aceite
    public Sprite aceiteVacioSprite; // Sprite de Aceite sin recoger
    public Sprite fosforoSprite; // Sprite de Fosforo
    public Sprite fosforoVacioSprite; // Sprite Fosfor sin recoger
    public Sprite guisantesSprite; // Sprite de Guisantes
    public Sprite guisantesVacioSprite; // Sprite de Guisantes sin reoger

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
        onionText.text = "" + amount;
    }

    public void UpdateCoins(int amount)
    {
        coinText.text = "" + amount;
    }

    public void UpdateGems(int amount)
    {
        gemText.text = "" + amount;
    }

    public void UpdateRescues(int current, int total)
    {
        rescueText.text = current + " / " + total;
    }

    public void UpdateItem(int currentItem)
    {
        if (inventorySlots == null || inventorySlots.Length == 0) return;

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == null) continue;
            if (i == 1) inventorySlots[1].sprite = aceiteSprite;
            if (i == 2)
            {
                inventorySlots[1].sprite = aceiteSprite;
                inventorySlots[2].sprite = fosforoSprite;
            }
            if (i == 3) 
            {
                inventorySlots[1].sprite = aceiteSprite;
                inventorySlots[2].sprite = fosforoSprite;
                inventorySlots[3].sprite = guisantesSprite; 
            }

            inventorySlots[i].SetAllDirty();
        }
    }
}

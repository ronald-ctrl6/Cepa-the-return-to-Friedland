using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Vida")]
    public Image[] hearts;

    [Header("Recursos")]
    public TMP_Text onionText;
    public TMP_Text coinText;
    public TMP_Text gemText;
    public TMP_Text rescueText;

    [Header("Inventario")]
    public Image[] inventorySlots;

    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentHealth;
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

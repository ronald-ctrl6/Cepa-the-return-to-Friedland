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

    public static UIManager Instance { get; private set; }

    [Header("Notificaciones / Mensajes")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float messageDuration = 3f;
    private Coroutine messageRoutine;

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

    public void UpdateItem(int slotIndex, Sprite itemSprite)
    {
        if (inventorySlots == null || inventorySlots.Length == 0) return;

        if (slotIndex >= 0 && slotIndex < inventorySlots.Length)
        {
            if (inventorySlots[slotIndex] != null)
            {
                inventorySlots[slotIndex].sprite = itemSprite;

                Color color = inventorySlots[slotIndex].color;
                color.a = 1f;
                inventorySlots[slotIndex].color = color;

                inventorySlots[slotIndex].SetAllDirty();
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (messagePanel != null) messagePanel.SetActive(false);
    }

    // Método para mostrar mensajes flotantes o de aviso
    public void ShowNotification(string message)
    {
        if (messagePanel == null || messageText == null) return;

        messageText.text = message;
        messagePanel.SetActive(true);

        if (messageRoutine != null) StopCoroutine(messageRoutine);
        messageRoutine = StartCoroutine(HideMessageAfterTime());
    }

    private System.Collections.IEnumerator HideMessageAfterTime()
    {
        yield return new WaitForSeconds(messageDuration);
        messagePanel.SetActive(false);
        messageRoutine = null;
    }
}

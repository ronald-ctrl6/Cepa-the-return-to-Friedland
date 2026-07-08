using UnityEngine;

public class ItemCollectible : MonoBehaviour
{
    [Header("Collectible")]
    [SerializeField] private int points = 1;
    [SerializeField] private bool destroyAfterCollect = true;

    private bool wasCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (wasCollected || !other.CompareTag("Player"))
        {
            return;
        }

        wasCollected = true;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(points);
        }

        if (destroyAfterCollect)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
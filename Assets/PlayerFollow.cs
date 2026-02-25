using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Vector3 offset = new Vector3(0, 5, -5); // Mesafe ayarı
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float smoothTime = 0.3f; // Yumuşama süresi

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = player.position + offset;
        // Mevcut pozisyondan hedef pozisyona pürüzsüz geçiş
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}

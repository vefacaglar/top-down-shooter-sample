using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] Material gotHitMaterial;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GetComponent<MeshRenderer>().material = gotHitMaterial;
        }
    }
}

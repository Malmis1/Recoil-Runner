using UnityEngine;

public class BulletScript : MonoBehaviour {
    [Tooltip("Damage dealt by the bullet")]
    public int damage = 1;

    [Tooltip("Life time to the bullet")]
    public float lifeTime = 5f;

    private void Start() {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            // PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            //if (playerHealth != null)
            //{
            //    playerHealth.TakeDamage(damage);
            //}

            Debug.Log("Hit player");

            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            Destroy(gameObject);
            Debug.Log("Ground");
        }
    }
}

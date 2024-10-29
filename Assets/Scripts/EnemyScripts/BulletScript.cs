using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
    [Tooltip("Damage dealt by the bullet")]
    public int damage = 1;

    [Tooltip("Life time to the bullet")]
    public float lifeTime = 5f;

    [Tooltip("Delay before the bullet becomes visible")]
    public float visibilityDelay = 0.02f;

    private void Start() {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(MakeVisibleAfterDelay(visibilityDelay));

        Destroy(gameObject, lifeTime);
    }

    private IEnumerator MakeVisibleAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Debug.Log("Hit player");
            Destroy(gameObject);
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            Debug.Log("Ground");
            Destroy(gameObject);
        }
    }
}

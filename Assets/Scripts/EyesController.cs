using UnityEngine;

public class EyesController : MonoBehaviour
{
    [Tooltip("The eyes object that contains the SpriteRenderer")]
    [SerializeField] private Transform eyes;

    [Tooltip("Sprites for different eye directions")]
    [SerializeField] private Sprite[] eyeSprites;
    private SpriteRenderer eyesSpriteRenderer;

    [Tooltip("Distance threshold to detect if cursor is on player")]
    [SerializeField] private float cursorThreshold = 0.5f; 

    private void Awake() {
        if (eyes != null) {
            eyesSpriteRenderer = eyes.GetComponent<SpriteRenderer>();
        }
    }

    private void Update() {
        if (eyesSpriteRenderer != null) {
            UpdateEyesDirection();
        }
    }

    private void UpdateEyesDirection() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        Vector3 direction = mousePosition - transform.position;

        // Check if the cursor is within the threshold distance
        if (direction.magnitude <= cursorThreshold) {
            eyesSpriteRenderer.sprite = eyeSprites[8]; // Middle
        } else {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            SetEyesSprite(angle);
        }
    }

    private void SetEyesSprite(float angle) {
        if (angle >= -22.5f && angle < 22.5f) {
            eyesSpriteRenderer.sprite = eyeSprites[4]; // Right
        }
        else if (angle >= 22.5f && angle < 67.5f) {
            eyesSpriteRenderer.sprite = eyeSprites[7]; // TopRight
        }
        else if (angle >= 67.5f && angle < 112.5f) {
            eyesSpriteRenderer.sprite = eyeSprites[5]; // Top
        }
        else if (angle >= 112.5f && angle < 157.5f) {
            eyesSpriteRenderer.sprite = eyeSprites[6]; // TopLeft
        }
        else if ((angle >= 157.5f && angle <= 180f) || (angle >= -180f && angle < -157.5f)) {
            eyesSpriteRenderer.sprite = eyeSprites[3]; // Left
        }
        else if (angle >= -157.5f && angle < -112.5f) {
            eyesSpriteRenderer.sprite = eyeSprites[1]; // BottomLeft
        }
        else if (angle >= -112.5f && angle < -67.5f) {
            eyesSpriteRenderer.sprite = eyeSprites[0]; // Bottom
        }
        else if (angle >= -67.5f && angle < -22.5f) {
            eyesSpriteRenderer.sprite = eyeSprites[2]; // BottomRight
        }
    }
}

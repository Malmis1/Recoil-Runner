using UnityEngine;
using UnityEngine.UI;

public class EyesController : MonoBehaviour
{
    [Tooltip("The eyes object that contains the Image or SpriteRenderer component")]
    [SerializeField] private Transform eyes;

    [Tooltip("Sprites for different eye directions")]
    [SerializeField] private Sprite[] eyeSprites;

    private Image eyesImage;
    private SpriteRenderer eyesSpriteRenderer;

    [Tooltip("Distance threshold to detect if cursor is on player")]
    [SerializeField] private float cursorThreshold = 0.5f; 

    [Tooltip("If the eyes are on the Player or not (then in UI)")]


    private void Awake() {
        if (eyes != null) {
            eyesImage = eyes.GetComponent<Image>();
            eyesSpriteRenderer = eyes.GetComponent<SpriteRenderer>();
        }
    }

    private void Update() {
        if (eyesImage != null || eyesSpriteRenderer != null) {
            UpdateEyesDirection();
        }
    }

    private void UpdateEyesDirection() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        Vector3 direction = mousePosition - transform.position;

        if (direction.magnitude <= cursorThreshold) {
            SetEyesSprite(8); // Middle
        } else {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            SetEyesSpriteByAngle(angle);
        }
    }

    private void SetEyesSpriteByAngle(float angle) {
        if (angle >= -22.5f && angle < 22.5f) {
            SetEyesSprite(4); // Right
        }
        else if (angle >= 22.5f && angle < 67.5f) {
            SetEyesSprite(7); // TopRight
        }
        else if (angle >= 67.5f && angle < 112.5f) {
            SetEyesSprite(5); // Top
        }
        else if (angle >= 112.5f && angle < 157.5f) {
            SetEyesSprite(6); // TopLeft
        }
        else if ((angle >= 157.5f && angle <= 180f) || (angle >= -180f && angle < -157.5f)) {
            SetEyesSprite(3); // Left
        }
        else if (angle >= -157.5f && angle < -112.5f) {
            SetEyesSprite(1); // BottomLeft
        }
        else if (angle >= -112.5f && angle < -67.5f) {
            SetEyesSprite(0); // Bottom
        }
        else if (angle >= -67.5f && angle < -22.5f) {
            SetEyesSprite(2); // BottomRight
        }
    }

    private void SetEyesSprite(int index) {
        if (eyeSprites != null && index >= 0 && index < eyeSprites.Length) {
            if (eyesImage != null) {
                eyesImage.sprite = eyeSprites[index];
            }
            else if (eyesSpriteRenderer != null) {
                eyesSpriteRenderer.sprite = eyeSprites[index];
            }
        }
    }
}

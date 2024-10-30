using UnityEngine;

public class PickupVisual : MonoBehaviour
{
    [Tooltip("Hover height and speed")]
    public float hoverHeight = 0.3f;
    public float hoverSpeed = 1f;

    [Tooltip("Optional: Glow object behind the pickup")]
    public SpriteRenderer glowSpriteRenderer;
    [Tooltip("Rotation speed for the glow effect")]
    public float glowRotationSpeed = 80f;
    [Tooltip("Scale speed for the glow effect")]
    public float glowScaleSpeed = 1.5f;
    [Tooltip("Minimum and maximum scale for the glow effect")]
    public float minGlowScale = 0.1f;
    public float maxGlowScale = 0.12f;

    private Vector3 initialPosition;
    private Vector3 initialScale;

    private void Start()
    {
        initialPosition = transform.position;
        
        if (glowSpriteRenderer != null)
        {
            initialScale = glowSpriteRenderer.transform.localScale;
        }
    }

    private void Update()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

        if (glowSpriteRenderer != null)
        {
            glowSpriteRenderer.transform.Rotate(Vector3.forward, glowRotationSpeed * Time.deltaTime);

            float scale = Mathf.Lerp(minGlowScale, maxGlowScale, (Mathf.Sin(Time.time * glowScaleSpeed) + 1) / 2);
            glowSpriteRenderer.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScrollBackground : MonoBehaviour
{
    [Header("Background Settings")]
    public float scrollSpeed = 1f;          // The speed at which the background scrolls
    public float resetPositionX = -10f;     // The X position at which the layer will reset
    public float startPositionX = 10f;      // The starting X position after reset

    private Transform[] backgroundLayers;   // Array to store background layers
    private float[] zOffsets;               // Array to store the initial Z offsets

    // Start is called before the first frame update
    void Start()
    {
        // Initialize background layers array and store child layers
        backgroundLayers = new Transform[transform.childCount];
        zOffsets = new float[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            backgroundLayers[i] = transform.GetChild(i);
            zOffsets[i] = backgroundLayers[i].position.z;  // Store initial Z positions
        }
    }

    // Update is called once per frame
    void Update()
    {
        ScrollBackground();
    }

    private void ScrollBackground()
    {
        // Loop through each background layer
        for (int i = 0; i < backgroundLayers.Length; i++)
        {
            // Move the layer left based on scroll speed and layer's Z offset
            float scrollModifier = 1 / (1 + Mathf.Abs(zOffsets[i])); // Slow down layers further away
            backgroundLayers[i].Translate(Vector3.left * scrollSpeed * scrollModifier * Time.deltaTime);

            // If the layer moves past the reset position, reset its X position
            if (backgroundLayers[i].position.x <= resetPositionX)
            {
                Vector3 newPos = backgroundLayers[i].position;
                newPos.x = startPositionX;
                backgroundLayers[i].position = newPos;
            }
        }
    }
}

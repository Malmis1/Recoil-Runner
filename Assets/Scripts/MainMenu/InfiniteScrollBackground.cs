using UnityEngine;

public class InfiniteScrollBackground : MonoBehaviour
{
    public float scrollSpeed = 0.02f; 
    private Material[] materials; 
    private float[] backSpeeds;

    private void Start()
    {
        int childCount = transform.childCount;
        materials = new Material[childCount];
        backSpeeds = new float[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Renderer renderer = transform.GetChild(i).GetComponent<Renderer>();
            materials[i] = renderer.material;

            float zPosition = transform.GetChild(i).position.z;
            backSpeeds[i] = 1 - (zPosition / 10f); 
        }
    }

    private void Update()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            float offsetX = Time.time * scrollSpeed * backSpeeds[i]; 
            materials[i].SetTextureOffset("_MainTex", new Vector2(offsetX, 0)); 
        }
    }
}

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("The player object that the camera will follow.")]
    [SerializeField] private Transform player; 
    
    private float minX;
    private float maxX;
    
    private Vector3 offset;  // Offset between the camera and the player.

    public float MinX
    {
        get => minX;
        set => minX = value;
    }

    public float MaxX
    {
        get => maxX;
        set => maxX = value;
    }

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        Vector3 newPos = player.position + offset;
        
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("The player object that the camera will follow.")]
    [SerializeField] private Transform player; 
    
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    
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

    public float MinY
    {
        get => minY;
        set => minY = value;
    }

    public float MaxY
    {
        get => maxY;
        set => maxY = value;
    }

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        Vector3 newPos = player.position + offset;
        
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}

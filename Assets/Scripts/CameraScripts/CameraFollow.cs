using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("The player object that the camera will follow.")]
    [SerializeField] private Transform player; 
    
    [Tooltip("Minimum X value for camera position. The camera won't move further left than this.")]
    [SerializeField] private float minX;  
    
    [Tooltip("Maximum X value for camera position. The camera won't move further right than this.")]
    [SerializeField] private float maxX;  
    
    private Vector3 offset;  // Offset between the camera and the player.

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

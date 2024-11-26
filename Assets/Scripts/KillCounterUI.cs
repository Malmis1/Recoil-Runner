using UnityEngine;
using TMPro;

public class KillCounterUI : MonoBehaviour 
{
    void Start()
    {
        GameManager.Instance.killCountText = GetComponent<TextMeshProUGUI>();
    }
}
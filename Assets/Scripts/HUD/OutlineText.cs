using TMPro;
using UnityEngine;

public class OutlineText : MonoBehaviour
{
    public TextMeshProUGUI tmpText;

    void Start()
    {
        tmpText.outlineWidth = 0.2f;

        tmpText.outlineColor = Color.black;
    }
}

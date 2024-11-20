using UnityEditor;
using UnityEngine;

public class PlayerPrefsUtility : Editor
{
    [MenuItem("Tools/Reset PlayerPrefs")]
    public static void ResetPlayerPrefs()
    {
        if (EditorUtility.DisplayDialog(
            "Reset PlayerPrefs",
            "Are you sure you want to reset all PlayerPrefs data?",
            "Yes",
            "No"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs has been reset.");
        }
    }
}

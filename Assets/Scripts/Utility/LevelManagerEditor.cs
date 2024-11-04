#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelManager levelManager = (LevelManager)target;

        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "gunName");

        if (levelManager.startWithGun)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                WeaponController weaponController = player.GetComponent<WeaponController>();
                if (weaponController != null && weaponController.gunDataList.Length > 0)
                {
                    string[] gunNames = new string[weaponController.gunDataList.Length];
                    for (int i = 0; i < gunNames.Length; i++)
                    {
                        gunNames[i] = weaponController.gunDataList[i].name;
                    }

                    int selectedIndex = System.Array.IndexOf(gunNames, levelManager.gunName);
                    if (selectedIndex == -1) selectedIndex = 0;

                    int newSelectedIndex = EditorGUILayout.Popup("Starting Gun", selectedIndex, gunNames);
                    if (newSelectedIndex != selectedIndex)
                    {
                        levelManager.gunName = gunNames[newSelectedIndex];
                        EditorUtility.SetDirty(levelManager);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("No guns found in WeaponController's gunDataList.", MessageType.Warning);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Player GameObject with tag 'Player' not found.", MessageType.Warning);
            }
        }

        // Apply changes to the serialized property
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
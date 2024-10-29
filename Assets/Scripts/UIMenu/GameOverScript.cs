using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour {
    [Tooltip("The game-over menu object")]
    public GameObject GOMenu;
    
    public void PauseAndShowGOMenu() {
        Time.timeScale = 0;
        GOMenu.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    bool restart;
    void Awake() {
        restart = false;
    }

    void Update() {
        if (Input.GetButtonDown("Restart") && !restart /*&& !this.GetComponent<PlayerMovementBruce>().playerInvincible*/) {
            restart = true;  
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
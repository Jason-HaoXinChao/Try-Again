using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{
    [SerializeField] string sceneName;

    void Update() {
        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}

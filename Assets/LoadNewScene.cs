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
            Destroy(GameObject.Find("GameManager"));
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}

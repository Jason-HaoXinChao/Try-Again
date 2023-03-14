using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneScript : MonoBehaviour
{
    private GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    void OnTriggerEnter()
    {
        SceneManager.LoadScene("Level Complete");
        gameManager.GetComponent<GameManager>().currentLevel++;
    }
}

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{   
    private GameObject gameManager;
    private string[] ratings = {"Does Not Meet", "Meets Some", "Meets Most", "Meets All", "Exceeds", "Greatly Exceeds", "Redefines"};
    void Start()
    {
    }

    void Awake()
    {   
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        gameManager = GameObject.Find("GameManager");
        
        if (sceneName == "Level Select") {
            GameObject levelTitle = GameObject.Find("Canvas/Level Details/Level Title");
            // GameObject test = GameObject.Find("Canvas/Level Details/Death Count");
            // Debug.Log(gameManager);
            GameObject.Find("Canvas/Level Details/Death Count").GetComponent<TMP_Text>().text = "Death Count: " + gameManager.GetComponent<GameManager>().deathCount;

            // Debug.Log(test.GetComponent<TMP_Text>().text);

            if (gameManager.GetComponent<GameManager>().currentLevel == 0){
                levelTitle.GetComponent<TMP_Text>().text = "Tutorial";
            } else if (gameManager.GetComponent<GameManager>().currentLevel == 1) {
                levelTitle.GetComponent<TMP_Text>().text = "Level 1";
                GameObject.Find("Canvas/Level/Level 1").SetActive(true);
                GameObject.Find("Canvas/Level/Tutorial").SetActive(false);
            } else if (gameManager.GetComponent<GameManager>().currentLevel == 2) {
                levelTitle.GetComponent<TMP_Text>().text = "Level 2";
                GameObject.Find("Canvas/Level/Level 2").SetActive(true);
                GameObject.Find("Canvas/Level/Level 1").SetActive(false);
            } else if (gameManager.GetComponent<GameManager>().currentLevel == 3) {
                levelTitle.GetComponent<TMP_Text>().text = "Level 3";
                GameObject.Find("Canvas/Level/Level 2").SetActive(true);
                GameObject.Find("Canvas/Level/Level 1").SetActive(false);
            } 
        } else if (sceneName == "Level Complete") {
            int deathCount = gameManager.GetComponent<GameManager>().currDeathCount;
            GameObject.Find("Canvas/Details/Death Count").GetComponent<TMP_Text>().text = "Interns Fired: " + deathCount;
            
            int rating = Math.Max(6 - deathCount / 10, 0);
            GameObject.Find("Canvas/Details/Rating").GetComponent<TMP_Text>().text = ratings[rating] + " Expectations";
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("Level Select");
        gameManager.GetComponent<GameManager>().currDeathCount = 0;
    }

    // Start is called before the first frame update
    public void StartLevel()
    {
        if (gameManager.GetComponent<GameManager>().currentLevel == 0){
            SceneManager.LoadScene("Tutorial");
        } else if (gameManager.GetComponent<GameManager>().currentLevel == 1) {
            SceneManager.LoadScene("Level Showcase");
        } else if (gameManager.GetComponent<GameManager>().currentLevel == 2) {
            SceneManager.LoadScene("Level Showcase");
        } else if (gameManager.GetComponent<GameManager>().currentLevel == 3) {
            SceneManager.LoadScene("Level Showcase");
        } 

    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }
}

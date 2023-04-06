#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Random=UnityEngine.Random;

public class MenuManager : MonoBehaviour
{   
    private GameObject gameManager;
    [SerializeField] private GameObject corpse;
    public int[] minDeaths = {5, 10, 15, 25};
    public int ratingIncrement = 5;
    [SerializeField] GameObject[] instructions;
    private int instructionsIdx = 0;
    private string[] ratings = {"Does Not Meet", "Meets Some", "Meets Most", "Meets All", "Exceeds", "Greatly Exceeds", "Redefines"};
    void Start()
    {
    }

    void Awake()
    {   
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        gameManager = GameObject.Find("GameManager");
        
        if (sceneName == "Level Complete") {
            int deathCount = gameManager.GetComponent<GameManager>().currDeathCount;
            GameObject.Find("Canvas/Details/Death Count").GetComponent<TMP_Text>().text = "Interns Fired: " + deathCount;

            int minDeathCount = minDeaths[gameManager.GetComponent<GameManager>().currentLevel];
            int rating = Math.Max(6 - (deathCount - minDeathCount)/ ratingIncrement, 0);

            GameObject.Find("Canvas/Details/Rating").GetComponent<TMP_Text>().text = ratings[rating] + " Expectations";
            StartCoroutine(SpawnCorpse(deathCount));
        }
    }

    void Update() {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Level Select") {
            bool up = true;

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                gameManager.GetComponent<GameManager>().currentLevel = Math.Max(gameManager.GetComponent<GameManager>().currentLevel - 1, 0);
                up = false;
                // Debug.Log(up);
            } else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                gameManager.GetComponent<GameManager>().currentLevel = Math.Min(gameManager.GetComponent<GameManager>().currentLevel + 1, 3);
                up = true;
                // Debug.Log(up);
            }

            GameObject levelTitle = GameObject.Find("Canvas/Level Details/Level Title");
            // GameObject test = GameObject.Find("Canvas/Level Details/Death Count");
            // Debug.Log(gameManager);
            GameObject.Find("Canvas/Death Count").GetComponent<TMP_Text>().text = "Death Count: " + gameManager.GetComponent<GameManager>().deathCount;

            // Debug.Log(test.GetComponent<TMP_Text>().text);

            if (gameManager.GetComponent<GameManager>().currentLevel == 0){
                levelTitle.GetComponent<TMP_Text>().text = "Tutorial: First Day";
                GameObject.Find("Canvas/Level/Level 3").SetActive(false);
                GameObject.Find("Canvas/Level/Level 2").SetActive(false);
                GameObject.Find("Canvas/Level/Level 1").SetActive(false);
                GameObject.Find("Canvas/Level/Tutorial").SetActive(true);

                // if (up) {
                //     GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 5);
                // } else {
                //     GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 5);
                //     GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 5);
                // }
                if (up) {
                    GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                } else {
                    GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                }
            } else if (gameManager.GetComponent<GameManager>().currentLevel == 1) {
                levelTitle.GetComponent<TMP_Text>().text = "Level 1: Errand Boy";
                GameObject.Find("Canvas/Level/Level 3").SetActive(false);
                GameObject.Find("Canvas/Level/Level 2").SetActive(false);
                GameObject.Find("Canvas/Level/Level 1").SetActive(true);
                GameObject.Find("Canvas/Level/Tutorial").SetActive(false);

                if (up) {
                    GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 226f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 226f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 226f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 226f, 0f), 10);
                } else {
                    GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 559f, 0f), 10);
                }
            } else if (gameManager.GetComponent<GameManager>().currentLevel == 2) {
                levelTitle.GetComponent<TMP_Text>().text = "Level 2: Oh the Monotony";
                GameObject.Find("Canvas/Level/Level 3").SetActive(false);
                GameObject.Find("Canvas/Level/Level 2").SetActive(true);
                GameObject.Find("Canvas/Level/Level 1").SetActive(false);
                GameObject.Find("Canvas/Level/Tutorial").SetActive(false);

                if (up) {
                    GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -137f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -137f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -137f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -137f, 0f), 10);
                } else {
                    GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 226f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 226f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 226f, 0f), 10);
                    GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 226f, 0f), 10);
                }
                // if (up) {
                //     GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -137f, 0f), 5);
                // } else {
                //     GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -137f, 0f), 2);
                //     GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, 226f, 0f), 5);
                // }
            } else if (gameManager.GetComponent<GameManager>().currentLevel == 3) {
                levelTitle.GetComponent<TMP_Text>().text = "Level 3: Downsize";
                GameObject.Find("Canvas/Level/Level 3").SetActive(true);
                GameObject.Find("Canvas/Level/Level 2").SetActive(false);
                GameObject.Find("Canvas/Level/Level 1").SetActive(false);
                GameObject.Find("Canvas/Level/Tutorial").SetActive(false);

                if (up) {
                    GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -200f, 0f), 5);
                    GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -200f, 0f), 5);
                    GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -200f, 0f), 5);
                    GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -200f, 0f), 5);
                } else {
                    GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Tutorial").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -137f, 0f), 5);
                    GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 1").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -137f, 0f), 5);
                    GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 2").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -137f, 0f), 5);
                    GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -137f, 0f), 5);
                }
                // if (up) {
                //     GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -200f, 0f), 2);
                // } else {
                //     GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GameObject.Find("Canvas/Level/Level 3").GetComponent<RectTransform>().localPosition, new Vector3(438.73f, -137f, 0f), 2);
                // }
            } 
        } else if (sceneName == "Onboarding") {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                instructions[instructionsIdx].SetActive(false);
                if (instructionsIdx == 0) {
                    instructionsIdx = 4;
                } else {
                    instructionsIdx -= 1;
                }

                instructions[instructionsIdx].SetActive(true);
            } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                instructions[instructionsIdx].SetActive(false);
                instructionsIdx = (instructionsIdx + 1) % 5;
                instructions[instructionsIdx].SetActive(true);
            }
        }
    }

    IEnumerator SpawnCorpse(int deathCount)
    {
        int bodyCount = 0;
        while (bodyCount < deathCount)
        {
            //Debug.Log("waiting");
            yield return new WaitForSeconds(0.1f);
            //Debug.Log("done");
            float x = Random.Range(-7.0f,7.0f);
            Vector3 position = new Vector3(x, 6, 9);
            Quaternion rotation = Random.rotation;
            Instantiate(corpse, position, rotation);
            bodyCount++;
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

    public void GoToOnboarding()
    {
        SceneManager.LoadScene("Onboarding");
    }

    public void StartLevel()
    {
        if (gameManager.GetComponent<GameManager>().currentLevel == 0){
            SceneManager.LoadScene("Tutorial");
        } else if (gameManager.GetComponent<GameManager>().currentLevel == 1) {
            SceneManager.LoadScene("Main Game - Level 1");
        } else if (gameManager.GetComponent<GameManager>().currentLevel == 2) {
            SceneManager.LoadScene("Level Showcase");
        } else if (gameManager.GetComponent<GameManager>().currentLevel == 3) {
            SceneManager.LoadScene("Main Game - Level 3");
        } else if (gameManager.GetComponent<GameManager>().currentLevel == 4) {
            SceneManager.LoadScene("Alpha - End Screen");
        } 

    }

    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeLevel() {
        this.gameObject.SetActive(false);
    }
}

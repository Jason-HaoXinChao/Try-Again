using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!pauseMenu.activeSelf) {
                pauseMenu.SetActive(true);
            } else {
                pauseMenu.SetActive(false);
            }
        }
    }


}

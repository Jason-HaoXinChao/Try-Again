using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private bool pauseMenuOpen;
    public GameObject pauseMenu;

    void Start()
    {
        pauseMenuOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!pauseMenuOpen) {
                pauseMenu.SetActive(true);
                pauseMenuOpen = true;
            } else {
                pauseMenu.SetActive(false);
                pauseMenuOpen = false;
            }
        }
    }
}

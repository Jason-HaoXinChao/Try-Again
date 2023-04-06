using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlackoutMenu : MonoBehaviour
{
    public GameObject firstSelected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable() {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}

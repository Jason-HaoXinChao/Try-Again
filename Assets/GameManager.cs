using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public int deathCount;
    public int currentLevel;
    public int currDeathCount;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0;
        deathCount = 0;
        currDeathCount = 0;
    }

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

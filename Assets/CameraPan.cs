using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraPan : MonoBehaviour
{
    [SerializeField] Vector3 targetPosition;
    [SerializeField] float speed;
    [SerializeField] GameObject cont;
    bool once;

    void Start()
    {
        once = true;
    }

    void Update()
    {
        transform.localPosition 
            = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * speed);

        if(once && transform.localPosition.y < 6)
        {
            cont.SetActive(true);
            once = false;
        }

        if(Input.GetButtonDown("Confirm") && transform.localPosition.y < 44)
        {
            SceneManager.LoadScene("Tutorial");
        }

        if(!once && Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}

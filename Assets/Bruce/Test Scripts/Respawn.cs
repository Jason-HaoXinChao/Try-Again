using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject playerObject;

    public void RespawnCall()
    {
        playerObject.GetComponent<Transform>().position = this.GetComponent<Transform>().position;
        playerObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource respawnSound;
    [SerializeField] private AudioSource impaleSound;
    [SerializeField] private AudioSource deathSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void respawn()
    {
        respawnSound.Play();
    }

    public void impale()
    {
        impaleSound.Play();
    }

    public void die()
    {
        deathSound.Play();
    }

}

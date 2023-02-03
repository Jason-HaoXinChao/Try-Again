using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource respawnSound;
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

    public IEnumerator playRespawnSound()
    {
        respawnSound.Play();
        yield return new WaitForSeconds(1);
    }
}

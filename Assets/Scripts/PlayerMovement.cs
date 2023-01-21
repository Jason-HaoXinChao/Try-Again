using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveForce = 10f;
    private float jumpForce = 10f;
    private float moveX;
    // private RigidBody2D playerBody;  
    

    private void Awake() {
        // playerBody = GetComponent<RigidBody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // playerBody = GetComponent<RigidBody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveKeyboard();
        
    }

    void playerMoveKeyboard() {
        moveX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(moveX, 0f, 0f) * Time.deltaTime * moveForce;
    }
}

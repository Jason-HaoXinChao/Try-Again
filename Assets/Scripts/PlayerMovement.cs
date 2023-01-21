using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveForce = 10f;
    private float jumpForce = 10f;
    private float moveX;
    private Rigidbody2D playerBody;  
    

    private void Awake() {
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveKeyboard();
        PlayerJump();
        
    }

    private void FixedUpdate() {
        // PlayerJump();
    }

    void playerMoveKeyboard() {
        moveX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(moveX, 0f, 0f) * Time.deltaTime * moveForce;
    }

    void PlayerJump() {
        if (Input.GetButtonDown("Jump")) {
            Debug.Log("Jump pressed");
            playerBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
}

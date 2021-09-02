using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private Transform mainCamera;
    public bool can_move = true;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        if(can_move){
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            move = mainCamera.forward * move.z + mainCamera.right *move.x;
            move.y = 0;
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            // Changes the height position of the player..
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }

    public void Lock_Controls(){
        can_move = false;
    }
    public void UnLock_Controls(){
        can_move = true;
    }

    public void Disable_Collison(){
        controller.detectCollisions = false;
    }
    public void Enable_Collision(){
        controller.detectCollisions = true;
    }
    public void Remove_Gravity(){
        gravityValue = 0;
    }
    public void Restore_Gravity(){
        gravityValue = -9.8f;
    }
}

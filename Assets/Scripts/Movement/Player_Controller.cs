using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;
    public Rigidbody rb;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private Transform mainCamera;
    public bool can_move = true;
    public bool initialized;
    public Animator animator;
    public bool b_anim_idle;
    public bool b_anim_walk;
    public Transform camera_transform;
    public Vector3 cur_dir;
    public billboarder billboarder;
    public Vector3 last_grounded_loc;

    private void Start()
    {
        camera_transform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        controller = gameObject.GetComponent<CharacterController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Game_Manager.Instance.player = this;
    }

    public Vector3 GetLastGroundedLoc(){
        return last_grounded_loc;
    }


    void Update()
    {
        inputs input = Game_Manager.Instance.Get_Inputs();
        groundedPlayer = controller.isGrounded;
        if(groundedPlayer){
            last_grounded_loc = transform.position;
        }
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector3 move = new Vector3(0f, 0f, 0f);
        if(can_move){
            move = new Vector3(input.XAxis , 0, input.YAxis);
        }
        move = mainCamera.forward * move.z + mainCamera.right *move.x;
        //quantize move.y into movez and movex, as it'll create downward force on overhead cams

        move.z -= move.y;
        cur_dir = move;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        b_anim_idle = true;
        b_anim_walk = false;
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
            b_anim_walk = true;
            b_anim_idle = false;
        }

        // Changes the height position of the player..
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        Animation_Control();
    }

    public void Lock_Controls(){
        can_move = false;
    }
    public void UnLock_Controls(){
        can_move = true;
    }
    public void SetPos(Vector3 pos){
        Debug.Log("Setting Player Pos");
        rb.MovePosition(pos);
        Physics.SyncTransforms();
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

    public void Animation_Control(){
        Debug.DrawRay(transform.position,transform.forward);
        Vector3 dir = transform.forward;

        //this.billboarder.billboardY = false;
        this.billboarder.isAbove = false;
        if(b_anim_idle){
            if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                animator.Play("idle_t");
                this.billboarder.isAbove = true;
                //this.billboarder.billboardY = true;
            }
            else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                animator.Play("idle_L");
            }
            else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                animator.Play("idle_R");
            }
            else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                animator.Play("idle_b");
            }
            else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                animator.Play("idle_f");
            }  
        }
        if(b_anim_walk){
            if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                animator.Play("walk_t");
                this.billboarder.isAbove = true;
                //this.billboarder.billboardY = true;
            }
            else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                animator.Play("walk_L");
            }
            else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                animator.Play("walk_R");
            }
            else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                animator.Play("walk_b");
            }
            else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                animator.Play("walk_f");
            }
        }
    }
}

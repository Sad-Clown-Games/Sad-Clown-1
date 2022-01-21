using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Rock_Hopper_Gate : MonoBehaviour
{
    // Start is called before the first frame update


    public Rock_Hopper_Platform platform; //go to this platorm.
    public bool can_hop;
    public bool is_hopping;
    public bool start_hopping;
    public bool reverse_hop;
    public float jump_speed = 1;
    public Player_Controller player;
    public Rock_Hopper_Controller controller;
    public CinemachineDollyCart jumpCart;
    public CinemachineSmoothPath jumpTrack;
    public Transform real_pos;
    public float player_height;
    public List<Vector3> original_path;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            can_hop = true;
            player = other.GetComponent<Player_Controller>();
            player_height = 0.42f;
            controller.Set_Cur_Gate(this);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            can_hop = false;
            //player = null;
            if(!is_hopping){
                Reset_Path();
            }
        }
    }


    void Start()
    {
        foreach(CinemachineSmoothPath.Waypoint w in jumpTrack.m_Waypoints){
            original_path.Add(w.position);
        }
    }

    //This logic should start and stop the cart properly.
    void Update()
    {
        if(can_hop && player != null && !is_hopping){
            Vector3 player_ref_pos =  player.transform.position - real_pos.position ;
            CinemachineSmoothPath.Waypoint start = jumpTrack.m_Waypoints[0];
            CinemachineSmoothPath.Waypoint mid = jumpTrack.m_Waypoints[1];
            CinemachineSmoothPath.Waypoint end = jumpTrack.m_Waypoints[2];
            float posY;

            if(Vector3.Distance(player_ref_pos,start.position) < Vector3.Distance(player_ref_pos,end.position)){
                //move start to player
                posY = start.position.y;
                jumpTrack.m_Waypoints[0].position = player_ref_pos;
                jumpTrack.m_Waypoints[0].position.y = player_height;
                reverse_hop = false;
            }
            else{
                posY = end.position.y;
                jumpTrack.m_Waypoints[2].position = player_ref_pos;
                jumpTrack.m_Waypoints[2].position.y = player_height;
                reverse_hop = true; //we know we on the other side
            }
            //update midpoint to match logically;
            posY = mid.position.y;
            jumpTrack.m_Waypoints[1].position = Vector3.Lerp(start.position,end.position,0.5f);
            jumpTrack.m_Waypoints[1].position.y = posY;
            jumpTrack.InvalidateDistanceCache();
        } 
        if(start_hopping){
            jumpTrack.InvalidateDistanceCache();
            jumpCart.m_Position = 0;
            jumpCart.m_Speed = jump_speed;
            if(reverse_hop){
                jumpCart.m_Speed = -jump_speed;
                jumpCart.m_Position = (float)jumpTrack.PathLength;
            }
            controller.Lock_Controls();
            player.Lock_Controls();
            player.Disable_Collison();
            player.Remove_Gravity();
            start_hopping = false;
            is_hopping = true;
        }
        if(is_hopping){
            player.transform.position = jumpCart.transform.position; //set player position position
            if(reverse_hop){
                if(jumpCart.m_Position <= 0){
                    is_hopping = false;                    
                    controller.UnLock_Controls();
                    player.UnLock_Controls();
                    player.Enable_Collision();
                    player.Restore_Gravity();
                    Reset_Path();
                }
            }
            else{
                if(jumpCart.m_Position >= jumpTrack.PathLength){
                    is_hopping = false;
                    controller.UnLock_Controls();
                    player.UnLock_Controls();
                    player.Enable_Collision();
                    player.Restore_Gravity();
                    Reset_Path();
                }
            }
        }
    }

    private void Reset_Path(){
        for(int i = 0; i < original_path.Count;i++){
                jumpTrack.m_Waypoints[i].position = original_path[i]; //dumb way to do this but whatever its not slow 
            }
        jumpTrack.InvalidateDistanceCache();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Hopper_Controller : MonoBehaviour
{
    // Start is called before the first frame update

    public Rock_Hopper_Platform cur_platform; 
    public Rock_Hopper_Gate cur_gate; 
    public bool lock_hopping;

    // Update is called once per frame
    void Update()
    {
        //controller for how the player jumps
        if(cur_gate && cur_gate.can_hop && Input.GetButtonDown("Select")){
            cur_gate.start_hopping = true;
        }
    }

    public void Set_Cur_Gate(Rock_Hopper_Gate gate){
        if(!lock_hopping)
            cur_gate = gate;
    }

    public void Set_Cur_Platform(Rock_Hopper_Platform platform){
        if(!lock_hopping)
            cur_platform = platform;
    }
    public void Lock_Controls(){
        lock_hopping = true;
    }
    public void UnLock_Controls(){
        lock_hopping = false;
    }
}

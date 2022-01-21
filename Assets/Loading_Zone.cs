using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_Zone : MonoBehaviour
{

    [SerializeField]
    private Vector3 new_location;
    [SerializeField]
    private string new_scene_name;

    private void Activate(){
         Game_Manager.Instance.Move_To_New_Map(new_scene_name,new_location);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            Activate();
        }
    }
}

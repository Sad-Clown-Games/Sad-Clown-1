using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


[RequireComponent(typeof(Collider))]
public class CameraAngleChanger : MonoBehaviour
{

    public CinemachineVirtualCamera assigned_camera;

    public void PrioritizeCamera(){
        Game_Manager.Instance.camera_manager.GetCurrentCamera().Priority = 0;
        assigned_camera.Priority = 99;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            PrioritizeCamera();
        }
    }

}

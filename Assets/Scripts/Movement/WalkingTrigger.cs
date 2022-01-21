using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WalkingTrigger : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera cameraOn;
    public Cinemachine.CinemachineVirtualCamera cameraOff;
    //Upon collision with another GameObject
    private void OnTriggerEnter(Collider other)
    {
        cameraOff.Priority = 0;
        cameraOn.Priority = 99;
    }
}

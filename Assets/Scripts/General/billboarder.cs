using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class billboarder : MonoBehaviour
{
    // Start is called before the first frame update

    public bool billboardX = true;
    public bool billboardY = false;
    public bool billboardZ = true;

    public GameObject mainCamera;

    void Start(){
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newLook = mainCamera.transform.position;
        if(!billboardX){
            newLook.x = transform.position.x;
        }
        if(!billboardY){
            newLook.y = transform.position.y;
        }
        if(!billboardZ){
            newLook.y = transform.position.z;
        }
        transform.LookAt(newLook, Vector3.up);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public CinemachineBrain brain;
    public Camera main_camera;
    public float timer;
    public bool zooming;
    public bool zooming_in;
    public float zoom_speed;
    public float zoom_amount;
    public float defaultFOV;



    private void Start() {
        Get_Camera_Reference();
        main_camera.fieldOfView = defaultFOV;
    }

    private void Get_Camera_Reference(){
        main_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        brain = main_camera.GetComponent<CinemachineBrain>();    
    }

    private void Update(){
        if(main_camera == null || brain ==  null)
            Get_Camera_Reference();
        CinemachineVirtualCamera current_cam = null;
        try{
            current_cam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        }
        catch{
            Debug.Log("Cannot find current virtual cam");
        }

        if(zooming && current_cam != null){
            ZoomCamera(current_cam);
        }

    }

    public void StartZoomCamera(float speed, float amount){
        zoom_speed = speed;
        zoom_amount = amount;
        CinemachineVirtualCamera cm = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        float dir = cm.m_Lens.FieldOfView - amount;
        if(amount > 0){ //we decrease
            zoom_speed *= -1;
            zooming_in = true;
        }
        zooming = true;

    }

    public CinemachineVirtualCamera GetCurrentCamera(){
        return brain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }

    //safety check at 0 and 180 to prevent fuckery
    private void ZoomCamera(CinemachineVirtualCamera c){
        float step = zoom_speed * Time.deltaTime;
        c.m_Lens.FieldOfView += step;
        if(c.m_Lens.FieldOfView <= 0 || c.m_Lens.FieldOfView >= 180){
            zooming = false;
        }
        if(zooming_in && c.m_Lens.FieldOfView <= zoom_amount){
            zooming = false;
        }
        //else if(c.m_Lens.FieldOfView >= zoom_amount){
        //    zooming = false;
       // }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InSceneTransition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Vector3 telecoords;
    [SerializeField]
    private CinemachineVirtualCamera new_cam;

    [SerializeField]
    private float fade_time = 2f;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            StartCoroutine(TeleportSequence());
        }
    }


    IEnumerator TeleportSequence(){
        Game_Manager.Instance.Disable_Input_Control();
        CrossFader fader = GameObject.FindGameObjectWithTag("CrossFadeUI").GetComponent<CrossFader>();
        fader.CrossFade_Out();
        yield return new WaitForSeconds(1.5f);
        new_cam.Priority = 99;
        Game_Manager.Instance.camera_manager.GetCurrentCamera().Priority = 0;
        Game_Manager.Instance.TeleportPlayer(telecoords);
        yield return new WaitForSeconds(.5f);
        Game_Manager.Instance.Enable_Input_Control();
        fader.CrossFade_In();
    }
}

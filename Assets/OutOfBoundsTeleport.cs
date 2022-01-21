using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsTeleport : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            StartCoroutine(TeleportSequence());
        }
    }

     IEnumerator TeleportSequence(){
        Game_Manager.Instance.Disable_Input_Control();
        CrossFader fader = GameObject.FindGameObjectWithTag("CrossFadeUI").GetComponent<CrossFader>();
        fader.CrossFade_Out();
        yield return new WaitForSeconds(1.5f);
        Game_Manager.Instance.TeleportPlayerLastGrounded();
        yield return new WaitForSeconds(.5f);
        Game_Manager.Instance.Enable_Input_Control();
        fader.CrossFade_In();
    }
}

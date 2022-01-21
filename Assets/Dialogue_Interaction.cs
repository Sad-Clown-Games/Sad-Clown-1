using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParadoxNotion;
using NodeCanvas.DialogueTrees;

public class Dialogue_Interaction : MonoBehaviour
{
    // Start is called before the first frame update


    private bool can_activate;
    [SerializeField]
    private GameObject action_indicator;
    private bool start_playing;
    public DialogueTreeController dtController;
    private Player_Controller player;
    

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            action_indicator.SetActive(true);
            can_activate = true;
            player = other.gameObject.GetComponent<Player_Controller>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            action_indicator.SetActive(false);
            can_activate = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
                //controller for how the player jumps
        
        if(!start_playing && can_activate && Input.GetButtonDown("Select")){
            Input.ResetInputAxes(); // 
            player.Lock_Controls();
            dtController.StartDialogue();
            start_playing = true;
        }
        if(!dtController.isRunning && start_playing){
            //if we're not running anymore
            player.UnLock_Controls();
            start_playing = false;
        }
    }
}

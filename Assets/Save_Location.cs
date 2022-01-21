using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.DialogueTrees;

public class Save_Location : MonoBehaviour
{

    private bool can_activate;
    [SerializeField]
    private bool start_playing;
    public DialogueTreeController dtController;
    private Player_Controller player;
    [SerializeField]
    private SpriteRenderer shader;
    public bool doSave;
    
     private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            shader.material.SetFloat("_OutlineStrength",3.5f);
            can_activate = true;
            player = other.gameObject.GetComponent<Player_Controller>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            shader.material.SetFloat("_OutlineStrength",1f);
            can_activate = false;
        }
    }
    void Update()
    {
        
        if(!start_playing && can_activate && Input.GetButtonDown("Select")){
            Input.ResetInputAxes(); // 
            player.Lock_Controls();
            dtController.StartDialogue();
            //Game_Manager.Instance.Save_Game(); //save game when you start the save dialogue
            start_playing = true;
        }
        if(!dtController.isRunning && start_playing){
            //if we're not running anymore
            player.UnLock_Controls();
            start_playing = false;
        }
    }
    protected void Save(){
        Game_Manager.Instance.Save_Game();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private bool hit_player;
    private Player_Controller player;
    public string battle_scene_name = "Battle_Scene";
    public float transition_time = 1.5f;
    public OverworldEnemy_Vision vision;
    public float speed;
    private bool chasing_player;
    private bool started_battle;
    public float max_distance = 10f;
    public float zoom_amount = 30f;
    public float zoom_speed= 30f;
    private Rigidbody rb;
    private Animator transition;
    [SerializeField]
    private bool debug_ai_active;

    void Start(){
        rb = GetComponent<Rigidbody>();
        transition = GameObject.FindGameObjectWithTag("CrossFadeUI").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!debug_ai_active){
            if(vision.sees_player && !chasing_player){
                Start_Chasing();
            }
            if(chasing_player){
                Chase_Player();
            }
            if(hit_player && !started_battle){
                Start_Battle();
            }
            if(!chasing_player){
                Patrol();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            player = other.transform.GetComponent<Player_Controller>();
            hit_player = true;
        }
    }

    private void Start_Chasing(){
        chasing_player = true;
    }

    private void Chase_Player(){
        Vector3 dir = (vision.player.transform.position - transform.position).normalized;
        //just move towards player
        rb.MovePosition(transform.position + (dir * speed * Time.deltaTime));

        //check if player is too far away
        if(vision.Distance_To_Player() > max_distance){
            chasing_player = false;
        }

    }

    private void Patrol(){

    }

    private void Start_Battle(){
        started_battle = true; //only do this once.
        //freeze player
        player.Lock_Controls();
        //
        //do animation transition
        Camera_Manager cm = Game_Manager.Instance.camera_manager;
        cm.StartZoomCamera(zoom_speed,zoom_amount);
        //
        //change scene
        StartCoroutine(SceneTransition());
        
    }

    IEnumerator SceneTransition(){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f); //why don't I do this more lol
        SceneTransferHelper.GoToBattleScene(battle_scene_name);
    }


}

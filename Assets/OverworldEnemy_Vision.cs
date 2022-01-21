using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldEnemy_Vision : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public bool sees_player;

    public float Distance_To_Player(){
        return Vector3.Distance(transform.position,player.transform.position);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            player = other.gameObject;
            sees_player = true;
        }

    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            sees_player = false;
        }

    }
}

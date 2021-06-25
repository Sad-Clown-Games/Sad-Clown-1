using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battle_Option_Skill : MonoBehaviour
{
    // Start is called before the first frame update

    Animator animator;
    public bool hovered;
    public GameObject left_adjacent;
    public GameObject right_adjacent;
    public GameObject up_adjacent;
    public GameObject down_adjacent;
    public Attack attack;
    public string description;
    public TMPro.TMP_Text text;
    public bool is_active;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        if(hovered)
            animator.Play("Hovered");
        else{
            animator.Play("Not Hovered");
        }

    }

    public void Activate(Attack attk){
        is_active = true;
        attack = attk;
        text.text = attack.attack_name;
        description = attack.description;
        this.gameObject.SetActive(true);
    }
    public void Deactivate(){
        text.text = "null";
        description = "null";
        attack = null;
        this.gameObject.SetActive(false);
    }
}

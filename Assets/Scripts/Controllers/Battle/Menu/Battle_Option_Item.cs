using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battle_Option_Item : MonoBehaviour
{
    // Start is called before the first frame update

    Animator animator;
    public bool hovered;
    public GameObject left_adjacent;
    public GameObject right_adjacent;
    public GameObject up_adjacent;
    public GameObject down_adjacent;
    public Item item;
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

    public void Activate(Item i){
        is_active = true;
        this.item = i;
        text.text = i.item_name;
        description = i.description;
        this.gameObject.SetActive(true);
    }
    public void Deactivate(){
        text.text = "null";
        description = "null";
        item = null;
        this.gameObject.SetActive(false);
    }
}

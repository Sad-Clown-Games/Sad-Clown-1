using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option_Attack : Battle_Option
{
    // Start is called before the first frame update

    //Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();        
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

    public override void Select_Option(){
        Debug.Log("Dummy Action");
    }
}

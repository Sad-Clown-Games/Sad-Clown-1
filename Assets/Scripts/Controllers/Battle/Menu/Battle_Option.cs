using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Option : MonoBehaviour
{
    // Start is called before the first frame update

    Animator animator;
    public bool hovered;
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
}

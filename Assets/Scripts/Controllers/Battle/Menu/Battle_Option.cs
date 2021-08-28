using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Option : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    public SpriteRenderer sprite;
    public bool hovered;
    public string option_name;
    public string option_description;
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

    virtual public void Select_Option(){
        Debug.Log("Dummy Action");
    }

    public void Gray_Out(){
        sprite.color = Color.gray;
    }
    public void UnGray(){
        sprite.color = Color.white;
    }
}

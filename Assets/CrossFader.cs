using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFader : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CrossFade_In(){
        animator.Play("CrossFade_In");
    }

    public void CrossFade_Out(){
        animator.Play("CrossFade_Out");
    }
}

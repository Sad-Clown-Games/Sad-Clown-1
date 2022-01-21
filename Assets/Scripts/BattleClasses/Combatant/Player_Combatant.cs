using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//User the player character class for any player specific stuff.
public class Player_Combatant : Combatant
{

    public float rattle_death_time = 0.35f;
    public float rattle_hurt_time = 0.35f;
    void Start()
    {
        camera_transform =  GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(pawn){
            Animation_Control();
        }
    }
    public bool is_being_switched;

    //move the animation logic to the child once we impliment individual character
    override public void Animation_Control(){
        this.billboarder.isAbove = false;
        CheckAnimation(b_anim_idle,"idle");
        CheckAnimation(b_anim_walk,"walk");
        CheckAnimation(b_anim_dying,"dying");
        CheckAnimation(is_dead,"dead");
        CheckAnimation(b_anim_casting,"casting");
        CheckAnimation(b_anim_attk,"attack");
        CheckAnimation(b_anim_hurt,"hurt");
    }
    public override void AnimAttack(bool trans = false)
    {
        b_anim_hurt = false;
        b_anim_idle = false;
        b_anim_walk = false;
        b_anim_attk =  true;
        b_anim_casting = false;
        b_anim_dying = false;
    }

    public override void AnimHurt()
    {
        b_anim_hurt = true;
        b_anim_idle = false;
        b_anim_walk = false;
        b_anim_attk = false;
        b_anim_casting = false;
        b_anim_dying = false;
        Invoke("AnimIdle",rattle_hurt_time); //move to idle
    }

    public override void AnimIdle()
    {
        b_anim_hurt = false;
        b_anim_idle = true;
        b_anim_walk = false;
        b_anim_attk =  false;
        b_anim_casting = false;
        b_anim_dying = false;
    }
    public override void AnimDying()
    {
        b_anim_hurt = false;
        b_anim_idle = false;
        b_anim_walk = false;
        b_anim_attk =  false;
        b_anim_casting = false;
        b_anim_dying = true;
        Invoke("AnimDead",rattle_death_time);   //move to dead
    }
    public override void AnimDead()
    {
        b_anim_hurt = false;
        b_anim_idle = false;
        b_anim_walk = false;
        b_anim_attk =  false;
        b_anim_casting = false;
        b_anim_dying = false;
        is_dead = true; //we do this since we need to animate dying before 
    }
    public override void AnimMove()
    {
        b_anim_hurt = false;
        b_anim_idle = false;
        b_anim_walk = true;
        b_anim_attk =  false;
        b_anim_casting = false;
        b_anim_dying = false;
    }

    public override void AnimCast(bool trans = false)
    {
        b_anim_hurt = false;
        b_anim_idle = false;
        b_anim_walk = false;
        b_anim_attk =  false;
        b_anim_casting = true;
        b_anim_dying = false;
    }

}

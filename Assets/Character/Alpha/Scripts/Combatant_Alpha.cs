using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant_Alpha : Player_Combatant
{
    override public void Animation_Control(){
        this.billboarder.isAbove = false;
        CheckAnimation(b_anim_idle,"idle");
        CheckAnimation(b_anim_walk,"walk");
        CheckAnimation(b_anim_dying,"dying");
        CheckAnimation(is_dead,"dead");
        CheckAnimation(b_anim_casting,"casting");
        CheckAnimation(b_anim_attk,"attack");
        CheckAnimation(b_anim_hurt,"hurt");
        if(clear_animation_bools)
            Clear_Animation_Bools(); //this should stop the animator from trying to play the same animation, and allow for transitions
    }

    public override void AnimAttack(bool trans = false)
    {
        b_anim_hurt = false;
        b_anim_idle = false;
        b_anim_walk = false;
        b_anim_attk =  true;
        b_anim_casting = false;
        b_anim_dying = false;
        if(trans){
            clear_animation_bools = true;
        }
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
    public override void AnimSpecial1(bool trans = false)
    {
        b_anim_spec1 = true;
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

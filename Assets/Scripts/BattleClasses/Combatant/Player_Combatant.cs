using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Parent class for party members, we need a child of this for each party member and enemy in the game
public class Player_Combatant : Combatant
{

    //equipment objects go here,
    //head
    //body
    //weapon
    
    public bool is_being_switched;

    //move the animation logic to the child once we impliment individual character
    override public void Animation_Control(){
        if(b_anim_idle){
            if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                animator.Play("idle_t");
            }
            else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                animator.Play("idle_L");
            }
            else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                animator.Play("idle_R");
            }
            else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                animator.Play("idle_b");
            }
            else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                animator.Play("idle_f");
            }  
        }
        if(b_anim_walk){
            if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                //animator.Play("walk_t");
                animator.Play("idle_t");
            }
            else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                //animator.Play("walk_L");
                animator.Play("idle_L");
            }
            else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                //animator.Play("walk_R");
                animator.Play("idle_R");
            }
            else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                //animator.Play("walk_b");
                animator.Play("idle_b");
            }
            else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                animator.Play("walk_f");
            }
        }
        
        if(b_anim_dying){
            if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                animator.Play("idle_t");
            }
            else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                animator.Play("dying_L");
            }
            else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                animator.Play("dying_R");
            }
            else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                animator.Play("idle_b");
            }
            else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                animator.Play("dying_f");
            }  
        }
        
        if(is_dead){
            if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                animator.Play("dead_t");
            }
            else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                animator.Play("dead_L");
            }
            else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                animator.Play("dead_R");
            }
            else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                animator.Play("dead_b");
            }
            else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                animator.Play("dead_f");
            }  
        }
        if(b_anim_casting){
            if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                animator.Play("casting");
            }
            else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                animator.Play("casting");
            }
            else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                animator.Play("casting");
            }
            else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                animator.Play("casting");
            }
            else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                animator.Play("casting");
            }  
        }
         if(b_anim_attk){
            if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                animator.Play("attack_f");
            }
            else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                animator.Play("attack_f");
            }
            else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                animator.Play("attack_f");
            }
            else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                animator.Play("attack_f");
            }
            else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                animator.Play("attack_f");
            }  
        }
        
        if(b_anim_hurt){
            if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                animator.Play("idle_t");
            }
            else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                animator.Play("damage_L");
            }
            else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                animator.Play("damage_r");
            }
            else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                animator.Play("damage_b");
            }
            else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                animator.Play("damage_f");
            }  
        }
    }
    override public void AnimAttack(){

    }
    override public void AnimHurt(){

    }
    override public void AnimDying(){

    }
    override public void AnimDead(){

    }
    override public void AnimMove(){

    }
    override public void AnimSpecial(){

    }
    override public void AnimIdle(){

    }
    override public void AnimCast(){

    }

}

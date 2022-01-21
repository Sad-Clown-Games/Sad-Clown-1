using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shroom: Enemy_Combatant
{
    // Start is called before the first frame update

    public List<Combatant> debug_party;
    public List<Combatant> debug_enemy;
    public float rattle_death_time = 0.35f;
    public float rattle_hurt_time = 0.35f;
    void Start()
    {
        camera_transform =  GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Animation_Control();
    }

    //
    override public CombatAction Run_AI(List<Combatant> player_party, List<Combatant> enemy_party ){
        CombatAction ca = new CombatAction();
        debug_enemy = enemy_party;
        debug_party = player_party;
        ca.targets = new List<Combatant>();
        ca.targets.Add(player_party[Random.Range(0,player_party.Count)]); //get random party member
        ca.action = attacks[0];
        ca.actor = this;
        ca.speed = stats.spd + ca.action.speed;
        //ca.action.is_flipped = true;
        return ca;
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



    override public void Animation_Control(){
        billboarder.billboardY = false;
        if(b_anim_idle){
            if(!b_anim_spec1){
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
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
            else{
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
                    animator.Play("idle_t_hurt");
                }
                else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                    animator.Play("idle_L_hurt");
                }
                else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                    animator.Play("idle_R_hurt");
                }
                else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                    animator.Play("idle_b_hurt");
                }
                else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                    animator.Play("idle_f_hurt");
                }  
            }
        }
        if(b_anim_walk){
            if(!b_anim_spec1){
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    //animator.Play("walk_t");
                    billboarder.billboardY = true;
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
        
            else{
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    //animator.Play("walk_t");
                    billboarder.billboardY = true;
                    animator.Play("idle_t_hurt");
                }
                else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                    //animator.Play("walk_L_hurt");
                    animator.Play("idle_L_hurt");
                }
                else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                    //animator.Play("walk_R");
                    animator.Play("idle_R_hurt");
                }
                else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                    //animator.Play("walk_b");
                    animator.Play("idle_L_hurt");
                }
                else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                    animator.Play("walk_f_hurt");
                }  
            }
        }
        if(b_anim_dying){
            if(!b_anim_spec1){
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
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
        
            else{
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
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
                    animator.Play("dying_f_hurt");
                }  
            }
        }
        if(is_dead){
            if(!b_anim_spec1){
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
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
        
            else{
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
                    animator.Play("dead_t_hurt");
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
                    animator.Play("dead_f_hurt");
                }  
            }
        }
        if(b_anim_casting){
             if(!b_anim_spec1){
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
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
        
            else{
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
                    animator.Play("casting_hurt");
                }
                else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                    animator.Play("casting_hurt");
                }
                else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                    animator.Play("casting_hurt");
                }
                else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                    animator.Play("casting_hurt");
                }
                else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                    animator.Play("casting_hurt");
                }  
            }
        }
         if(b_anim_attk){
             if(!b_anim_spec1){
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
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
        
            else{
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
                    animator.Play("attack_f_hurt");
                }
                else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                    animator.Play("attack_f_hurt");
                }
                else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                    animator.Play("attack_f_hurt");
                }
                else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                    animator.Play("attack_f_hurt");
                }
                else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                    animator.Play("attack_f_hurt");
                }  

            }
         }
        if(b_anim_hurt){
             if(!b_anim_spec1){
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
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
        
            else{
                if(BillboardHelper.AngleIsAbove(this.transform,camera_transform)){
                    billboarder.billboardY = true;
                    animator.Play("idle_t_hurt");
                }
                else if(BillboardHelper.AngleIsLeft(this.transform,camera_transform)){
                    animator.Play("damage_L_hurt");
                }
                else if(BillboardHelper.AngleIsRight(this.transform,camera_transform)){
                    animator.Play("damage_r");
                }
                else if(BillboardHelper.AngleIsBehind(this.transform,camera_transform)){
                    animator.Play("damage_b_hurt");
                }
                else if(BillboardHelper.AngleIsFront(this.transform,camera_transform)){
                    animator.Play("damage_f_hurt");
                }  

            }
         }
    }

}

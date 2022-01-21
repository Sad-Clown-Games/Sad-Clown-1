using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Menu_Option : MonoBehaviour
{
    public Animator animator;
    public UnityEngine.UI.Image sprite;
    public bool hovered;
    public string option_name;
    public string option_description;
    public GameObject left_adjacent;
    public GameObject right_adjacent;
    public GameObject up_adjacent;
    public GameObject down_adjacent;
    public GameObject selector;
    public Item item;
    public Player_Character character;
    public Action action;
    private int store_idx = -1;
    public TMPro.TMP_Text text;
    public Sprite icon;
    public bool is_active;
    public bool left_col;
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

    public void Show_Selector(){
        if(selector)
            selector.SetActive(true);
    }
    public void Hide_Selector(){
        if(selector)
            selector.SetActive(false);
    }
    public void Hide_All(){
        Hide_Selector();
        hovered = false;
    }

    public void Gray_Out(){
        sprite.color = Color.gray;
    }
    public void UnGray(){
        sprite.color = Color.white;
    }

    
    public void Activate(){
        is_active = true;
        this.gameObject.SetActive(true);
    }
    public void Activate(int idx){
        store_idx = idx;
        is_active = true;
        this.gameObject.SetActive(true);
    }
    //activate with item
    public void Activate(Item i,int idx, int option_idx, int colsize, List<Menu_Option> items){
        if(left_col){
            right_adjacent = items.ElementAtOrDefault(option_idx+1)?.gameObject ?? null;
        }
        else{
            left_adjacent = items.ElementAtOrDefault(option_idx-1)?.gameObject ?? null;
        }
        up_adjacent = items.ElementAtOrDefault(option_idx-2)?.gameObject ?? null;
        down_adjacent = items.ElementAtOrDefault(option_idx+2)?.gameObject ?? null;
        
        store_idx = idx;
        is_active = true;
        this.item = i;
        text.text = i.action_name;
        option_description = i.description;
        this.gameObject.SetActive(true);
    }

    //activate with ability
    public void Activate(Action a,int idx, int option_idx, int colsize, List<Menu_Option> actions){
        if(left_col){
            right_adjacent = actions.ElementAtOrDefault(option_idx+1)?.gameObject ?? null;
        }
        else{
            left_adjacent = actions.ElementAtOrDefault(option_idx-1)?.gameObject ?? null;
        }
        up_adjacent = actions.ElementAtOrDefault(option_idx-2)?.gameObject ?? null;
        down_adjacent = actions.ElementAtOrDefault(option_idx+2)?.gameObject ?? null;
        
        store_idx = idx;
        is_active = true;
        this.action = a;
        text.text = a.action_name;
        option_description = a.description;
        this.gameObject.SetActive(true);
    }

    public void Activate(Player_Character c){
        character = c;
        if(text)
            text.text = c.battle_object.combatant_name;
        option_description = c.battle_object.combatant_title;
        icon = c.battle_object.portrait;
        is_active = true;
        this.gameObject.SetActive(true);
    }
    public void Activate(Action a){
        action = a;
        text.text = a.action_name;
        option_description = a.description;
        is_active = true;
        this.gameObject.SetActive(true);
    }

    public void Deactivate(){
        item = null;
        store_idx = -1;
        Hide_Selector();
        this.gameObject.SetActive(false);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/*
Singleton responsible for scene changes, saving data, and pulling data.
Add this to every scene.
*/
public class Game_Manager : MonoBehaviour
{

    public Player_Controller player;
    public Transform player_t;
    //Static reference
    public static Game_Manager Instance;

    [Header("Player Data")]
    public Player_Data player_data;

    [Header("Characters")]
    //we need a way to represent active party members, and also access stats at all times in a reasonable way
    //with no duplicate "player character" classes
    public GameObject gameobject_alpha;
    public GameObject gameobject_mimosa;
    public GameObject gameobject_conner;
    public GameObject gameobject_asher;
    public GameObject gameobject_judas;
    public GameObject gameobject_ferdinand;
    public GameObject placeholder7;
    public GameObject placeholder8;
    public Action_Registry action_registry;
    public Input_Controller input_controller;
    public Camera_Manager camera_manager;
    //Object containing all helper functions script
    public Party_Controller party_controller;
    public bool finished_loading;
    public CrossFader cross_fader;

    //public List<Player_Character> party_list;
    //private List<Player_Character> data_ordered_party_list; //stupid ass implimentation but basically matches the order thatt Player Data has

    //debug save game in inspector;

    [System.Serializable]
    public enum Location_State
    {
           LoadSave,
           NewMap,
           FromBattle,
           ToBattle
    }
    public int lifetime;
    public bool load_from_save;

    void Awake()
    {
        /*//Let the gameobject persist over the scenes
        //Check if the Instance instance is null
        if(Instance != null && Instance != this){
            Destroy(this.gameObject);
        }
        else
        {
            //This instance becomes the single instance available
            
        }
        Debug.Log("GameManager Awaken");
        DontDestroyOnLoad(this.gameObject);*/
        Instance = this;
        Startup();
    }

    public void Startup(){
        //Copy data from our 
        lifetime++;
        finished_loading = false;
        Load_Session();
        Populate_Party_Controller(); //populates data from our current savegame
        finished_loading = true;
        if(load_from_save){
            SetLocation_State(0);
        }
        Relocate_Player();
    }

    private void Relocate_Player(){
        Debug.Log(GetLocation_State());
        switch (GetLocation_State())
        {
            case Location_State.LoadSave:
                TeleportPlayer(GetSave_Scene().location);
                break;
            case Location_State.FromBattle:
                TeleportPlayer(GetLast_Scene().location);
                break;
            case Location_State.NewMap:
                TeleportPlayer(GetNext_Scene().location);
                break;
            case Location_State.ToBattle:
                break;
            default:
                TeleportPlayer(GetSave_Scene().location);
                break;
        }
    }
    
    public void Populate_Party_Controller(){
        party_controller = GameObject.FindGameObjectWithTag("Party_Manager").GetComponent<Party_Controller>();
        //copy data from Player_Data into each party member
        List<Player_Character> party_list = new List<Player_Character>();
        party_list.Add(Instantiate(gameobject_alpha).GetComponent<Player_Character>());
        party_list.Add(Instantiate(gameobject_mimosa).GetComponent<Player_Character>());
        party_list.Add(Instantiate(gameobject_conner).GetComponent<Player_Character>());
        party_list.Add(Instantiate(gameobject_asher).GetComponent<Player_Character>());
        party_list.Add(Instantiate(gameobject_judas).GetComponent<Player_Character>());
        party_list.Add(Instantiate(gameobject_ferdinand).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder7).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder8).GetComponent<Player_Character>());
        party_controller.InitializeLists(party_list);
        party_controller.Populate_Party_Data(player_data.save_data.party_data);
    }

    //it'll really break some shit if the order ever gets disrupted so this is a super headass way to do this,
    //But I dug this hole, and it's honestly pretty shallow, this is probably as bad as it'll get.
    public void Update_Party_Data(List<Player_Character> party){
        player_data.save_data.party_data.alpha.stats = party[0].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.mimosa.stats = party[1].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.conner.stats = party[2].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.asher.stats = party[3].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.judas.stats = party[4].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.ferdinand.stats = party[5].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.placeholder7.stats = party[6].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.placeholder8.stats = party[7].battle_object.GetCharacter_Stats();
    }

    public void Save_Game(){
        Transfer_Active_Party_Data_To_Saved_Party_Data();
        SetSaveScene();
        Save_Load save = new Save_Load();
        save.Save(player_data.save_data,"gamesave");
    }
    public void Load_Game(){
        Save_Load load = new Save_Load();
        player_data.save_data = load.Load("gamesave");
        player_data.items = action_registry.Get_Items_By_Names(player_data.save_data.saved_items.items);
        SceneTransferHelper.GoToWorldScene(GetSave_Scene().scene_name,GetSave_Scene().location);
    }

    //Saving and loading session keeps state between scene, but not the savegame
    public void Save_Session(){
        Transfer_Active_Party_Data_To_Saved_Party_Data();
        if(player != null)
            SetLastScene();
        Save_Load save = new Save_Load();
        save.Save(player_data.save_data,"session");
    }
    //load_seession is called 
    public void Load_Session(){
        Save_Load load = new Save_Load();
        player_data.save_data = load.Load("session");
        player_data.items = action_registry.Get_Items_By_Names(player_data.save_data.saved_items.items);
    }

    public Location_State GetLocation_State(){
        return (Location_State)player_data.save_data.location_state;
    }

    public void SetLocation_State(int state){
        player_data.save_data.location_state = state;
    }

    public void SetLastScene(){
        //if was in battle sequence ignore
        if(GetLocation_State() == Location_State.ToBattle){
            return; //in battle
        }
        GameObject player_object = GameObject.FindGameObjectWithTag("Player");
        player = player_object.GetComponent<Player_Controller>();
        Scene_Loc scene = new Scene_Loc{
            location = player.transform.position,
            scene_name = SceneManager.GetActiveScene().name,
        };
        player_data.save_data.Set_Last_Scene(scene);
    }

    public void SetNextScene(Scene_Loc scene){
        player_data.save_data.Set_Next_Scene(scene);
    }

    public void SetSaveScene(){
        if(!player){
            return; //in battle scene
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();
        Scene_Loc scene = new Scene_Loc{
            location = player.transform.position,
            scene_name = SceneManager.GetActiveScene().name,
        };
        player_data.save_data.Set_Save_Scene(scene);
    }

    public Scene_Loc GetLast_Scene(){
        return player_data.save_data.last_scene;
    }

    public Scene_Loc GetSave_Scene(){
        return player_data.save_data.save_scene;
    }

    public Scene_Loc GetNext_Scene(){
        return player_data.save_data.next_scene;
    }

    public inputs Get_Inputs(){
        return input_controller.Get_Inputs();
    }

    public void TeleportPlayerLastGrounded(){
        TeleportPlayer(player.GetLastGroundedLoc());
    }


    public void TeleportPlayer(Vector3 location){
        Debug.Log(location);
        Debug.Log("Teleporting Player");
        if(player == null){ //no player has been located
            Debug.Log("No Player in Scene, or Missing Reference!");
            return;
        }
        player_t = player.transform;
        player_t.position = location;
        Physics.SyncTransforms();
    }
    public void Disable_Player_Control(){
        player.Lock_Controls();
    }
    public void Enable_Player_Control(){
        player.UnLock_Controls();
    }
    public void Global_Disable_AI(){

    }
    public void Global_Enable_AI(){
        
    }
    public List<Item> Get_Items(){
        Update_Item_Indexes(); //make sure the working indexes are correct
        return player_data.items;
    }
    public void Equip_Character(Player_Character character,Item item,string mode = "none"){
        Item cur_equip;
        switch(mode)
        {
            case "Weapon":
                cur_equip = character.Get_Weapon();
                break;
            case "Armor":
                cur_equip = character.Get_Armor();
                break;
            case "Trinket":
                cur_equip = character.Get_Trinket();
                break;
            default:
                Debug.Log("Equip_Player: ERROR: invalid mode");
                return;
        }
        cur_equip = player_data.items.Remove_Or_Swap_Item(cur_equip,item.original_list_idx);
        switch(mode)
        {
            case "Weapon":
                character.Equip_Weapon(cur_equip);
                break;
            case "Armor":
                character.Equip_Armor(cur_equip);
                break;
            case "Trinket":
                character.Equip_Trinket(cur_equip);
                break;
            default:
                cur_equip = null;
                break;
        }
    }
    public void Unequip_Character(Player_Character character, string mode = "none"){
        Item cur_equip;
        switch(mode)
        {
            case "Weapon":
                cur_equip = character.Get_Weapon();
                character.Equip_Weapon(null);
                break;
            case "Armor":
                cur_equip = character.Get_Armor();
                character.Equip_Armor(null);
                break;
            case "Trinket":
                cur_equip = character.Get_Trinket();
                character.Equip_Trinket(null);
                break;
            default:
                Debug.Log("Equip_Player: ERROR: invalid mode");
                return;
        }
        player_data.items.Add(cur_equip);
    }
    public void Update_Item_Indexes(){
        for(int i = 0; i < player_data.items.Count;i++){
                    player_data.items[i].original_list_idx = i;
        }
    }

    public void Add_Item(string item_name){
        player_data.items.Add(action_registry.Get_Item_By_Name(item_name));
    }
    public void Remove_Item(int idx){
        player_data.items.RemoveAt(idx);
    }

    public void Freeze_Time(){
        Time.timeScale = 0f;
    }
    public void Resume_Time(){
        Time.timeScale = 1f;
    }
    public void Disable_Input_Control(){
        input_controller.Disable();
    }

    public void Enable_Input_Control(){
        input_controller.Enable();
    }

    public void Move_To_New_Map(string new_scene_name,Vector3 location){
        //do local saving tasks
        StartCoroutine(_Move_To_New_Map(new_scene_name,location));
    }
    public IEnumerator _Move_To_New_Map(string new_scene_name,Vector3 location){
        Disable_Input_Control();
        Exit_Scene_FX();
        yield return new WaitForSeconds(1.5f);
        SceneTransferHelper.GoToWorldScene(new_scene_name, location);
    }

    public void Exit_Scene_FX(){
        cross_fader.CrossFade_Out();
    }

    public void SetState_FromBattle(){
        SetLocation_State((int)Location_State.FromBattle);
    }

    public void SetState_ToBattle(){
        SetLocation_State((int)Location_State.ToBattle);
    }

    public void SetState_NewMap(){
        SetLocation_State((int)Location_State.NewMap);
    }

    public void SetState_LoadSave(){
        SetLocation_State((int)Location_State.LoadSave);
    }

    public void Transfer_Active_Party_Data_To_Saved_Party_Data(){
        Update_Party_Data(party_controller.data_ordered_party_list);
    }

}

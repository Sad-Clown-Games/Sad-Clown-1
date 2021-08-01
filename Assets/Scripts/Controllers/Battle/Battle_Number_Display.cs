using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battle_Number_Display : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public Mesh mesh;
    public Vector3[] vertices;
    public Vector3[] orig_vertices;

    public int value;
    public bool rainbow_text;
    public float alive_time = 1.0f;
    private void Start() {
        textMeshPro.ForceMeshUpdate();
        mesh = textMeshPro.mesh;
        orig_vertices = mesh.vertices;
    }
    private void Update() {
        if(rainbow_text){
            //do nothing because fuck me lol
        }
        mesh = textMeshPro.mesh;
        vertices = mesh.vertices;
        for(int i = 0; i < textMeshPro.textInfo.characterCount; i++){
            TMP_CharacterInfo c = textMeshPro.textInfo.characterInfo[i];
            int index = c.vertexIndex;
            float rate = .3f - Vector3.Distance(vertices[index],orig_vertices[index]);
            if(rate < 0)
                rate = 0;
            Vector3 offset = new Vector2(0,(5+index)*Time.deltaTime*rate);
            vertices[index] -= offset;
            vertices[index+1] -= offset;
            vertices[index+2] -= offset;
            vertices[index+3] -= offset;

        }
        mesh.vertices = vertices;
        Invoke("Die",alive_time);
    }

    public void Set_Value(int value){
        if(value < 1){ //death
            this.value = -1;
            textMeshPro.text = "DEATH";
            return;
        }
        this.value = value;
        textMeshPro.text = this.value.ToString();
    }

    public void Set_Healing_Gradient(){
        VertexGradient newGrad = new VertexGradient(Color.green,Color.yellow,Color.green,Color.green);
        textMeshPro.colorGradient = newGrad;
    }

    public void Set_Mental_Gradient(){
        VertexGradient newGrad = new VertexGradient(Color.green,Color.green,Color.blue,Color.blue);
        textMeshPro.colorGradient = newGrad;
    }

    public void Set_Mental_Dmg_Gradient(){
        VertexGradient newGrad = new VertexGradient(Color.white,Color.grey,Color.black,Color.blue);
        textMeshPro.colorGradient = newGrad;
    }

    public void Set_Damage_Gradient(){
        VertexGradient newGrad = new VertexGradient(Color.red,Color.white,Color.red,new Color(255,151,0));
        if(value < 0)//death
            Set_Death_Gradient();
        if(value > 100)
            newGrad = new VertexGradient(Color.red,new Color(255,151,0),new Color(255,151,0),Color.yellow);
        if(value > 300)
            newGrad = new VertexGradient(Color.white,Color.red,Color.red,new Color(255,151,0));
        if(value > 500)
            newGrad = new VertexGradient(Color.black,Color.red,Color.red,Color.red);
        if(value >= 999){
            rainbow_text = true;
        }
        textMeshPro.colorGradient = newGrad;
    }

    public void Set_Death_Gradient(){
        VertexGradient newGrad = new VertexGradient(Color.red,Color.red,Color.red,Color.black);
        textMeshPro.colorGradient = newGrad;
    }

    private void Die(){
        Destroy(this.gameObject);
    }

    private Vector2 Drop(int i){
        return new Vector2((1+i)*Time.deltaTime, (1+i)*Time.deltaTime);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDissolve : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Material material;
    [SerializeField]
    private GameObject main_camera;
    [SerializeField]
    private Collider mesh_collider;
    [SerializeField]
    private float activation_distance = 2;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private float tiling_factor;
    void Start()
    {
        main_camera = GameObject.FindGameObjectWithTag("MainCamera");
        material = GetComponent<MeshRenderer>().material;
        mesh_collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_TilingFactor",tiling_factor);
        material.SetVector("_Offset",offset);
        Vector3 cam_loc = main_camera.transform.position;
        Vector3 closest_vert = mesh_collider.ClosestPoint(cam_loc); //expensive if big mesh
        float distance = Vector3.Distance(cam_loc,closest_vert); 
        material.SetFloat("_Fade",distance/activation_distance);
    }
}
